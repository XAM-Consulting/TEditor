using System;

namespace TEditor
{
	partial class TEditor : ITEditorAPI
	{
		Func<string, string> _javaScriptEvaluatFunc;

		public void SetJavaScriptEvaluatingFunction (Func<string,string> function)
		{
			if (function == null)
				throw new ArgumentNullException ("Function cannot be null");
			_javaScriptEvaluatFunc = function;
		}

		public void UpdateHTML ()
		{
			string html = this.InternalHTML;
			string cleanedHTML = RemoveQuotesFromHTML (html);
			string trigger = string.Format ("zss_editor.setHTML(\"{0}\");", cleanedHTML);
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public string GetHTML ()
		{
			
			string html = _javaScriptEvaluatFunc.Invoke ("zss_editor.getHTML();");
			html = RemoveQuotesFromHTML (html);
			html = TidyHTML (html);
			return html;
		}

		string RemoveQuotesFromHTML (string html)
		{
			html = html.Replace ("\"", "\\\"");
			html = html.Replace ("“", "&quot;");
			html = html.Replace ("”", "&quot;");
			html = html.Replace ("\r", "\\r");
			html = html.Replace ("\n", "\\n");
			return html;
		}

		string TidyHTML (string html)
		{
			html = html.Replace ("<br>", "<br />");
			html = html.Replace ("<hr>", "<hr />");
			if (this.FormatHTML)
				html = _javaScriptEvaluatFunc.Invoke (string.Format ("style_html(\"{0}\");", html));
			return html;
		}

		public void InsertHTML (string html)
		{
			string cleanedHTML = RemoveQuotesFromHTML (html);
			string trigger =string.Format ("zss_editor.insertHTML(\"{0}\");", cleanedHTML);
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void RemoveFormat ()
		{
			string trigger = @"zss_editor.removeFormating();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void AlignLeft ()
		{
			string trigger = @"zss_editor.setJustifyLeft();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void AlignCenter ()
		{
			string trigger = @"zss_editor.setJustifyCenter();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void AlignRight ()
		{
			string trigger = @"zss_editor.setJustifyRight();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void AlignFull ()
		{
			string trigger = @"zss_editor.setJustifyFull();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetBold ()
		{
			string trigger = @"zss_editor.setBold();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetItalic ()
		{
			string trigger = @"zss_editor.setItalic();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetSubscript ()
		{
			string trigger = @"zss_editor.setSubscript();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetUnderline ()
		{
			string trigger = @"zss_editor.setUnderline();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetSuperscript ()
		{
			string trigger = @"zss_editor.setSuperscript();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetStrikethrough ()
		{
			string trigger = @"zss_editor.setStrikeThrough();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetUnorderedList ()
		{
			string trigger = @"zss_editor.setUnorderedList();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetOrderedList ()
		{
			string trigger = @"zss_editor.setOrderedList();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetHR ()
		{
			string trigger = @"zss_editor.setHorizontalRule();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetIndent ()
		{
			string trigger = @"zss_editor.setIndent();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetOutdent ()
		{
			string trigger = @"zss_editor.setOutdent();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void Heading1 ()
		{
			string trigger = @"zss_editor.setHeading('h1');";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void Heading2 ()
		{
			string trigger = @"zss_editor.setHeading('h2');";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void Heading3 ()
		{
			string trigger = @"zss_editor.setHeading('h3');";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void Heading4 ()
		{
			string trigger = @"zss_editor.setHeading('h4');";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void Heading5 ()
		{
			string trigger = @"zss_editor.setHeading('h5');";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void Heading6 ()
		{
			string trigger = @"zss_editor.setHeading('h6');";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void Paragraph ()
		{
			string trigger = @"zss_editor.setParagraph();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetPlatformAsIOS ()
		{
			string trigger = @"zss_editor.setPlatformAsIOS();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}

		public void SetPlatformAsDroid ()
		{
			string trigger = @"zss_editor.setPlatformAsDroid();";
			_javaScriptEvaluatFunc.Invoke (trigger);
		}
	}
}

