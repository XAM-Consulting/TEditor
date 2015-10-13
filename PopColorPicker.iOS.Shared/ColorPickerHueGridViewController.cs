using System;
using System.Collections.Generic;
using System.Drawing;

#if __UNIFIED__
using CoreAnimation;
using Foundation;
using UIKit;

using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;
#else
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace PopColorPicker.iOS
{
    public class ColorPickerHueGridViewController: UIViewController
    {
        private List<UIColor> _colors;

        public ColorPickerHueGridViewController()
            : base()
        {
            _colors = new List<UIColor>();

            for (var i = 0; i < 12; i++)
            {
                var hue = (float)(i * 30f / 360f);
                var colorCount = DisplayHelper.Is4InchDisplay() ? 32 : 24;

                for (var j = 0; j < colorCount; j++)
                {
                    var row = j / 4;
                    var column = j % 4;

                    var saturation = (float)column * 0.25f + 0.25f;
                    var luminosity = 1f - (float)row * 0.12f;
                    var color = UIColor.FromHSBA(hue, saturation, luminosity, 1f);
                    _colors.Add(color);
                }
            }
        }

        private UIScrollView _paletteView;
        private UIImageView _colorBarView;
        private CALayer _previousLayer;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.Clear;

            _colorBarView = new UIImageView(new RectangleF(10f, 10f, 300f, 30f));
            _colorBarView.Image = new UIImage("color-bar@2x.png");

            this.View.AddSubview(_colorBarView);

            _paletteView = new UIScrollView(new RectangleF(0, 50f, 320f, this.View.Frame.Height - 120f));
            _paletteView.BackgroundColor = UIColor.Clear;
            _paletteView.Bounces = true;
            _paletteView.AlwaysBounceHorizontal = true;
            _paletteView.AlwaysBounceVertical = false;
            _paletteView.PagingEnabled = true;

            this.View.AddSubview(_paletteView);

            var layer = new CALayer();
            layer.Frame = new RectangleF(130f, 16f, 100f, 40f);
            layer.CornerRadius = 6f;
            layer.ShadowColor = UIColor.Black.CGColor;
            layer.ShadowOffset = new SizeF(0f, 2f);
            layer.ShadowOpacity = 0.8f;

            _paletteView.Layer.AddSublayer(layer);

            var index = 0;
            for (var i = 0; i < 12; i++)
            {
                var colorCount = DisplayHelper.Is4InchDisplay() ? 32 : 24;

                for (var j = 0; j < colorCount && index < _colors.Count; j++)
                {
                    var colorIndex = index++;
                    layer = new CALayer();
                    layer.Name = string.Format("Color_{0}", colorIndex);
                    layer.CornerRadius = 6f;
                    layer.BackgroundColor = _colors[colorIndex].CGColor;

                    var column = j % 4;
                    var row = j / 4;
                    layer.Frame = new RectangleF((float)(i * 320 + 8 + (column * 78)), (float)(8 + row * 48), 70f, 40f);
                    LayerHelper.SetupShadow(layer);
                    _paletteView.Layer.AddSublayer(layer);
                }
            }
                
            _paletteView.ContentSize = new SizeF(3840f, 296f);

            var colorRecognizer = new UITapGestureRecognizer(ColorGridTapped);
            _paletteView.AddGestureRecognizer(colorRecognizer);

            _colorBarView.UserInteractionEnabled = true;
            var barRecognizer = new UITapGestureRecognizer(ColorBarTapped);
            _colorBarView.AddGestureRecognizer(barRecognizer);
        }

        public void ColorBarTapped(UITapGestureRecognizer recognizer)
        {
            var point = recognizer.LocationInView(_colorBarView);
            var page = (int)point.X / 25;

            var frame = new RectangleF(page * 320f, 0f, 320f, _paletteView.Frame.Size.Height);
            _paletteView.ScrollRectToVisible(frame, true);
        }

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
                    var parent = this.ParentViewController as PopColorPickerViewController;
                    parent.SelectedColor = _colors[index];
                }
            }
        }
    }
}

