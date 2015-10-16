using System;
using TEditorForms.Droid;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Content;
using TEditor.Droid;
using TEditor.Forms.Sample;

[assembly: Dependency (typeof(TEditorService))]
namespace TEditorForms.Droid
{
	public class TEditorService : ITEditorService
	{
		public TEditorService ()
		{
		}

		public Task<string> ShowTEditor (string html)
		{
			TaskCompletionSource<string> taskRes = new TaskCompletionSource<string> ();

			var tActivity = new Intent (Forms.Context , typeof(TEditorActivity));
			//				tActivity.PutExtra ("ToolbarStyle", "Basic");
			//				tActivity.PutExtra ("ToolbarStyle", "Standard");
			tActivity.PutExtra ("ToolbarStyle", "All");
			tActivity.PutExtra("HTMLString", "<!-- This is an HTML comment --><p>This is a test of the <strong>TEditor</strong> by <a title=\"XAM consulting\" href=\"http://www.xam-consulting.com\">XAM consulting</a></p>");
			var acitivity = Forms.Context as Android.App.Activity;
			if (acitivity != null)
				acitivity.StartActivityForResult (tActivity, 0);
			
			return taskRes.Task;
		}
	}
}

