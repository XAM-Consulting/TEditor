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
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace MonoDroid.ColorPickers
{
    public delegate void PanelClickedEventHandler(object sender, ColorChangedEventArgs e);

    class ColorPickerPanelView: View
    {
        public event PanelClickedEventHandler PanelClicked;

        /**
	     * The width in pixels of the border
	     * surrounding the color panel.
	     */
	    private const float	BorderWidthPx = 1;

	    private float _density = 1f;

	    private Color _borderColor = Color.Argb(255,110,110,110);
	    private Color _color = Color.Argb(255,0,0,0);

	    private Paint _borderPaint;
	    private Paint _colorPaint;

	    private RectF _drawingRect;
	    private RectF _colorRect;

	    private AlphaPatternDrawable _alphaPattern;


        protected ColorPickerPanelView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public ColorPickerPanelView(Context context) : this(context, null)
        {
        }

        public ColorPickerPanelView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public ColorPickerPanelView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init();
        }

        private void Init()
        {
            _borderPaint = new Paint();
            _colorPaint = new Paint();
            _density = Context.Resources.DisplayMetrics.Density;
        }

        public Color Color
        {
            get { return _color; }
            set { 
                _color = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            var rect = _colorRect;

            if (BorderWidthPx > 0)
            {
                _borderPaint.Color = _borderColor;
                canvas.DrawRect(_drawingRect, _borderPaint);
		    }

            if (_alphaPattern != null)
            {
                _alphaPattern.Draw(canvas);
		    }

            _colorPaint.Color = _color;

            canvas.DrawRect(rect, _colorPaint);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            var height = MeasureSpec.GetSize(heightMeasureSpec);

            SetMeasuredDimension(width, height);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            _drawingRect = new RectF
                               {
                                   Left = PaddingLeft,
                                   Right = w - PaddingRight,
                                   Top = PaddingTop,
                                   Bottom = h - PaddingBottom
                               };

            SetUpColorRect();
        }

        private void SetUpColorRect(){
		    var dRect = _drawingRect;

            var left = dRect.Left + BorderWidthPx;
            var top = dRect.Top + BorderWidthPx;
            var bottom = dRect.Bottom - BorderWidthPx;
            var right = dRect.Right - BorderWidthPx;

		    _colorRect = new RectF(left,top, right, bottom);

		    _alphaPattern = new AlphaPatternDrawable((int)(5 * _density));

            _alphaPattern.SetBounds(
			    (int) Math.Round(_colorRect.Left),
                (int) Math.Round(_colorRect.Top),
                (int) Math.Round(_colorRect.Right),
                (int) Math.Round(_colorRect.Bottom)
		    );
	    }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Down)
            {
                if (PanelClicked != null)
                    PanelClicked(this, new ColorChangedEventArgs { Color = _color });
            }

            return base.OnTouchEvent(e);
        }
    }
}