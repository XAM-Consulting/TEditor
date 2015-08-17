using System;
using System.Reflection;
using System.IO;

namespace TEditor
{
	public partial class TEditor
	{
		public TEditor ()
		{
			EditorLoaded = false;
			FormatHTML = false;
			InternalHTML = string.Empty;
		}

		public string InternalHTML { get; set; }

		public bool EditorLoaded { get; set; }

		public bool FormatHTML { get; set; }

		public string LoadResources()
		{
			var assembly = typeof(TEditor).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("TEditor.EditorResources.editor.html");
			string htmlData = "";
			using (var reader = new System.IO.StreamReader (stream,System.Text.Encoding.UTF8)) {
				htmlData = reader.ReadToEnd ();
			}
			string jsData = "";
			stream = assembly.GetManifestResourceStream("TEditor.EditorResources.ZSSRichTextEditor.js");
			using (var reader = new System.IO.StreamReader (stream,System.Text.Encoding.UTF8)) {
				jsData = reader.ReadToEnd ();
			}
			return htmlData.Replace ("<!--editor-->", jsData);
		}
	}
}

