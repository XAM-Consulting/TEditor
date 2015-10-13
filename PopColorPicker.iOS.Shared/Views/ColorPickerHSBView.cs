using System;
using System.Drawing;

#if __UNIFIED__
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;
#else
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace PopColorPicker.iOS
{
    public class ColorPickerHSBView : UIView
    {
        private nfloat _hue;
        private nfloat _saturation;
        private nfloat _brightness;

        private byte _red;
        private byte _green;
        private byte _blue;

        private UIImageView _crosshairView;

        public ColorPickerHSBView(RectangleF frame)
            : base(frame)
        {
            _hue = 0f;
            _saturation = 1f;
            _brightness = 1f;

            SetCrosshairView();
            SetGestureRecognizer();
        }

        private void SetGestureRecognizer()
        {
            var gestureRecognizer = new UIPanGestureRecognizer(PanOrTapValue);
            this.AddGestureRecognizer(gestureRecognizer);
        }

        private void SetCrosshairView()
        {
            _crosshairView = new UIImageView(new UIImage("color-picker-inner-marker@2x.png"));
            var frame = _crosshairView.Frame;
            frame.X = this.Frame.Width - frame.Width / 2f;
            frame.Y = 0 - frame.Height / 2f;
            _crosshairView.Frame = frame;

            this.AddSubview(_crosshairView);
        }

        public byte Red
        {
            get { return _red; }
            set
            {
                _red = value;

                SetCrosshairPosition();
                CalculateHSB();
            }
        }

        public byte Green
        {
            get { return _green; }
            set
            {
                _green = value;

                SetCrosshairPosition();
                CalculateHSB();
            }
        }

        public byte Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;

                SetCrosshairPosition();
                CalculateHSB();
            }
        }

        public delegate void ValueChanged(object sender,EventArgs e);

        public event ValueChanged Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        public nfloat Hue
        {
            get { return _hue; }
            set
            {
                _hue = value;

                CalculateRGB();
                this.SetNeedsDisplay();

                OnChanged(EventArgs.Empty);
            }
        }

        #if __UNIFIED__
        public override void Draw(CGRect rect)
        #else
        public override void Draw(RectangleF rect)
        #endif
        {
            DrawColorSqure();
        }

        private void DrawColorSqure()
        {
            var startX = this.Bounds.GetMinX();
            var startY = this.Bounds.GetMinY();
            var endX = this.Bounds.GetMaxX();
            var endY = this.Bounds.GetMaxY();

            using (var context = UIGraphics.GetCurrentContext())
            {
                var path = new CGPath();
                path.AddLines(new PointF[]
                    {
                        new PointF(startX, startY),
                        new PointF(endX, startY), 
                        new PointF(endX, endY),
                        new PointF(startX, endY)
                    });

                path.CloseSubpath();
                context.AddPath(path);
                context.Clip();

                using (var rgb = CGColorSpace.CreateDeviceRGB())
                {
                    nfloat r, g, b, a;
                    UIColor.FromHSBA(_hue, 1f, 1f, 1f).GetRGBA(out r, out g, out b, out a);

                    var gradient1 = new CGGradient(rgb, new CGColor[]
                        {
                            CGColorFromHex(0xff, 0xff, 0xff, 0xff),
                            CGColorFromHex((byte)(r * 0xff), (byte)(g * 0xff), (byte)(b * 0xff), 0xff)
                        });

                    var gradient2 = new CGGradient(rgb, new CGColor[]
                        {
                            CGColorFromHex(0x00, 0x00, 0x00, 0x0f),
                            CGColorFromHex(0x00, 0x00, 0x00, 0xff)
                        });

                    context.DrawLinearGradient(gradient1, new PointF(startX, startY), new PointF(endX, startX), 0);
                    context.DrawLinearGradient(gradient2, new PointF(startX, startY), new PointF(startY, endY), 0);
                }
            }
        }

        private CGColor CGColorFromHex(byte red, byte green, byte blue, byte alpha)
        {
            return UIColor.FromRGBA(red, green, blue, alpha).CGColor;
        }

        private void PanOrTapValue(UIGestureRecognizer recognizer)
        {
            var startX = this.Bounds.GetMinX();
            var startY = this.Bounds.GetMinY();
            var endX = this.Bounds.GetMaxX();
            var endY = this.Bounds.GetMaxY();

            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:
                case UIGestureRecognizerState.Changed:
                case UIGestureRecognizerState.Ended:
                    var point = recognizer.LocationInView(this);
                    var pX = point.X;
                    var pY = point.Y;

                    if (point.X <= startX)
                        pX = startX;
                    else if (point.X >= endX)
                        pX = endX;

                    if (point.Y <= startY)
                        pY = startY;
                    else if (point.Y >= endY)
                        pY = endY;

                    _saturation = pX / endX;
                    _brightness = (nfloat)(Math.Abs(pY - endY) / endY);

                    SetCrosshairPosition();
                    CalculateRGB();
                    OnChanged(EventArgs.Empty);
                    break;

                case UIGestureRecognizerState.Failed:
                case UIGestureRecognizerState.Cancelled:
                    break;

                default:
                    break;
            }
        }

        private void SetCrosshairPosition()
        {
            var endX = this.Bounds.GetMaxX();
            var endY = this.Bounds.GetMaxY();
            var pX = _saturation * endX;
            var pY = (nfloat)Math.Abs((_brightness * endY) - endY);

            var frame = _crosshairView.Frame;
            frame.X = pX - frame.Width / 2f;
            frame.Y = pY - frame.Width / 2f;
            _crosshairView.Frame = frame;
        }

        private void CalculateRGB()
        {
            nfloat r, g, b, a;
            UIColor.FromHSBA(_hue, _saturation, _brightness, 1f).GetRGBA(out r, out g, out b, out a);

            this.Red = (byte)(r * 0xff);
            this.Green = (byte)(g * 0xff);
            this.Blue = (byte)(b * 0xff);
        }

        private void CalculateHSB()
        {
            nfloat h, s, b, a;
            UIColor.FromRGBA(_red, _green, _blue, (byte)0xff).GetHSBA(out h, out s, out b, out a);

            _hue = h;
            _saturation = s;
            _brightness = b;
        }
    }
}

