using System;
using System.Collections.Generic;
using System.Linq;

namespace TEditor
{


	public class ToolbarBuilder : List<TEditorToolbarItem>
	{
		public ToolbarBuilder AddBasic ()
		{
			AddBold ();
			AddItalic ();
			AddUnderline ();
			AddRemoveFormat ();
			return this;
		}

		public ToolbarBuilder AddStandard ()
		{
			AddBasic ();
			AddJustifyCenter ();
			AddJustifyFull ();
			AddJustifyLeft ();
			AddJustifyRight ();
			AddH1 ();
			AddH2 ();
			AddH3 ();
			AddH4 ();
			AddH5 ();
			AddH6 ();
			AddTextColor ();
			//BackgroundColor
			AddUnorderedList ();
			AddOrderedList ();
			return this;
		}

		public ToolbarBuilder AddAll ()
		{
			AddStandard ();
			AddSubscript ();
			AddSuperscript ();
			AddStrikeThrough ();
			AddHorizontalRule ();
			AddIndent ();
			AddOutdent ();
			//insertImage
			//insertLink
			//removeLink
			//QuickLink
			AddUndo ();
			AddRedo ();
			AddParagraph ();
			return this;
		}

		#region Add functions

		public ToolbarBuilder AddBold (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSbold.png" : imagePath,
				Label = "bold",
				ClickFunc = (input) => {
					input.SetBold ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddItalic (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSitalic.png" : imagePath,
				Label = "italic",
				ClickFunc = (input) => {
					input.SetItalic ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddSubscript (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSsubscript.png" : imagePath,
				Label = "subscript",
				ClickFunc = (input) => {
					input.SetSubscript ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddSuperscript (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSsuperscript.png" : imagePath,
				Label = "superscript",
				ClickFunc = (input) => {
					input.SetSuperscript ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddStrikeThrough (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSstrikethrough.png" : imagePath,
				Label = "strikethrough",
				ClickFunc = (input) => {
					input.SetStrikeThrough ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddUnderline (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSunderline.png" : imagePath,
				Label = "underline",
				ClickFunc = (input) => {
					input.SetUnderline ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddRemoveFormat (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSclearstyle.png" : imagePath,
				Label = "clearstyle",
				ClickFunc = (input) => {
					input.RemoveFormat ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyLeft (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSleftjustify.png" : imagePath,
				Label = "leftjustify",
				ClickFunc = (input) => {
					input.AlignLeft ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyCenter (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSScenterjustify.png" : imagePath,
				Label = "centerjustify",
				ClickFunc = (input) => {
					input.AlignCenter ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyRight (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSrightjustify.png" : imagePath,
				Label = "rightjustify",
				ClickFunc = (input) => {
					input.AlignRight ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyFull (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSforcejustify.png" : imagePath,
				Label = "forcejustify",
				ClickFunc = (input) => {
					input.AlignFull ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH1 (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSh1.png" : imagePath,
				Label = "h1",
				ClickFunc = (input) => {
					input.Heading1 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH2 (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSh2.png" : imagePath,
				Label = "h2",
				ClickFunc = (input) => {
					input.Heading2 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH3 (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSh3.png" : imagePath,
				Label = "h3",
				ClickFunc = (input) => {
					input.Heading3 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH4 (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSh4.png" : imagePath,
				Label = "h4",
				ClickFunc = (input) => {
					input.Heading4 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH5 (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSh5.png" : imagePath,
				Label = "h5",
				ClickFunc = (input) => {
					input.Heading5 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH6 (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSh6.png" : imagePath,
				Label = "h6",
				ClickFunc = (input) => {
					input.Heading6 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddUnorderedList (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSunorderedlist.png" : imagePath,
				Label = "unorderedlist",
				ClickFunc = (input) => {
					input.SetUnorderedList ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddOrderedList (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSorderedlist.png" : imagePath,
				Label = "orderedlist",
				ClickFunc = (input) => {
					input.SetOrderedList ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddHorizontalRule (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSShorizontalrule.png" : imagePath,
				Label = "horizontalrule",
				ClickFunc = (input) => {
					input.SetHR ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddIndent (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSindent.png" : imagePath,
				Label = "indent",
				ClickFunc = (input) => {
					input.SetIndent ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddOutdent (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSoutdent.png" : imagePath,
				Label = "outdent",
				ClickFunc = (input) => {
					input.SetOutdent ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddQuickLink (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSquicklink.png" : imagePath,
				Label = "quicklink",
				ClickFunc = (input) => {
					input.QuickLink ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddUndo (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSundo.png" : imagePath,
				Label = "undo",
				ClickFunc = (input) => {
					input.Undo ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddRedo (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSredo.png" : imagePath,
				Label = "redo",
				ClickFunc = (input) => {
					input.Redo ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddParagraph (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSSparagraph.png" : imagePath,
				Label = "paragraph",
				ClickFunc = (input) => {
					input.Paragraph ();
					return string.Empty;
				}
			});
			return this;
		}

		public ToolbarBuilder AddTextColor (string imagePath = null)
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = string.IsNullOrEmpty (imagePath) ? "ZSStextcolor.png" : imagePath,
				Label = "textcolor",
				ClickFunc = (input) => {
					if (input.LaunchColorPicker != null) {
						input.PrepareInsert ();
						input.LaunchColorPicker.Invoke ();
					}
					return string.Empty;
				}
			});
			return this;
		}

		#endregion

		void AddOnce (TEditorToolbarItem item)
		{
			if (this.Count == 0) {
				this.Add (item);
				return;
			}
			var iteminlist = this.FirstOrDefault (t => t.Label == item.Label);
			if (iteminlist == null)
				this.Add (item);
		}
	}

	public class TEditorToolbarItem
	{
		public string ImagePath { get; set; }

		public string Label { get; set; }

		internal Func<ITEditorAPI, string> ClickFunc { get; set; }

	}

}

