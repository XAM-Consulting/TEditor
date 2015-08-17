using System;
using UIKit;

namespace TEditor.iOS
{
	public class TEditorViewController : UIViewController
	{
		UIButton _boldButton;
		UIButton _ItalicButton;

		TEditorWebView _editor;
		public TEditorViewController ()
		{
			_editor = new TEditorWebView ();
			_boldButton = new UIButton ();
			_ItalicButton = new UIButton ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.White;
			this.EdgesForExtendedLayout = UIRectEdge.None;
			_boldButton.SetTitle ("B", UIControlState.Normal);
			_boldButton.SetTitleColor (UIColor.Black, UIControlState.Normal);
			_ItalicButton.SetTitle ("I", UIControlState.Normal);
			_ItalicButton.SetTitleColor (UIColor.Black, UIControlState.Normal);
			_editor.LoadResource ();

			_boldButton.Frame = new CoreGraphics.CGRect (10, 10, 20, 20);
			_ItalicButton.Frame = new CoreGraphics.CGRect (40, 10, 20, 20);
			_editor.Frame = new CoreGraphics.CGRect (0, 30, View.Frame.Width, View.Frame.Height - 30);
			_editor.Layout ();
			_boldButton.TouchUpInside += (object sender, EventArgs e) => {
				_editor.SetBold();
			};

			_ItalicButton.TouchUpInside += (object sender, EventArgs e) => {
				_editor.SetItalic();
			};

			_boldButton.TranslatesAutoresizingMaskIntoConstraints = false;
			View.AddSubview (_boldButton);
			_ItalicButton.TranslatesAutoresizingMaskIntoConstraints = false;
			View.AddSubview (_ItalicButton);
			_editor.TranslatesAutoresizingMaskIntoConstraints = false;
			View.AddSubview (_editor);

			_editor.SetHTML("<!-- This is an HTML comment --><p>This is a test of the <strong>ZSSRichTextEditor</strong> by <a title=\"Zed Said\" href=\"http://www.zedsaid.com\">Zed Said Studio</a></p>");
		}

		public override void ViewWillTransitionToSize (CoreGraphics.CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
		{
			base.ViewWillTransitionToSize (toSize, coordinator);


			_editor.Frame = new CoreGraphics.CGRect (0, (toSize.Height-100)*0.2+100, toSize.Width, (toSize.Height-100) * 0.8);
		}
	}
}

