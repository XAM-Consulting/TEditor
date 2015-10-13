/*
 * Derivative work of ColorPickerDialog.java from Android SDK API samples,
 * ported to Mono for Android and added .NET style event handling.
 * 
 * Copyright (C) 2007 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;

namespace MonoDroid.ColorPickers
{
    public class RoundColorPickerView : View
    {
        public event ColorChangedEventHandler ColorChanged;
        private readonly Paint _paint;
        private readonly Paint _centerPaint;
        private readonly int[] _colors;

        public RoundColorPickerView(Context c, Color color)
            : base(c)
        {
            _colors = new[] {
                new Color(255,0,0,255).ToArgb(), new Color(255,0,255,255).ToArgb(), new Color(0,0,255,255).ToArgb(),
                new Color(0,255,255,255).ToArgb(), new Color(0,255,0,255).ToArgb(), new Color(255,255,0,255).ToArgb(),
                new Color(255,0,0,255).ToArgb()
            };
            using (Shader s = new SweepGradient(0, 0, _colors, null))
            {
                _paint = new Paint(PaintFlags.AntiAlias);
                _paint.SetShader(s);
                _paint.SetStyle(Paint.Style.Stroke);
                _paint.StrokeWidth = 32;
            }

            _centerPaint = new Paint(PaintFlags.AntiAlias)
                            {
                                Color = color,
                                StrokeWidth = 5
                            };
        }

        private bool _trackingCenter;
        private bool _highlightCenter;

        protected override void OnDraw(Canvas canvas)
        {
            var r = CenterX - _paint.StrokeWidth * 0.5f;

            canvas.Translate(CenterX, CenterX);

            canvas.DrawOval(new RectF(-r, -r, r, r), _paint);
            canvas.DrawCircle(0, 0, CenterRadius, _centerPaint);

            if (_trackingCenter)
            {
                var c = _centerPaint.Color;
                _centerPaint.SetStyle(Paint.Style.Stroke);

                _centerPaint.Alpha = _highlightCenter ? 255 : 128;

                canvas.DrawCircle(0, 0,
                                  CenterRadius + _centerPaint.StrokeWidth,
                                  _centerPaint);

                _centerPaint.SetStyle(Paint.Style.Fill);
                _centerPaint.Color = c;
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            SetMeasuredDimension(CenterX * 2, CenterY * 2);
        }

        private const int CenterX = 100;
        private const int CenterY = 100;
        private const int CenterRadius = 32;

        private static int Ave(int s, int d, float p)
        {
            return (int)(s + Math.Round(p * (d - s)));
        }

        private static int InterpColor(IList<int> colors, float unit)
        {
            if (unit <= 0)
            {
                return colors[0];
            }
            if (unit >= 1)
            {
                return colors[colors.Count - 1];
            }

            float p = unit * (colors.Count - 1);
            var i = (int)p;
            p -= i;

            // now p is just the fractional part [0...1) and i is the index
            var c0 = colors[i];
            var c1 = colors[i + 1];
            var a = Ave(Color.GetAlphaComponent(c0), Color.GetAlphaComponent(c1), p);
            var r = Ave(Color.GetRedComponent(c0), Color.GetRedComponent(c1), p);
            var g = Ave(Color.GetGreenComponent(c0), Color.GetGreenComponent(c1), p);
            var b = Ave(Color.GetBlueComponent(c0), Color.GetBlueComponent(c1), p);

            return Color.Argb(a, r, g, b);
        }

        private const float Pi = 3.1415926f;

        public override bool OnTouchEvent(MotionEvent e)
        {
            float x = e.GetX() - CenterX;
            float y = e.GetY() - CenterY;
            bool inCenter = Math.Sqrt(x * x + y * y) <= CenterRadius;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _trackingCenter = inCenter;
                    if (inCenter)
                    {
                        _highlightCenter = true;
                        Invalidate();
                    }
                    break;
                case MotionEventActions.Move:
                    if (_trackingCenter)
                    {
                        if (_highlightCenter != inCenter)
                        {
                            _highlightCenter = inCenter;
                            Invalidate();
                        }
                    }
                    else
                    {
                        var angle = (float)Math.Atan2(y, x);
                        // need to turn angle [-PI ... PI] into unit [0....1]
                        var unit = angle / (2 * Pi);
                        if (unit < 0)
                        {
                            unit += 1;
                        }
                        _centerPaint.Color = new Color(InterpColor(_colors, unit));
                        Invalidate();
                    }
                    break;
                case MotionEventActions.Up:
                    if (_trackingCenter)
                    {
                        if (inCenter)
                        {
                            if (null != ColorChanged)
                                ColorChanged(this, new ColorChangedEventArgs { Color = _centerPaint.Color });
                        }
                        _trackingCenter = false;    // so we draw w/o halo
                        Invalidate();
                        GC.Collect();
                    }
                    break;
            }
            return true;
        }
    }
}