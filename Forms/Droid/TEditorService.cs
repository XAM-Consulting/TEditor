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
		public TaskCompletionSource<string> TaskResult;
		public TEditorService ()
		{
		}

		public Task<string> ShowTEditor (string html)
		{
			TaskResult = null;
			TaskResult = new TaskCompletionSource<string> ();

			var tActivity = new Intent (Forms.Context , typeof(TEditorActivity));
			//				tActivity.PutExtra ("ToolbarStyle", "Basic");
			//				tActivity.PutExtra ("ToolbarStyle", "Standard");
			tActivity.PutExtra ("ToolbarStyle", "All");
			tActivity.PutExtra("HTMLString", html);
			var acitivity = Forms.Context as Android.App.Activity;
			if (acitivity != null)
				acitivity.StartActivityForResult (tActivity, 0);
			
			return TaskResult.Task;
		}			
	}
}

