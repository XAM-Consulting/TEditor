using System;
using System.Threading.Tasks;

namespace TEditor.Forms.Sample
{
	public interface ITEditorService
	{
		Task<string> ShowTEditor(string html);
	}
}

