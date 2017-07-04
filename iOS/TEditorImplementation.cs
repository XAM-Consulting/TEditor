using System;
using System.Threading.Tasks;
using TEditor.Abstractions;
using UIKit;

namespace TEditor
{
    public class TEditorImplementation : BaseTEditor
    {
        public override Task<string> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null)
        {
            TaskCompletionSource<string> taskRes = new TaskCompletionSource<string>();
            var tvc = new TEditorViewController();
            ToolbarBuilder builder = toolbarBuilder;
            if (toolbarBuilder == null)
                builder = new ToolbarBuilder().AddAll();
            tvc.BuildToolbar(builder);
            tvc.SetHTML(html);
            tvc.Title = CrossTEditor.PageTitle;

            UINavigationController nav = null;
            foreach (var vc in
                UIApplication.SharedApplication.Windows[0].RootViewController.ChildViewControllers)
            {
                if (vc is UINavigationController)
                    nav = (UINavigationController)vc;
            }
            tvc.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(CrossTEditor.CancelText, UIBarButtonItemStyle.Plain, (item, args) =>
            {
				if (nav != null)
					nav.PopViewController(true);
				taskRes.SetResult(string.Empty);
            }), true);
            tvc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(CrossTEditor.SaveText, UIBarButtonItemStyle.Done, async (item, args) =>
            {
                if (nav != null)
                    nav.PopViewController(true);
                taskRes.SetResult(await tvc.GetHTML());
            }), true);

            if (nav != null)
                nav.PushViewController(tvc, true);
            return taskRes.Task;
        }
    }
}
