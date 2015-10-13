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
    public class ColorPickerHueCircleView : UIView
    {
        private nfloat _hue;
        private UIImageView _crosshairView;

        public ColorPickerHueCircleView(RectangleF frame)
            : base(frame)
        {
            _hue = 0f;

            SetCrosshairView();

            var gestureRecognizer = new UIPanGestureRecognizer(PanOrTapValue);
            this.AddGestureRecognizer(gestureRecognizer);
        }

        private void SetCrosshairView()
        {
            _crosshairView = new UIImageView(new UIImage("color-picker-marker@2x.png"));
            var frame = _crosshairView.Frame;
            frame.X = this.Frame.Width - frame.Width - 1;
            frame.Y = (this.Frame.Height / 2) - (frame.Height / 2);
            _crosshairView.Frame = frame;

            this.AddSubview(_crosshairView);
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

                SetCrosshairPosition();
                OnChanged(EventArgs.Empty);
            }
        }

        #if __UNIFIED__
        public override void Draw(CGRect rect)
        #else
        public override void Draw(RectangleF rect)
        #endif
        {
            DrawHueCircle();
            DrawBlankMiddleCircle();
        }

        private void DrawHueCircle()
        {
            var size = new SizeF(this.Bounds.Size.Width, this.Bounds.Size.Height);

            UIGraphics.BeginImageContextWithOptions(size, true, 0f);
            UIColor.FromRGB(83, 83, 83).SetFill();
            UIGraphics.RectFill(new RectangleF(0f, 0f, size.Width, size.Height));

            var sectors = 180;
            var radius = (nfloat)(Math.Min(size.Width, size.Height) / 2);
            var angle = 2f * (float)Math.PI / sectors;
            UIBezierPath bezierPath;

            for (var i = 0; i < sectors; i++)
            {
                var center = new PointF(size.Width / 2, size.Height / 2);
                bezierPath = UIBezierPath.FromArc(center, radius, i * angle, (i + 1) * angle, true);
                bezierPath.AddLineTo(center);
                bezierPath.ClosePath();

                var color = UIColor.FromHSBA((float)i / sectors, 1f, 1f, 1f);
                color.SetFill();
                color.SetStroke();

                bezierPath.Fill();
                bezierPath.Stroke();
            }

            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            img.Draw(new PointF(0f, 0f));
        }

        private void DrawBlankMiddleCircle()
        {
            var size = new SizeF(this.Bounds.Size.Width - 40f, this.Bounds.Size.Height - 40f);

            UIGraphics.BeginImageContextWithOptions(size, false, 0f);
            UIColor.Clear.SetFill();
            UIGraphics.RectFill(new RectangleF(0f, 0f, size.Width, size.Height));

            var radius = (nfloat)(Math.Min(size.Width, size.Height) / 2);
            var angle = 2f * (float)Math.PI / 1;

            var center = new PointF(size.Width / 2, size.Height / 2);
            var bezierPath = UIBezierPath.FromArc(center, radius, 0, 1 * angle, true);
            bezierPath.AddLineTo(center);
            bezierPath.ClosePath();

            var color = UIColor.FromRGB(83, 83, 83);
            color.SetFill();
            color.SetStroke();

            bezierPath.Fill();
            bezierPath.Stroke();

            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            img.Draw(new PointF(20f, 20f));
        }

        private void SetCrosshairPosition()
        {
            var outerRadius = (this.Frame.Width / 2) - 10;
            var angle = _hue * (2f * (float)Math.PI) - (float)Math.PI;

            var x = -(outerRadius * Math.Cos(angle));
            var y = -(outerRadius * Math.Sin(angle));

            var center = _crosshairView.Center;
            center.X = (float)x + (this.Frame.Width / 2);
            center.Y = (float)y + (this.Frame.Width / 2);

            _crosshairView.Center = center;
        }

        private void PanOrTapValue(UIGestureRecognizer recognizer)
        {
            var center = new PointF(this.Bounds.Width / 2f, this.Bounds.Height / 2f);

            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:
                case UIGestureRecognizerState.Changed:
                case UIGestureRecognizerState.Ended:
                    var point = recognizer.LocationInView(this);
                    var dx = point.X - center.X;
                    var dy = point.Y - center.Y;
                    //var distance = Math.Sqrt(dx * dx + dy * dy);

                    var angle = (float)Math.Atan2(-dy, -dx);
                    var h = (angle + (float)Math.PI) / (2f * (float)Math.PI);

                    this.Hue = h;
                    break;

                case UIGestureRecognizerState.Failed:
                case UIGestureRecognizerState.Cancelled:
                    break;

                default:
                    break;
            }
        }
    }
}

