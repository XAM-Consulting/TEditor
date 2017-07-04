using System;
using Android.App;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Content;
using Android.Support.V7.App;
using TEditor.Abstractions;

namespace TEditor
{
    [Activity(Label = "TEditorActivity",
        WindowSoftInputMode = Android.Views.SoftInput.AdjustResize | Android.Views.SoftInput.StateHidden,
        Theme = "@style/Theme.AppCompat.NoActionBar.FullScreen")]
    public class TEditorActivity : Activity
    {
        const int ToolbarFixHeight = 60;
        TEditorWebView _editorWebView;
        LinearLayoutDetectsSoftKeyboard _rootLayout;
        LinearLayout _toolbarLayout;
        Android.Support.V7.Widget.Toolbar _topToolBar;

        public static Action<bool, string> SetOutput { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TEditorActivity);

            _topToolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.TopToolbar);
            _topToolBar.Title = CrossTEditor.PageTitle;
            _topToolBar.InflateMenu(Resource.Menu.TopToolbarMenu);
            _topToolBar.MenuItemClick += async (sender, e) =>
            {
                if (SetOutput != null)
                {
                    if (e.Item.TitleFormatted.ToString() == "Save")
                    {
                        string html = await _editorWebView.GetHTML();
                        SetOutput.Invoke(true, html);
                    }
                    else {
                        SetOutput.Invoke(false, null);
                    }
                }
                Finish();
            };

            _rootLayout = FindViewById<LinearLayoutDetectsSoftKeyboard>(Resource.Id.RootRelativeLayout);
            _editorWebView = FindViewById<TEditorWebView>(Resource.Id.EditorWebView);
            _toolbarLayout = FindViewById<LinearLayout>(Resource.Id.ToolbarLayout);

            _rootLayout.onKeyboardShown += HandleSoftKeyboardShwon;
            _editorWebView.SetOnCreateContextMenuListener(this);

            BuildToolbar();

            string htmlString = Intent.GetStringExtra("HTMLString") ?? "<p></p>";
            _editorWebView.SetHTML(htmlString);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _rootLayout.onKeyboardShown -= HandleSoftKeyboardShwon;
        }

        public void BuildToolbar()
        {
            ToolbarBuilder builder = TEditorImplementation.ToolbarBuilder;
            if (builder == null)
                builder = new ToolbarBuilder().AddAll();

            foreach (var item in builder)
            {
                ImageButton imagebutton = new ImageButton(this);
                imagebutton.Click += (sender, e) =>
                {
                    item.ClickFunc.Invoke(_editorWebView.RichTextEditor);
                };
                string imagename = item.ImagePath.Split('.')[0];
                int resourceId = (int)typeof(Resource.Drawable).GetField(imagename).GetValue(null);
                imagebutton.SetImageResource(resourceId);
                var toolbarItems = FindViewById<LinearLayout>(Resource.Id.ToolbarItemsLayout);
                toolbarItems.AddView(imagebutton);
            }
        }

        public void HandleSoftKeyboardShwon(bool shown, int newHeight)
        {
            if (shown)
            {
                _toolbarLayout.Visibility = Android.Views.ViewStates.Visible;
                int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                _toolbarLayout.Measure(widthSpec, heightSpec);
                int toolbarHeight = _toolbarLayout.MeasuredHeight == 0 ? (int)(ToolbarFixHeight * Resources.DisplayMetrics.Density) : _toolbarLayout.MeasuredHeight;
                int topToolbarHeight = _topToolBar.MeasuredHeight == 0 ? (int)(ToolbarFixHeight * Resources.DisplayMetrics.Density) : _topToolBar.MeasuredHeight;
                int editorHeight = newHeight - toolbarHeight - topToolbarHeight;
                _editorWebView.LayoutParameters.Height = editorHeight;
                _editorWebView.LayoutParameters.Width = LinearLayout.LayoutParams.MatchParent;
                _editorWebView.RequestLayout();
            }
            else {
                if (newHeight != 0)
                {
                    _toolbarLayout.Visibility = Android.Views.ViewStates.Invisible;
                    _editorWebView.LayoutParameters = new LinearLayout.LayoutParams(-1, -1);
                    ;
                }
            }
        }

    }
}

