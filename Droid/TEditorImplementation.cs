using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using TEditor.Abstractions;

namespace TEditor
{
    public class TEditorImplementation : BaseTEditor
    {
        public static ToolbarBuilder ToolbarBuilder = null;
        public override Task<string> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null)
        {
            var result = new TaskCompletionSource<string>();

            var tActivity = new Intent(Application.Context, typeof(TEditorActivity));
            ToolbarBuilder = toolbarBuilder;
            if (ToolbarBuilder == null)
                ToolbarBuilder = new ToolbarBuilder().AddAll();
            tActivity.PutExtra("HTMLString", html);
            tActivity.SetFlags(ActivityFlags.NewTask);
            TEditorActivity.SetOutput = (res, resStr) =>
            {
                TEditorActivity.SetOutput = null;
                if (res)
                {
                    result.SetResult(resStr);
                }
                else
                    result.SetResult(string.Empty);
            };
            Application.Context.StartActivity(tActivity);
            return result.Task;
        }

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ToolbarBuilder = null;
        }
    }
}
