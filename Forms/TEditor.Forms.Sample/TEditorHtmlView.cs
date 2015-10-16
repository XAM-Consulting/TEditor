using System;
using Xamarin.Forms;

namespace TEditor.Forms.Sample
{
	public class TEditorHtmlView : StackLayout
	{
		//create bindable property, html
		public string Html { get; set; }
		WebView _displayWebView;
		public TEditorHtmlView ()
		{
			this.Children.Add(new Button { Text = "HTML Editor", Command = new Command((obj)=>{
					ShowTEditor();
			})});
			_displayWebView = new WebView ();
			this.Children.Add(_displayWebView);
		}

		async void ShowTEditor()
		{
			var iEditor = DependencyService.Get<ITEditorService> ();
			string html = await iEditor.ShowTEditor("<!-- This is an HTML comment --><p>This is a test of the <strong>TEditor</strong> by <a title=\"XAM consulting\" href=\"http://www.xam-consulting.com\">XAM consulting</a></p>");
			_displayWebView.Source = new HtmlWebViewSource (){ Html = html };
		}
	}
}

