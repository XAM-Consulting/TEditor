using System;
using Android.App;
using Android.Widget;

namespace TEditor.Droid
{
	[Activity (Label = "TEditorActivity")]
	public class TEditorActivity : Activity
	{
		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.TEditorActivity);


			TEditorWebView editorWebView = FindViewById<TEditorWebView> (Resource.Id.EditorWebView);
			Button boldbutton = FindViewById<Button> (Resource.Id.BlodButton);

			boldbutton.Click += delegate {
				editorWebView.SetBold();
			};

			Button Italicbutton = FindViewById<Button> (Resource.Id.BlodButton);

			Italicbutton.Click += delegate {
				editorWebView.SetItalic();
			};

			editorWebView.SetHTML("<!-- This is an HTML comment --><p>This is a test of the <strong>ZSSRichTextEditor</strong> by <a title=\"Zed Said\" href=\"http://www.zedsaid.com\">Zed Said Studio</a></p>");
		}
	}
}

