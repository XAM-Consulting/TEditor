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

            UINavigationController nav = null;
            foreach (var vc in
                UIApplication.SharedApplication.Windows[0].RootViewController.ChildViewControllers)
            {
                if (vc is UINavigationController)
                    nav = (UINavigationController)vc;
            }

            tvc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, async (item, args) =>
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
