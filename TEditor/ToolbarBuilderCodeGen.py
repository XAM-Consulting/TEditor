from __future__ import print_function

f = open("tempcode.txt",'w')

funcnameArr =[["Bold","bold","SetBold"],
["Italic","italic","SetItalic"],
["Subscript","subscript","SetSubscript"],
["Superscript","superscript","SetSuperscript"],
["StrikeThrough","strikethrough","SetStrikeThrough"],
["Underline","underline","SetUnderline"],
["RemoveFormat","clearstyle","RemoveFormat"],
["JustifyLeft","leftjustify","AlignLeft"],
["JustifyCenter","centerjustify","AlignCenter"],
["JustifyRight","rightjustify","AlignRight"],
["JustifyFull","forcejustify","AlignFull"],
["H1","h1","Heading1"],
["H2","h2","Heading2"],
["H3","h3","Heading3"],
["H4","h4","Heading4"],
["H5","h5","Heading5"],
["H6","h6","Heading6"],
["UnorderedList","unorderedlist","SetUnorderedList"],
["OrderedList","orderedlist","SetOrderedList"],
["HorizontalRule","horizontalrule","SetHR"],
["Indent","indent","SetIndent"],
["Outdent","outdent","SetOutdent"],
["QuickLink","quicklink","QuickLink"],
["Undo","undo", "Undo"],
["Redo","redo", "Redo"],
["Paragraph","paragraph","Paragraph"]]

codetempleStr = """

		public ToolbarBuilder Add{0}(string imagePath = null)
		{{
			AddOnce (new TEditorToolbarItem {{
				ImagePath = string.IsNullOrEmpty(imagePath)? \"ZSS{1}.png\" : imagePath,
				Label = \"{1}\",
				ClickFunc = (input) => {{
					input.{2} ();
					return string.Empty;
				}}
			}});
			return this;
		}}"""

for funcname in funcnameArr:
	print (codetempleStr.format(*funcname),file=f)


