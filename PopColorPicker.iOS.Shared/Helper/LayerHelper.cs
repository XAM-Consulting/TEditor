using System;
using System.Collections.Generic;
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
    public static class LayerHelper
    {
        public static void SetupShadow(CALayer layer)
        {
            layer.ShadowColor = UIColor.Black.CGColor;
            layer.ShadowOpacity = 0.8f;
            layer.ShadowOffset = new SizeF(0f, 2f);

            var rect = layer.Frame;
            rect.X = 0f;
            rect.Y = 0f;

            layer.ShadowPath = UIBezierPath.FromRoundedRect(rect, layer.CornerRadius).CGPath;
        }

        public static UIColor InverseColor(UIColor color)
        {
            var componentColor = color.CGColor.Components;
            var newColor = UIColor.FromRGBA(1f - componentColor[0], 1f - componentColor[1], 1f - componentColor[2], componentColor[3]);

            return newColor;
        }
    }
}

