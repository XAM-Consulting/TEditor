using System;
using Android.App;
using Android.Webkit;
using Android.Views;
using Android.Content;
using Android.Util;

namespace TEditor.Droid
{
	public class TEditorWebViewClient :WebViewClient
	{
		TEditor _richTextEditor;

		public TEditorWebViewClient (TEditor richTextEditor)
		{
			_richTextEditor = richTextEditor;
		}

		public override WebResourceResponse ShouldInterceptRequest (WebView view, IWebResourceRequest request)
		{
			if (request.HasGesture)
				return base.ShouldInterceptRequest (view, request);
			return null;
		}

		public override bool ShouldOverrideUrlLoading (WebView view, string url)
		{
			if (url.Contains ("scroll://"))
				return false;
			else if (url.Contains ("callback://"))
				return true;
			else
				view.LoadUrl (url);
			return true;
		}

		public override void OnReceivedError (WebView view, ClientError errorCode, string description, string failingUrl)
		{
			base.OnReceivedError (view, errorCode, description, failingUrl);
		}

		public override void OnPageFinished (WebView view, string url)
		{
			_richTextEditor.EditorLoaded = true;
			_richTextEditor.SetPlatformAsDroid ();
			if (string.IsNullOrEmpty (_richTextEditor.InternalHTML))
				_richTextEditor.InternalHTML = "";
			_richTextEditor.UpdateHTML ();

			base.OnPageFinished (view, url);
		}
	}

	public class TEditorChromeWebClient :WebChromeClient
	{
		public override bool OnConsoleMessage (ConsoleMessage consoleMessage)
		{
			Log.Info ("WebView", consoleMessage.Message ());
			return base.OnConsoleMessage (consoleMessage);
		}
			
			
	}

	public class JavaScriptResult : Java.Lang.Object, IValueCallback
	{
		public string Result { get; set; }

		public void OnReceiveValue (Java.Lang.Object result)
		{
			Java.Lang.String json = (Java.Lang.String)result;
			// |json| is a string of JSON containing the result of your evaluation
			Result = json.ToString ();
		}
	}

	public class TEditorWebView :WebView , ITEditorAPI
	{
		TEditor _richTextEditor;
		//WebView _webView;

		public TEditorWebView (Context context) : base (context)
		{
			Init (context);
		}

		void Init (Context context)
		{
			_richTextEditor = new TEditor ();

			this.SetWebViewClient (new TEditorWebViewClient (_richTextEditor));
			this.SetWebChromeClient (new TEditorChromeWebClient ());
			_richTextEditor.SetJavaScriptEvaluatingFunction ((input) => {
				JavaScriptResult result = new JavaScriptResult ();
				this.EvaluateJavascript (input, result);
				return result.Result;
			});
			this.LoadResource ();
			
		}

		public TEditorWebView (Context context, IAttributeSet attrs) : base (context, attrs)
		{  
			Init (context);
		}

		public TEditorWebView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			Init (context);
		}

		public void LoadResource ()
		{
			this.Settings.JavaScriptEnabled = true;
			this.Settings.AllowUniversalAccessFromFileURLs = true;
			this.Settings.AllowFileAccessFromFileURLs = true;
			this.Settings.AllowFileAccess = true;
			this.Settings.DomStorageEnabled = true;

			string htmlResource = _richTextEditor.LoadResources ();
			this.LoadDataWithBaseURL ("http://www.xam-consulting.com", htmlResource, "text/html", "UTF-8", "");
		}

		public void SetHTML (string html)
		{
			_richTextEditor.InternalHTML = html;
			_richTextEditor.UpdateHTML ();
		}

		#region IZSSRichTextEditorAPI implementation

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

