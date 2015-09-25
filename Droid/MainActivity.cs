using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TEditor.Droid
{
	[Activity (Label = "TEditor.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
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
				tActivity.PutExtra ("ToolbarStyle", "Standard");
//				tActivity.PutExtra ("ToolbarStyle", "All");
				StartActivity (tActivity);
			};
		}
	}
}


