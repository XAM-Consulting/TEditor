using System;

namespace TEditor
{
	internal interface ITEditorAPI
	{
		void AlignFull ();

		void AlignLeft ();

		void AlignRight ();

		string GetHTML ();

		void Heading1 ();

		void Heading2 ();

		void Heading3 ();

		void Heading4 ();

		void Heading5 ();

		void Heading6 ();

		void InsertHTML (string html);

		void Paragraph ();

		void RemoveFormat ();

		void SetBold ();

		void SetHR ();

		void SetIndent ();

		void SetItalic ();

		void SetOrderedList ();

		void SetOutdent ();

		void SetStrikethrough ();

		void SetSubscript ();

		void SetSuperscript ();

		void SetUnderline ();

		void SetUnorderedList ();

		void UpdateHTML ();

		void SetPlatformAsIOS ();

		void SetPlatformAsDroid ();
	}
}

