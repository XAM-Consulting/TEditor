using System;
using System.Threading.Tasks;

namespace TEditor.Abstractions
{
    public interface ITEditor : IDisposable
    {
        Task<TEditorResponse> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null, bool autoFocusInput = false);
    }
}
