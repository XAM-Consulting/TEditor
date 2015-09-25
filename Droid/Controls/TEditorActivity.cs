using System;
using Android.App;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;

namespace TEditor.Droid
{
	[Activity (Label = "TEditorActivity", 
		WindowSoftInputMode = Android.Views.SoftInput.AdjustResize | Android.Views.SoftInput.StateVisible,
		Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
	public class TEditorActivity : Activity
	{
		const int ToolbarFixHeight = 144;
		TEditorWebView _editorWebView;
		LinearLayoutDetectsSoftKeyboard _rootLayout;
		LinearLayout _toolbarLayout;

		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.TEditorActivity);

			_rootLayout = FindViewById<LinearLayoutDetectsSoftKeyboard> (Resource.Id.RootRelativeLayout);
			_editorWebView = FindViewById<TEditorWebView> (Resource.Id.EditorWebView);
			_toolbarLayout = FindViewById<LinearLayout> (Resource.Id.ToolbarLayout);

			_rootLayout.onKeyboardShown += HandleSoftKeyboardShwon;
			_editorWebView.SetOnCreateContextMenuListener (this);

			string toolbarStyle = Intent.GetStringExtra ("ToolbarStyle") ?? "Basic";
			switch (toolbarStyle) {
			case "Basic":
				BuildToolbar (new ToolbarBuilder ().AddBasic ());
				break;
			case "Standard":
				BuildToolbar (new ToolbarBuilder ().AddStandard ());
				break;
			case "All":
				BuildToolbar (new ToolbarBuilder ().AddAll ());
				break;
			default:
				BuildToolbar (new ToolbarBuilder ().AddBasic ());
				break;
			}

			_editorWebView.SetHTML ("<!-- This is an HTML comment --><p>This is a test of the <strong>TEditor</strong> by <a title=\"XAM consulting\" href=\"http://www.xam-consulting.com\">XAM consulting</a></p>");

		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			_rootLayout.onKeyboardShown -= HandleSoftKeyboardShwon;
		}

		public void BuildToolbar (ToolbarBuilder builder)
		{
			foreach (var item in builder) {
				ImageButton imagebutton = new ImageButton (this);
				imagebutton.Click += (sender, e) => {
					item.ClickFunc.Invoke (_editorWebView.RichTextEditor);
				};
				string imagename = item.ImagePath.Split ('.') [0];
				int resourceId = (int)typeof(Resource.Drawable).GetField (imagename).GetValue (null);
				imagebutton.SetImageResource (resourceId);
				var toolbarItems = FindViewById<LinearLayout> (Resource.Id.ToolbarItemsLayout);
				toolbarItems.AddView (imagebutton);
			}
		}

		public void HandleSoftKeyboardShwon (bool shown, int newHeight)
		{			
			if (shown) {
				_toolbarLayout.Visibility = Android.Views.ViewStates.Visible;
				int toolbarHeight = _toolbarLayout.MeasuredHeight == 0 ? ToolbarFixHeight : _toolbarLayout.MeasuredHeight;
				int editorHeight = newHeight - toolbarHeight;
				_editorWebView.LayoutParameters.Height = editorHeight;
				_editorWebView.LayoutParameters.Width = LinearLayout.LayoutParams.MatchParent;
				_editorWebView.RequestLayout ();
			} else {
				if (newHeight != 0) {
					_toolbarLayout.Visibility = Android.Views.ViewStates.Invisible;
					_editorWebView.LayoutParameters= new LinearLayout.LayoutParams (-1,-1);;
				}
			}
		}
			
	}
}

