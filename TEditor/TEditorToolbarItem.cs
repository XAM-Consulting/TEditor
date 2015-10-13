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

		public ToolbarBuilder AddBold ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSbold.png",
				Label = "bold",
				ClickFunc = (input) => {
					input.SetBold ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddItalic ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSitalic.png",
				Label = "italic",
				ClickFunc = (input) => {
					input.SetItalic ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddSubscript ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSsubscript.png",
				Label = "subscript",
				ClickFunc = (input) => {
					input.SetSubscript ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddSuperscript ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSsuperscript.png",
				Label = "superscript",
				ClickFunc = (input) => {
					input.SetSuperscript ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddStrikeThrough ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSstrikethrough.png",
				Label = "strikethrough",
				ClickFunc = (input) => {
					input.SetStrikeThrough ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddUnderline ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSunderline.png",
				Label = "underline",
				ClickFunc = (input) => {
					input.SetUnderline ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddRemoveFormat ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSclearstyle.png",
				Label = "clearstyle",
				ClickFunc = (input) => {
					input.RemoveFormat ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyLeft ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSleftjustify.png",
				Label = "leftjustify",
				ClickFunc = (input) => {
					input.AlignLeft ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyCenter ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSScenterjustify.png",
				Label = "centerjustify",
				ClickFunc = (input) => {
					input.AlignCenter ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyRight ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSrightjustify.png",
				Label = "rightjustify",
				ClickFunc = (input) => {
					input.AlignRight ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddJustifyFull ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSforcejustify.png",
				Label = "forcejustify",
				ClickFunc = (input) => {
					input.AlignFull ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH1 ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSh1.png",
				Label = "h1",
				ClickFunc = (input) => {
					input.Heading1 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH2 ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSh2.png",
				Label = "h2",
				ClickFunc = (input) => {
					input.Heading2 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH3 ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSh3.png",
				Label = "h3",
				ClickFunc = (input) => {
					input.Heading3 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH4 ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSh4.png",
				Label = "h4",
				ClickFunc = (input) => {
					input.Heading4 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH5 ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSh5.png",
				Label = "h5",
				ClickFunc = (input) => {
					input.Heading5 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddH6 ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSh6.png",
				Label = "h6",
				ClickFunc = (input) => {
					input.Heading6 ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddUnorderedList ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSunorderedlist.png",
				Label = "unorderedlist",
				ClickFunc = (input) => {
					input.SetUnorderedList ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddOrderedList ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSorderedlist.png",
				Label = "orderedlist",
				ClickFunc = (input) => {
					input.SetOrderedList ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddHorizontalRule ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSShorizontalrule.png",
				Label = "horizontalrule",
				ClickFunc = (input) => {
					input.SetHR ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddIndent ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSindent.png",
				Label = "indent",
				ClickFunc = (input) => {
					input.SetIndent ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddOutdent ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSoutdent.png",
				Label = "outdent",
				ClickFunc = (input) => {
					input.SetOutdent ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddQuickLink ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSquicklink.png",
				Label = "quicklink",
				ClickFunc = (input) => {
					input.QuickLink ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddUndo ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSundo.png",
				Label = "undo",
				ClickFunc = (input) => {
					input.Undo ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddRedo ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSredo.png",
				Label = "redo",
				ClickFunc = (input) => {
					input.Redo ();
					return string.Empty;
				}
			});
			return this;
		}


		public ToolbarBuilder AddParagraph ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSSparagraph.png",
				Label = "paragraph",
				ClickFunc = (input) => {
					input.Paragraph ();
					return string.Empty;
				}
			});
			return this;
		}

		public ToolbarBuilder AddTextColor ()
		{
			AddOnce (new TEditorToolbarItem {
				ImagePath = "ZSStextcolor.png",
				Label = "textcolor",
				ClickFunc = (input) => {
					if (input.LaunchColorPicker != null)
					{
						input.PrepareInsert();
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

