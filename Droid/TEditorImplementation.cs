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
        public override Task<TEditorResponse> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null, bool autoFocusInput = false)
        {
            var result = new TaskCompletionSource<TEditorResponse>();

            var tActivity = new Intent(Application.Context, typeof(TEditorActivity));
            ToolbarBuilder = toolbarBuilder;
            if (ToolbarBuilder == null)
                ToolbarBuilder = new ToolbarBuilder().AddAll();
            tActivity.PutExtra("HTMLString", html);
            tActivity.PutExtra("AutoFocusInput", autoFocusInput);
            tActivity.SetFlags(ActivityFlags.NewTask);
            TEditorActivity.SetOutput = (res, resStr) =>
            {
                TEditorActivity.SetOutput = null;
                if (res)
                {
                    result.SetResult(new TEditorResponse() { IsSave = true, HTML = resStr});
                }
                else
                    result.SetResult(new TEditorResponse() { IsSave = false, HTML = string.Empty });
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
