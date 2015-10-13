using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

#if __UNIFIED__
using CoreGraphics;
using CoreAnimation;
using Foundation;
using UIKit;
using ObjCRuntime;

using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace PopColorPicker.iOS
{
	public class ColorPickerFavoriteViewController : UIViewController
	{
		private Dictionary<string, UIColor> _colors;

		private UIView _paletteView;
		private UIView _deleteView;

		private UILongPressGestureRecognizer _longPressGestureRecognizer;

		private UIView _currentView;
		private UIView _previousView;
		private PointF _previousPoint;

		private readonly FavoriteColorManager _favoriteColorManager;

		public ColorPickerFavoriteViewController()
			: base()
		{
			_colors = new Dictionary<string, UIColor>();
			_favoriteColorManager = new FavoriteColorManager();
		}

		public override void ViewWillAppear(bool animated)
		{
			SetColorDict();
			ClearColorGrid();
			SetColorGrid();

			base.ViewWillAppear(animated);
		}

		private void SetColorDict()
		{
			_colors = new Dictionary<string, UIColor>();
			var colorList = _favoriteColorManager.List();

			foreach (var color in colorList)
			{
				var temp = color.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
				var r = (byte)(float.Parse(temp[1]) * 0xff);
				var g = (byte)(float.Parse(temp[2]) * 0xff);
				var b = (byte)(float.Parse(temp[3]) * 0xff);

				_colors.Add(color, UIColor.FromRGB(r, g, b));
			}
		}

		private UIImageView _trashImg;
		private UILabel _removeLabel;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_paletteView = new UIView(new RectangleF(0, 10f, 320f, View.Frame.Height - 80f));
			_paletteView.BackgroundColor = UIColor.Clear;

			View.AddSubview(_paletteView);

			var layer = new CALayer();
			layer.Frame = new RectangleF(130f, 16f, 100f, 40f);
			layer.CornerRadius = 6f;
			layer.ShadowColor = UIColor.Black.CGColor;
			layer.ShadowOffset = new SizeF(0f, 2f);
			layer.ShadowOpacity = 0.8f;

			_paletteView.Layer.AddSublayer(layer);

			var colorRecognizer = new UITapGestureRecognizer(ColorGridTapped);
			_paletteView.AddGestureRecognizer(colorRecognizer);

			_longPressGestureRecognizer = new UILongPressGestureRecognizer(HandleLongPressGesture);
			View.AddGestureRecognizer(_longPressGestureRecognizer);

			_deleteView = new UIView();
			_deleteView.BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f);
			_deleteView.Frame = new RectangleF(0f, View.Bounds.Bottom, View.Frame.Width, 190f);
			_paletteView.AddSubview(_deleteView);

			_trashImg = new UIImageView(new UIImage("color-picker-trash@2x.png"));
			var frame = _trashImg.Frame;
			frame.X = _deleteView.Center.X - 45;
			frame.Y = 20f;
			_trashImg.Frame = frame;

			_removeLabel = new UILabel(new RectangleF(_deleteView.Center.X - 15, 20f, 100f, 20f));
			_removeLabel.Text = "Remove";
			_removeLabel.TextColor = UIColor.Gray;
			_removeLabel.AdjustsFontSizeToFitWidth = true;
			_removeLabel.MinimumScaleFactor = 10f;

			_deleteView.AddSubview(_trashImg);
			_deleteView.AddSubview(_removeLabel);

			SetColorGrid();
		}

		private void SetColorGrid()
		{
			var colorCount = _colors.Count;

			for (var i = 0; i < colorCount && i < _colors.Count; i++)
			{
				var color = _colors.ElementAt(i);

				var layer = new CALayer();
				layer.Name = string.Format("Color_{0}", i);
				layer.CornerRadius = 6f;
				layer.BackgroundColor = color.Value.CGColor;
				layer.Frame = new RectangleF(0f, 0f, 70f, 40f);
				LayerHelper.SetupShadow(layer);

				var column = i % 4;
				var row = i / 4;
				var frame = new RectangleF((float)(8 + (column * 78)), (float)(8 + row * 48), 70f, 40f);
				var myView = new UIView(frame);
				myView.Tag = 99;
				myView.Layer.AddSublayer(layer);

				_paletteView.AddSubview(myView);
			}
		}

		private void ClearColorGrid()
		{
			foreach (var view in _paletteView.Subviews)
			{
				if (view.Tag == 99)
				{
					view.RemoveFromSuperview();
				}
			}
		}

		private RectangleF _previousDeleteViewFrame;

		public void HandleLongPressGesture(UILongPressGestureRecognizer recognizer)
		{
			var point = recognizer.LocationInView(_paletteView);

			switch (recognizer.State)
			{
				case UIGestureRecognizerState.Began:
					{
						if (_currentView == null)
						{
							var touchedView = _paletteView.HitTest(point, null);

							if (!touchedView.Equals(_paletteView))
							{
								UIView.Animate(0.5, () =>
									{
										_previousDeleteViewFrame = _deleteView.Frame;
										var frame = _deleteView.Frame;
										frame.Y = frame.Y - 190f;

										_deleteView.Frame = frame;
										_paletteView.BringSubviewToFront(_deleteView);
									});

								_paletteView.BringSubviewToFront(touchedView);
								_currentView = touchedView;
								_previousView = touchedView;
								_previousPoint = touchedView.Center;
							}
						}
					}
					break;
				case UIGestureRecognizerState.Changed:
					{
						if (_currentView != null)
						{
							var center = _currentView.Center;
							center.X += point.X - _previousView.Center.X;
							center.Y += point.Y - _previousView.Center.Y;

							_currentView.Center = center;
						}
					}
					break;
				case UIGestureRecognizerState.Ended:
					{
						if (_currentView != null)
						{
							var deletePoint = recognizer.LocationInView(_deleteView);

							if (_deleteView.Bounds.Contains(deletePoint))
							{
								var temp = _currentView.Layer.Sublayers.First().Name.Split(new char[] { '_' });
								var index = int.Parse(temp[1]);
								var color = _colors.ElementAt(index);
								_colors.Remove(color.Key);

								_favoriteColorManager.Delete(color.Key);

								ClearColorGrid();
								SetColorGrid();
							}

							_currentView.Center = _previousPoint;
							_currentView = null;

							UIView.Animate(0.5, () =>
								{
									_deleteView.Frame = _previousDeleteViewFrame;
								});
						}
					}
					break;
			}
		}

		private CALayer _previousLayer;

		public void ColorGridTapped(UITapGestureRecognizer recognizer)
		{
			var point = recognizer.LocationInView(_paletteView);
			var touchedLayer = _paletteView.Layer.PresentationLayer.HitTest(_paletteView.ConvertPointToView(point, _paletteView.Superview));

			if (touchedLayer != null && !string.IsNullOrWhiteSpace(touchedLayer.Name) && touchedLayer.Name.IndexOf("Color") == 0)
			{
				var actualLayer = touchedLayer.ModelLayer;

				if (_previousLayer == null)
				{
					actualLayer.BorderWidth = 3f;
					actualLayer.BorderColor = UIColor.White.CGColor;

					_previousLayer = actualLayer;
				}
				else
				{
					if (actualLayer.Name.Equals(_previousLayer.Name) == false)
					{
						_previousLayer.BorderWidth = 0f;
						_previousLayer.BorderColor = UIColor.Clear.CGColor;

						actualLayer.BorderWidth = 3f;
						actualLayer.BorderColor = UIColor.White.CGColor;

						_previousLayer = actualLayer;
					}
				}
			}

			if (_previousLayer != null)
			{
				var temp = _previousLayer.Name.Split(new char[] { '_' });
				var index = int.Parse(temp[1]);


				if (index < _colors.Count)
				{
					var color = _colors.ElementAt(index);
					var parent = this.ParentViewController as PopColorPickerViewController;
					parent.SelectedColor = color.Value;
				}
			}
		}
	}
}

