using System;
using System.Threading.Tasks;

namespace TEditor.Abstractions
{
    public interface ITEditor : IDisposable
    {
        Task<string> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null);
    }
}
