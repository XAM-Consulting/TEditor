/*
 * Direct port to Mono for Android of https://github.com/attenzione/android-ColorPickerPreference
 * by Tomasz Cielecki <tomasz@ostebaronen.dk>
 * Whose license is:
 * 
 * Copyright (C) 2010 Daniel Nilsson
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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MonoDroid.ColorPickers
{
    public class AlphaPatternDrawable: Drawable
    {
        private readonly int _rectangleSize = 10;

        private readonly Paint _paint = new Paint();
        private readonly Paint _paintWhite = new Paint();
        private readonly Paint _paintGray = new Paint();

        private int _numRectanglesHorizontal;
        private int _numRectanglesVertical;

        /**
         * Bitmap in which the pattern will be cahched.
         */
        private Bitmap _bitmap;

        public AlphaPatternDrawable(int rectangleSize)
        {
            _rectangleSize = rectangleSize;
            _paintWhite.Color = Color.Argb(255,255,255,255);
            _paintGray.Color = Color.Argb(255,203,203,203);
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(_bitmap, null, Bounds, _paint);
        }

        protected override void OnBoundsChange(Rect bounds)
        {
            base.OnBoundsChange(bounds);

            var height = Bounds.Height();
            var width = Bounds.Width();

            _numRectanglesHorizontal = (int)Math.Ceiling((double) (width / _rectangleSize));
            _numRectanglesVertical = (int)Math.Ceiling((double) (height / _rectangleSize));

            GeneratePatternBitmap();
        }

        /**
	     * This will generate a bitmap with the pattern
	     * as big as the rectangle we were allow to draw on.
	     * We do this to chache the bitmap so we don't need to
	     * recreate it each time draw() is called since it
	     * takes a few milliseconds.
	     */
        private void GeneratePatternBitmap()
        {

            if (Bounds.Width() <= 0 || Bounds.Height() <= 0)
            {
                return;
            }

            _bitmap = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);

            using (var canvas = new Canvas(_bitmap))
            {
                var r = new Rect();
                var verticalStartWhite = true;
                for (var i = 0; i <= _numRectanglesVertical; i++)
                {
                    var isWhite = verticalStartWhite;
                    for (var j = 0; j <= _numRectanglesHorizontal; j++)
                    {

                        r.Top = i * _rectangleSize;
                        r.Left = j * _rectangleSize;
                        r.Bottom = r.Top + _rectangleSize;
                        r.Right = r.Left + _rectangleSize;

                        canvas.DrawRect(r, isWhite ? _paintWhite : _paintGray);

                        isWhite = !isWhite;
                    }

                    verticalStartWhite = !verticalStartWhite;
                }
            }
        }

        public override void SetAlpha(int alpha)
        {
            throw new NotImplementedException();
        }

        public override void SetColorFilter(ColorFilter cf)
        {
            throw new NotImplementedException();
        }

        public override int Opacity
        {
            get { return 0; }
        }
    }
}