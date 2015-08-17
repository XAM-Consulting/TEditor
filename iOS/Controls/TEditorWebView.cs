using System;
using UIKit;
using CoreGraphics;

namespace TEditor.iOS
{
	public class TEditorViewDelegate : UIWebViewDelegate
	{
		TEditor _richTextEditor;

		public TEditorViewDelegate (TEditor richTextEditor)
		{
			_richTextEditor = richTextEditor;
		}

		public override bool ShouldStartLoad (UIWebView webView, Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			string urlString = request.Url.AbsoluteString;
			if (navigationType == UIWebViewNavigationType.LinkClicked)
				return false;
			//			else if (urlString.Contains ("scroll://")) {
			//				int position; 
			//				if(int.TryParse(urlString.Replace ("scroll://", ""),out position);
			//			}
			return true;
		}

		public override void LoadingFinished (UIWebView webView)
		{
			_richTextEditor.EditorLoaded = true;
			_richTextEditor.SetPlatformAsIOS ();
			if (string.IsNullOrEmpty (_richTextEditor.InternalHTML))
				_richTextEditor.InternalHTML = "";
			_richTextEditor.UpdateHTML ();
		}
	}

	[Foundation.Register ("TEditorWebView")]
	public sealed class TEditorWebView : UIView, ITEditorAPI
	{
		TEditor _richTextEditor;
		UIWebView _webView;

		public TEditorWebView () : base ()
		{
			_richTextEditor = new TEditor ();
			_webView = new UIWebView () {
				Frame = new CGRect (0, 0, this.Frame.Width, this.Frame.Height)
			};
			_webView.Delegate = new TEditorViewDelegate (_richTextEditor);
			_webView.ScrollView.Bounces = false;
			Style ();
			_richTextEditor.SetJavaScriptEvaluatingFunction ((input) => {
				return _webView.EvaluateJavascript (input);
			});
			this.Add (_webView);
			BackgroundColor = UIColor.Brown;
			Layer.BorderWidth = 2;
			Layer.BorderColor = UIColor.Gray.CGColor;
		}

		void Style ()
		{
			_webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth
			| UIViewAutoresizing.FlexibleHeight
			| UIViewAutoresizing.FlexibleTopMargin
			| UIViewAutoresizing.FlexibleBottomMargin;
			_webView.ScalesPageToFit = true;
			_webView.BackgroundColor = UIColor.Black;
			
		}


		public void LoadResource ()
		{
			string htmlResource = _richTextEditor.LoadResources ();
			_webView.LoadHtmlString (htmlResource, new Foundation.NSUrl ("www.xam-consulting.com"));
		}

		public void Layout ()
		{
			_webView.Frame = new CGRect (0, 0, this.Frame.Width, this.Frame.Height);
		}

		public void SetHTML (string html)
		{
			_richTextEditor.InternalHTML = html;
			_richTextEditor.UpdateHTML ();
		}

	
		#region ITEditorAPI implementation

		public void AlignFull ()
		{
			_richTextEditor.AlignFull ();
		}

		public void AlignLeft ()
		{
			_richTextEditor.AlignLeft ();
		}

		public void AlignRight ()
		{
			_richTextEditor.AlignRight ();
		}

		public string GetHTML ()
		{
			return _richTextEditor.GetHTML ();
		}

		public void Heading1 ()
		{
			_richTextEditor.Heading1 ();
		}

		public void Heading2 ()
		{
			_richTextEditor.Heading2 ();
		}

		public void Heading3 ()
		{
			_richTextEditor.Heading3 ();
		}

		public void Heading4 ()
		{
			_richTextEditor.Heading4 ();
		}

		public void Heading5 ()
		{
			_richTextEditor.Heading5 ();
		}

		public void Heading6 ()
		{
			_richTextEditor.Heading6 ();
		}

		public void InsertHTML (string html)
		{
			_richTextEditor.InsertHTML (html);
		}

		public void Paragraph ()
		{
			_richTextEditor.Paragraph ();
		}

		public void RemoveFormat ()
		{
			_richTextEditor.RemoveFormat ();
		}

		public void SetBold ()
		{
			_richTextEditor.SetBold ();
		}

		public void SetHR ()
		{
			_richTextEditor.SetHR ();
		}

		public void SetIndent ()
		{
			_richTextEditor.SetIndent ();
		}

		public void SetItalic ()
		{
			_richTextEditor.SetItalic ();
		}

		public void SetOrderedList ()
		{
			_richTextEditor.SetOrderedList ();
		}

		public void SetOutdent ()
		{
			_richTextEditor.SetOutdent ();
		}

		public void SetStrikethrough ()
		{
			_richTextEditor.SetStrikethrough ();
		}

		public void SetSubscript ()
		{
			_richTextEditor.SetSubscript ();
		}

		public void SetSuperscript ()
		{
			_richTextEditor.SetSuperscript ();
		}

		public void SetUnderline ()
		{
			_richTextEditor.SetUnderline ();
		}

		public void SetUnorderedList ()
		{
			_richTextEditor.SetUnorderedList ();
		}

		public void UpdateHTML ()
		{
			_richTextEditor.UpdateHTML ();
		}

		public void SetPlatformAsIOS ()
		{
			_richTextEditor.SetPlatformAsIOS ();
		}

		public void SetPlatformAsDroid ()
		{
			_richTextEditor.SetPlatformAsDroid ();
		}

		#endregion
	}
}

