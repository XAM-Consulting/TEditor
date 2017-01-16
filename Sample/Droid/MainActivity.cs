using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;

namespace TEditor.Forms.Sample.Droid
{
	[Activity (Label = "TEditor.Forms.Sample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new App ());		
		}			

		//protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		//{
		//	base.OnActivityResult (requestCode, resultCode, data);
		//	if (resultCode == Result.Ok) {
		//		if (data != null) {
		//			string html = data.GetStringExtra ("HTMLString");
		//			var editor = (DependencyService.Get<ITEditorService> (DependencyFetchTarget.GlobalInstance) as TEditorService);
		//			if (editor != null) {
		//				editor.TaskResult.SetResult (html);
		//			}
		//		}
		//	}
		//}
	}
}

