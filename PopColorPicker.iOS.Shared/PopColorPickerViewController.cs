using System;
using System.Drawing;

#if __UNIFIED__
using CoreAnimation;
using Foundation;
using UIKit;
#else
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace PopColorPicker.iOS
{
	public class PopColorPickerViewController : UITabBarController
	{
        public PopColorPickerViewController()
			: base()
		{
            SelectedColor = UIColor.Red;
		}

		public string SelectedColorText
		{ 
			get;
			set; 
		}

		private UIColor _selectedColor;

		public UIColor SelectedColor
		{
			get { return _selectedColor; }
			set
			{
				_selectedColor = value;
				_centerButton.BackgroundColor = _selectedColor;
			}
		}

		private UIBarButtonItem _cancelButton;
		private UIBarButtonItem _doneButton;

		public UIBarButtonItem CancelButton
		{ 
			get { return _cancelButton; }
		}

		public UIBarButtonItem DoneButton
		{
			get { return _doneButton; }
		}

		private UIButton _centerButton;

		private ColorPickerStandardViewController _standardViewController;
		private ColorPickerHueGridViewController _hueGridViewController;
		private ColorPickerCustomViewController _customViewController;
		private ColorPickerFavoriteViewController _favoriteViewController;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.ViewControllerSelected += ColorPickerViewController_ViewControllerSelected;

			this.Title = "Color Picker";
			this.View.BackgroundColor = UIColor.FromRGB(83, 83, 83);
			this.PreferredContentSize = new SizeF(320f, 568f);

			if (UIDevice.CurrentDevice.CheckSystemVersion (7,0))
			{
				this.EdgesForExtendedLayout = UIRectEdge.None;
				this.ExtendedLayoutIncludesOpaqueBars = false;
				this.AutomaticallyAdjustsScrollViewInsets = false;
			}

			SetNavigationItems();
			SetViewControllers();
			SetCenterButton();
		}

		private int _currentIndex = 0;

		private void ColorPickerViewController_ViewControllerSelected(object sender, UITabBarSelectionEventArgs e)
		{
			var currentViewController = this.ViewControllers[_currentIndex];

			if (e.ViewController.Title == "Dummy")
			{
				this.SelectedViewController = currentViewController;
			}
			else
			{
				var index = Array.IndexOf(this.ViewControllers, e.ViewController);
				_currentIndex = index;
			}
		}

		private void SetNavigationItems()
		{
			_cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
			_doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
           
			NavigationItem.SetLeftBarButtonItem(_cancelButton, true);
			NavigationItem.SetRightBarButtonItem(_doneButton, true);
		}

		private void SetViewControllers()
		{
			var favoriteImage = new UIImage("color-picker-favorite@2x.png");
			var gridImage = new UIImage("color-picker-grid@2x.png");
			var customImage = new UIImage("color-picker-custom@2x.png");

			_standardViewController = new ColorPickerStandardViewController();
			_standardViewController.TabBarItem = new UITabBarItem("Standard", gridImage, 0);

			_hueGridViewController = new ColorPickerHueGridViewController();
			_hueGridViewController.TabBarItem = new UITabBarItem("HUE", gridImage, 0);

			_customViewController = new ColorPickerCustomViewController();
			_customViewController.TabBarItem = new UITabBarItem("Custom", customImage, 0);

			_favoriteViewController = new ColorPickerFavoriteViewController();
			_favoriteViewController.TabBarItem = new UITabBarItem("Favorites", favoriteImage, 0);

			var dummyViewController = new UIViewController();
			dummyViewController.Title = "Dummy";

			ViewControllers = new UIViewController[]
			{ 
				_standardViewController, 
				_hueGridViewController,
				dummyViewController,
				_customViewController,
				_favoriteViewController
			};

			SelectedViewController = _standardViewController;
		}

		private void SetCenterButton()
		{
			_centerButton = new UIButton(UIButtonType.Custom);
			_centerButton.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin;
			_centerButton.Frame = new RectangleF(0f, 0f, 70f, 60f);
			_centerButton.Layer.CornerRadius = 5f;
			_centerButton.BackgroundColor = UIColor.Black;

			var heightDifference = _centerButton.Frame.Height - this.TabBar.Frame.Size.Height - 7f;

			if (heightDifference < 0)
				_centerButton.Center = this.TabBar.Center;
			else
			{
				var center = this.TabBar.Center;
				center.Y = center.Y - heightDifference / 2f;
				_centerButton.Center = center;
			}

			this.View.AddSubview(_centerButton);
		}
	}
}

