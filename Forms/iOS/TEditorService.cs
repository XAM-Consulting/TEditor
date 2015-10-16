using System;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using TEditor.iOS;
using TEditor;
using UIKit;
using Xamarin.Forms;
using TEditor.Forms.Sample;
using TEditorForms.iOS;

[assembly: Dependency (typeof(TEditorService))]
namespace TEditorForms.iOS
{
	public class TEditorService : ITEditorService
	{
		public TEditorService ()
		{
		}

		public Task<string> ShowTEditor (string html)
		{
			TaskCompletionSource<string> taskRes = new TaskCompletionSource<string> ();
			var tvc = new TEditorViewController ();
			ToolbarBuilder builder = new ToolbarBuilder ().AddAll ();
			tvc.BuildToolbar (builder);
			tvc.SetHTML (html);

			UINavigationController nav = null;
			foreach (var vc in 
				UIApplication.SharedApplication.Windows[0].RootViewController.ChildViewControllers) {
				if (vc is UINavigationController)
					nav = (UINavigationController)vc;
			}

			tvc.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem ("Done", UIBarButtonItemStyle.Done, async (item, args) => {
				if (nav != null)
					nav.PopViewController (true);
				taskRes.SetResult (await tvc.GetHTML ());
			}), true);

			if (nav != null)
				nav.PushViewController (tvc, true);
			return taskRes.Task;
		}

	}
}

