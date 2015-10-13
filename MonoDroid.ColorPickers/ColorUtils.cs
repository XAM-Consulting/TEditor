using System;
using Android.Graphics;

namespace MonoDroid.ColorPickers
{
    public class ColorUtils
    {
        public struct HSV
        {
            public float H;
            public float S;
            public float V;

            public HSV(float h, float s, float v)
            {
                H = h;
                S = s;
                V = v;
            }
        }

        public static HSV ColorToHSV(Color color)
        {
            return ColorToHSV(color.R, color.G, color.B, color.GetHue());
        }

        public static HSV ColorToHSV(int r, int g, int b, float h)
        {
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));

            return new HSV(h, (max == 0) ? 0 : 1f - (1f * min / max), max / 255f);
        }

        public static Color ColorFromHSV(HSV hsv, int alpha = 255)
        {
            return ColorFromHSV(hsv.H, hsv.S, hsv.V, alpha);
        }

        /// <summary>
        /// From http://stackoverflow.com/questions/10289279/converting-hsv-circle-code-from-delphi-to-c-sharp
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="value"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color ColorFromHSV(float hue, float saturation, float value, int alpha = 255)
        {
            if (hue < 0.0f || hue > 1.0f)
                throw new ArgumentOutOfRangeException("hue");
            if (saturation < 0.0f || saturation > 1.0f)
                throw new ArgumentOutOfRangeException("saturation");
            if (value < 0.0f || value > 1.0f)
                throw new ArgumentOutOfRangeException("value");

            if (saturation == 0f)
            {
                var b1 = (int)(value * 255);
                return Color.Argb(alpha, b1, b1, b1);
            }

            float r;
            float g;
            float b;

            var h = hue * 6.0f;
            if (h == 6.0f)
            {
                h = 0.0f;
            }

            var i = (int)Math.Floor(h);

            var v1 = value * (1.0f - saturation);
            var v2 = value * (1.0f - saturation * (h - i));
            var v3 = value * (1.0f - saturation * (1.0f - (h - i)));

            switch (i)
            {
                case 0:
                    r = value;
                    g = v3;
                    b = v1;
                    break;
                case 1:
                    r = v2;
                    g = value;
                    b = v1;
                    break;
                case 2:
                    r = v1;
                    g = value;
                    b = v3;
                    break;
                case 3:
                    r = v1;
                    g = v2;
                    b = value;
                    break;
                case 4:
                    r = v3;
                    g = v1;
                    b = value;
                    break;
                default:
                    r = value;
                    g = v1;
                    b = v2;
                    break;
            }

            r = r * 255.0f;
            if (r > 255.0f)
            {
                r = 255.0f;
            }
            g = g * 255.0f;
            if (g > 255.0f)
            {
                g = 255.0f;
            }
            b = b * 255.0f;
            if (b > 255.0f)
            {
                b = 255.0f;
            }

            return Color.Argb(alpha, (int)r, (int)g, (int)b);
        }
    }
}