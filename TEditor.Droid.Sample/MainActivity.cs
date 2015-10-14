using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TEditor.Droid.Sample
{
	[Activity (Label = "TEditor.Droid.Sample", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				var tActivity = new Intent (this, typeof(TEditorActivity));
				//				tActivity.PutExtra ("ToolbarStyle", "Basic");
				//				tActivity.PutExtra ("ToolbarStyle", "Standard");
				tActivity.PutExtra ("ToolbarStyle", "All");
				tActivity.PutExtra("HTMLString", "<!-- This is an HTML comment --><p>This is a test of the <strong>TEditor</strong> by <a title=\"XAM consulting\" href=\"http://www.xam-consulting.com\">XAM consulting</a></p>");
				StartActivity (tActivity);
			};
		}
	}
}


