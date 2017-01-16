using System;
using System.Threading.Tasks;

namespace TEditor.Abstractions
{
    internal interface ITEditorAPI
    {
        void AlignCenter();

        void AlignFull();

        void AlignLeft();

        void AlignRight();

        Task<string> GetHTML();

        void Heading1();

        void Heading2();

        void Heading3();

        void Heading4();

        void Heading5();

        void Heading6();

        void InsertHTML(string html);

        void Paragraph();

        void QuickLink();

        void RemoveFormat();

        void Redo();

        void SetBold();

        void SetHR();

        void SetIndent();

        void SetItalic();

        void SetOrderedList();

        void SetOutdent();

        void SetStrikeThrough();

        void SetSubscript();

        void SetSuperscript();

        void SetUnderline();

        void SetUnorderedList();

        void SetPlatformAsIOS();

        void SetPlatformAsDroid();

        void UpdateHTML();

        void Undo();

        void SetFooterHeight(double height);

        void SetContentHeight(double height);

        Action LaunchColorPicker { get; set; }

        void PrepareInsert();

        void SetTextColor(int R, int G, int B);
    }
}

