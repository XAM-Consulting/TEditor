using System;
using Android.App;
using Android.Webkit;
using Android.Views;
using Android.Content;
using Android.Util;
using MonoDroid.ColorPickers;
using Android.Graphics;
using System.Threading.Tasks;
using Java.IO;

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
		TaskCompletionSource<string> _taskResult = new TaskCompletionSource<string> ();

		public void OnReceiveValue (Java.Lang.Object result)
		{			
			try 
			{				
				JsonReader reader = new JsonReader(new StringReader(result.ToString()));
				reader.Lenient = true;

				if(reader.Peek() != JsonToken.Null) {
					if(reader.Peek() == JsonToken.String) {
						String msg = reader.NextString();
						_taskResult.SetResult (msg.ToString());
					}
				}
			} 
			catch(Exception ex) 
			{
				_taskResult.SetException (ex);
			}
		}

		public Task<string> GetResultAsync ()
		{	
			return _taskResult.Task;
		}
	}

	public class TEditorWebView : WebView
	{
		TEditor _richTextEditor;
		ColorPickerDialog _colorPickerDialog;

		public TEditor RichTextEditor { get { return _richTextEditor; } }
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
				this.EvaluateJavascript (input, null);
			});
			_richTextEditor.SetJavaScriptEvaluatingWithResultFunction ((input) => {
				var activity = context as Activity;
				JavaScriptResult result = new JavaScriptResult ();
				this.EvaluateJavascript (input, result);
				if(activity!=null)
				{
					activity.RunOnUiThread(() => {
					});
				}
				return result.GetResultAsync();
			});
			_colorPickerDialog = new ColorPickerDialog (context, Color.Red);
			_colorPickerDialog.ColorChanged += (o, args) => {
				_richTextEditor.SetTextColor ((int)args.Color.R, (int)args.Color.G, (int)args.Color.B);
			};

			_richTextEditor.LaunchColorPicker = () => { 
				_colorPickerDialog.Show ();
			};
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

		public async Task<string> GetHTML ()
		{
			return await _richTextEditor.GetHTML ();
		}

	}
}

