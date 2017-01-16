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
    public class ColorPickerView : View
    {
        #region Fields
        public event ColorChangedEventHandler ColorChanged;

        private const int PanelSatVal = 0;
        private const int PanelHue = 1;
        private const int PanelAlpha = 2;

        /**
         * The width in pixels of the border
         * surrounding all color panels.
         */
        private const float BorderWidthPx = 1;

        /**
         * The width in dp of the hue panel.
         */
        private float _huePanelWidth = 30f;
        /**
         * The height in dp of the alpha panel
         */
        private float _alphaPanelHeight = 20f;
        /**
         * The distance in dp between the different
         * color panels.
         */
        private float _panelSpacing = 10f;
        /**
         * The radius in dp of the color palette tracker circle.
         */
        private float _paletteCircleTrackerRadius = 5f;
        /**
         * The dp which the tracker of the hue or alpha panel
         * will extend outside of its bounds.
         */
        private float _rectangleTrackerOffset = 2f;


        private float _density = 1f;

        private Paint _satValPaint;
        private Paint _satValTrackerPaint;

        private Paint _huePaint;
        private Paint _hueTrackerPaint;

        private Paint _alphaPaint;
        private Paint _alphaTextPaint;

        private Paint _borderPaint;

        private Shader _valShader;
        private Shader _satShader;
        private Shader _hueShader;
        private Shader _alphaShader;

        private int _alpha = 0xff;
        private float _hue = 360f;
        private float _sat;
        private float _val;

        private String _alphaSliderText = "";
        private Color _sliderTrackerColor = Color.Argb(255, 28, 28, 28);
        private Color _borderColor = Color.Argb(255, 110, 110, 100);
        private bool _showAlphaPanel;

        /*
         * To remember which panel that has the "focus" when
         * processing hardware button data.
         */
        private int _lastTouchedPanel = PanelSatVal;

        /**
         * Offset from the edge we must have or else
         * the finger tracker will get clipped when
         * it is drawn outside of the view.
         */


        /*
         * Distance form the edges of the view
         * of where we are allowed to draw.
         */
        private RectF _drawingRect;

        private RectF _satValRect;
        private RectF _hueRect;
        private RectF _alphaRect;

        private AlphaPatternDrawable _alphaPattern;

        private Point _startTouchPoint;
        #endregion

        #region Props

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        public Color Color
        {
            get { return ColorUtils.ColorFromHSV(_hue/360f, _sat, _val, _alpha); }
            set
            {
                var color = value;
                var alpha = Color.GetAlphaComponent(color);
                var red = Color.GetRedComponent(color);
                var blue = Color.GetBlueComponent(color);
                var green = Color.GetGreenComponent(color);

                var hsv = ColorUtils.ColorToHSV(red, green, blue, color.GetHue());

                _alpha = alpha;
                _hue = hsv.H;
                _sat = hsv.S;
                _val = hsv.V;

                if (ColorChanged != null)
                    ColorChanged(this, new ColorChangedEventArgs { Color = ColorUtils.ColorFromHSV(_hue / 360, _sat, _val, _alpha) });

                Invalidate();
            }
        }

        public string AlphaSliderText
        {
            get { return _alphaSliderText; } 
            set { _alphaSliderText = value; }
        }

        public Color SliderTrackerColor
        {
            get { return _sliderTrackerColor;  }
            set
            {
                _sliderTrackerColor = value;

                _hueTrackerPaint.Color = _sliderTrackerColor;

                Invalidate();
            }
        }

        public bool AlphaSliderVisible
        {
            get { return _showAlphaPanel; }
            set
            {
                if (_showAlphaPanel != value)
                {
                    _showAlphaPanel = value;

                    /*
                     * Reset all shader to force a recreation.
                     * Otherwise they will not look right after
                     * the size of the view has changed.
                     */
                    _valShader = null;
                    _satShader = null;
                    _hueShader = null;
                    _alphaShader = null;

                    RequestLayout();
                }
            }
        }

        public float DrawingOffset { get; private set; }

        #endregion


        protected ColorPickerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public ColorPickerView(Context context) : this(context, null)
        {
        }

        public ColorPickerView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public ColorPickerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init();
        }

        private void Init()
        {
            _density = Context.Resources.DisplayMetrics.Density;
            _paletteCircleTrackerRadius *= _density;
            _rectangleTrackerOffset *= _density;
            _huePanelWidth *= _density;
            _alphaPanelHeight *= _density;
            _panelSpacing = _panelSpacing * _density;

            DrawingOffset = CalculateRequiredOffset();


            InitPaintTools();

            //Needed for receiving trackball motion events.
            Focusable = true;
            FocusableInTouchMode = true;
        }

        private void InitPaintTools()
        {

            _satValPaint = new Paint();
            _satValTrackerPaint = new Paint();
            _huePaint = new Paint();
            _hueTrackerPaint = new Paint();
            _alphaPaint = new Paint();
            _alphaTextPaint = new Paint();
            _borderPaint = new Paint();


            _satValTrackerPaint.SetStyle(Paint.Style.Stroke);
            _satValTrackerPaint.StrokeWidth = 2f * _density;
            _satValTrackerPaint.AntiAlias = true;

            _hueTrackerPaint.Color = _sliderTrackerColor;
            _hueTrackerPaint.SetStyle(Paint.Style.Stroke);
            _hueTrackerPaint.StrokeWidth = 2f * _density;
            _hueTrackerPaint.AntiAlias = true;

            _alphaTextPaint.Color = Color.Argb(255,28,28,28);
            _alphaTextPaint.TextSize = 14f * _density;
            _alphaTextPaint.AntiAlias = true;
            _alphaTextPaint.TextAlign = Paint.Align.Center;
            _alphaTextPaint.FakeBoldText = true;
        }

        private float CalculateRequiredOffset()
        {
            var offset = Math.Max(_paletteCircleTrackerRadius, _rectangleTrackerOffset);
            offset = Math.Max(offset, BorderWidthPx * _density);

            return offset * 1.5f;
        }

        private static int[] BuildHueColorArray()
        {

            var hue = new int[361];

            var count = 0;
            for (var i = hue.Length - 1; i >= 0; i--, count++)
            {
                hue[count] = ColorUtils.ColorFromHSV(i/360f, 1f, 1f);
            }

            return hue;
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (_drawingRect.Width() <= 0 || _drawingRect.Height() <= 0) return;

            DrawSatValPanel(canvas);
            DrawHuePanel(canvas);
            DrawAlphaPanel(canvas);
        }

        private void DrawSatValPanel(Canvas canvas)
        {
            #if __ANDROID_11__
            if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.Honeycomb)
            {
                RootView.SetLayerType(LayerType.Software, null);
            }
            #endif
            
            var rect = _satValRect;

		    if(BorderWidthPx > 0){
			    _borderPaint.Color = _borderColor;
                canvas.DrawRect(_drawingRect.Left, _drawingRect.Top, rect.Right + BorderWidthPx, rect.Bottom + BorderWidthPx, _borderPaint);
		    }

		    if (_valShader == null) {
                _valShader = new LinearGradient(rect.Left, rect.Top, rect.Left, rect.Bottom,
					    Color.Argb(255,255,255,255), Color.Argb(255,0,0,0), Shader.TileMode.Clamp);
		    }

		    var rgb = ColorUtils.ColorFromHSV(_hue/360f,1f,1f);

		    using (_satShader = new LinearGradient(rect.Left, rect.Top, rect.Right, rect.Top,
				    Color.Argb(255,255,255,255), rgb, Shader.TileMode.Clamp))
            {
		        var mShader = new ComposeShader(_valShader, _satShader, PorterDuff.Mode.Multiply);
		        _satValPaint.SetShader(mShader);

		        canvas.DrawRect(rect, _satValPaint);
            }
		    var p = SatValToPoint(_sat, _val);

		    _satValTrackerPaint.Color = Color.Argb(255,0,0,0);
		    canvas.DrawCircle(p.X, p.Y, _paletteCircleTrackerRadius - 1f * _density, _satValTrackerPaint);

		    _satValTrackerPaint.Color = Color.Argb(255, 221, 221, 221);
            canvas.DrawCircle(p.X, p.Y, _paletteCircleTrackerRadius, _satValTrackerPaint);    
	    }

        private Point SatValToPoint(float sat, float val)
        {
		    var rect = _satValRect;
		    var height = rect.Height();
		    var width = rect.Width();

            var p = new Point
                        {
                            X = (int) (sat*width + rect.Left), 
                            Y = (int) ((1f - val)*height + rect.Top)
                        };

            return p;
	    }

        private void DrawHuePanel(Canvas canvas)
        {
		    var rect = _hueRect;

            if (BorderWidthPx > 0)
            {
			    _borderPaint.Color = _borderColor;
                canvas.DrawRect(rect.Left - BorderWidthPx,
                        rect.Top - BorderWidthPx,
                        rect.Right + BorderWidthPx,
                        rect.Bottom + BorderWidthPx,
					    _borderPaint);
		    }

		    if (_hueShader == null) {
                using(_hueShader = 
                    new LinearGradient(rect.Left, rect.Top, rect.Left, rect.Bottom, BuildHueColorArray(), null, Shader.TileMode.Clamp))
                    _huePaint.SetShader(_hueShader);
		    }

		    canvas.DrawRect(rect, _huePaint);

		    var rectHeight = 4 * _density / 2;

		    var p = HueToPoint(_hue);

            var r = new RectF
                        {
                            Left = rect.Left - _rectangleTrackerOffset,
                            Right = rect.Right + _rectangleTrackerOffset,
                            Top = p.Y - rectHeight,
                            Bottom = p.Y + rectHeight
                        };

            canvas.DrawRoundRect(r, 2, 2, _hueTrackerPaint);
	    }

        private Point HueToPoint(float hue)
        {
		    var rect = _hueRect;
		    var height = rect.Height();

            var p = new Point
                        {
                            Y = (int) (height - (hue*height/360f) + rect.Top), 
                            X = (int) rect.Left
                        };


            return p;
	    }

        private void DrawAlphaPanel(Canvas canvas){

		    if(!_showAlphaPanel || _alphaRect == null || _alphaPattern == null) return;

		    var rect = _alphaRect;

            if (BorderWidthPx > 0)
            {
			    _borderPaint.Color = _borderColor;
                canvas.DrawRect(rect.Left - BorderWidthPx,
                        rect.Top - BorderWidthPx,
                        rect.Right + BorderWidthPx,
                        rect.Bottom + BorderWidthPx,
					    _borderPaint);
		    }


		    _alphaPattern.Draw(canvas);

		    var color = ColorUtils.ColorFromHSV(_hue/360f,_sat,_val);
            var acolor = ColorUtils.ColorFromHSV(_hue/360f, _sat, _val, 0);

		    using (_alphaShader = new LinearGradient(rect.Left, rect.Top, rect.Right, rect.Top,
				    color, acolor, Shader.TileMode.Clamp))
            {
    		    _alphaPaint.SetShader(_alphaShader);

                canvas.DrawRect(rect, _alphaPaint);
            }

		    if(!string.IsNullOrEmpty(_alphaSliderText)){
                canvas.DrawText(_alphaSliderText, rect.CenterX(), rect.CenterY() + 4 * _density, _alphaTextPaint);
		    }

		    float rectWidth = 4 * _density / 2;

		    var p = AlphaToPoint(_alpha);

            var r = new RectF
                        {
                            Left = p.X - rectWidth,
                            Right = p.X + rectWidth,
                            Top = rect.Top - _rectangleTrackerOffset,
                            Bottom = rect.Bottom + _rectangleTrackerOffset
                        };

            canvas.DrawRoundRect(r, 2, 2, _hueTrackerPaint);

	    }

        private Point AlphaToPoint(int alpha)
        {
		    var rect = _alphaRect;
		    var width = rect.Width();

            var p = new Point
                        {
                            X = (int) (width - (alpha*width/0xff) + rect.Left), 
                            Y = (int) rect.Top
                        };

            return p;
	    }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            _drawingRect = new RectF
                               {
                                   Left = DrawingOffset + PaddingLeft,
                                   Right = w - DrawingOffset - PaddingRight,
                                   Top = DrawingOffset + PaddingTop,
                                   Bottom = h - DrawingOffset - PaddingBottom
                               };

            SetUpSatValRect();
            SetUpHueRect();
            SetUpAlphaRect();
        }

        private void SetUpSatValRect()
        {
		    var dRect = _drawingRect;
            var panelSide = dRect.Height() - BorderWidthPx * 2;

		    if(_showAlphaPanel){
			    panelSide -= _panelSpacing + _alphaPanelHeight;
		    }

            var left = dRect.Left + BorderWidthPx;
            var top = dRect.Top + BorderWidthPx;
            var bottom = top + panelSide;
            var right = left + panelSide;

		    _satValRect = new RectF(left,top, right, bottom);
	    }

        private void SetUpHueRect()
        {
		    var dRect = _drawingRect;

            var left = dRect.Right - _huePanelWidth + BorderWidthPx;
            var top = dRect.Top + BorderWidthPx;
            var bottom = dRect.Bottom - BorderWidthPx - (_showAlphaPanel ? (_panelSpacing + _alphaPanelHeight) : 0);
            var right = dRect.Right - BorderWidthPx;

		    _hueRect = new RectF(left, top, right, bottom);
	    }

        private void SetUpAlphaRect()
        {
		    if(!_showAlphaPanel) return;

		    var dRect = _drawingRect;

            var left = dRect.Left + BorderWidthPx;
            var top = dRect.Bottom - _alphaPanelHeight + BorderWidthPx;
            var bottom = dRect.Bottom - BorderWidthPx;
            var right = dRect.Right - BorderWidthPx;

		    _alphaRect = new RectF(left, top, right, bottom);

		    _alphaPattern = new AlphaPatternDrawable((int) (5 * _density));
            _alphaPattern.SetBounds(
                (int) Math.Round(_alphaRect.Left),
                (int) Math.Round(_alphaRect.Top),
                (int) Math.Round(_alphaRect.Right),
                (int) Math.Round(_alphaRect.Bottom)
		    );
	    }

        public override bool OnTrackballEvent(MotionEvent e)
        {
            var x = e.GetX();
		    var y = e.GetY();

		    var update = false;


		    if(e.Action == MotionEventActions.Move){

			    switch(_lastTouchedPanel)
                {
			        case PanelSatVal:
                        var sat = _sat + x/50f;
				        var val = _val - y/50f;

				        if(sat < 0f)
					        sat = 0f;
				        else if(sat > 1f)
					        sat = 1f;

				        if(val < 0f)
					        val = 0f;
				        else if(val > 1f)
					        val = 1f;

				        _sat = sat;
				        _val = val;

				        update = true;
				        break;
			        case PanelHue:
				        var hue = _hue - y * 10f;

				        if(hue < 0f){
					        hue = 0f;
				        }
				        else if(hue > 360f){
					        hue = 360f;
				        }

				        _hue = hue;

				        update = true;
				        break;
			        case PanelAlpha:

				        if(!_showAlphaPanel || _alphaRect == null)
					        update = false;
				        else
                        {
					        var alpha = (int) (_alpha - x*10);

					        if(alpha < 0)
						        alpha = 0;
					        else if(alpha > 0xff)
						        alpha = 0xff;

					        _alpha = alpha;
					        update = true;
				        }
				        break;
			    }
		    }

		    if(update)
            {
                if (ColorChanged != null)
                    ColorChanged(this, new ColorChangedEventArgs { Color = ColorUtils.ColorFromHSV(_hue / 360, _sat, _val, _alpha) });

			    Invalidate();
			    return true;
		    }

            return base.OnTrackballEvent(e);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            var update = false;

		    switch(e.Action)
            {
		        case MotionEventActions.Down:
			        _startTouchPoint = new Point((int)e.GetX(), (int)e.GetY());
			        update = MoveTrackersIfNeeded(e);
			        break;
                case MotionEventActions.Move:
			        update = MoveTrackersIfNeeded(e);
			        break;
                case MotionEventActions.Up:
			        _startTouchPoint = null;
			        update = MoveTrackersIfNeeded(e);
                    GC.Collect(); //Not sure if collecting too much here...
			        break;
		    }

		    if(update)
            {
			    if (ColorChanged != null)
                    ColorChanged(this, new ColorChangedEventArgs { Color = ColorUtils.ColorFromHSV(_hue / 360, _sat, _val, _alpha) });

			    Invalidate();
			    return true;
		    }
            return base.OnTouchEvent(e);
        }

        private bool MoveTrackersIfNeeded(MotionEvent e)
        {
		    if(_startTouchPoint == null) return false;

		    var update = false;

		    var startX = _startTouchPoint.X;
		    var startY = _startTouchPoint.Y;


		    if(_hueRect.Contains(startX, startY))
            {
			    _lastTouchedPanel = PanelHue;

			    _hue = PointToHue(e.GetY());

			    update = true;
		    }
		    else if(_satValRect.Contains(startX, startY))
            {
			    _lastTouchedPanel = PanelSatVal;

			    var result = PointToSatVal(e.GetX(), e.GetY());

			    _sat = result[0];
			    _val = result[1];

			    update = true;
		    }
		    else if(_alphaRect != null && _alphaRect.Contains(startX, startY))
		    {
		        _lastTouchedPanel = PanelAlpha;
			    _alpha = PointToAlpha((int)e.GetX());
			    update = true;
		    }

		    return update;
	    }

        private float[] PointToSatVal(float x, float y)
        {
		    var rect = _satValRect;
		    var result = new float[2];

		    var width = rect.Width();
		    var height = rect.Height();

		    if (x < rect.Left)
			    x = 0f;
		    else if (x > rect.Right)
			    x = width;
		    else
			    x = x - rect.Left;

		    if (y < rect.Top)
			    y = 0f;
		    else if(y > rect.Bottom)
			    y = height;
		    else
			    y = y - rect.Top;

		    result[0] = 1.0f / width * x;
		    result[1] = 1.0f - (1.0f / height * y);

		    return result;
	    }

        private float PointToHue(float y)
        {
		    var rect = _hueRect;

		    var height = rect.Height();

		    if (y < rect.Top)
			    y = 0f;
		    else if(y > rect.Bottom)
			    y = height;
		    else
			    y = y - rect.Top;

		    return 360f - (y * 360f / height);
	    }

        private int PointToAlpha(int x)
        {
		    var rect = _alphaRect;
		    var width = (int) rect.Width();

		    if(x < rect.Left)
			    x = 0;
		    else if(x > rect.Right)
			    x = width;
		    else
			    x = x - (int)rect.Left;

		    return 0xff - (x * 0xff / width);
	    }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width;
            int height;

            var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            var widthAllowed = MeasureSpec.GetSize(widthMeasureSpec);
            var heightAllowed = MeasureSpec.GetSize(heightMeasureSpec);

            widthAllowed = ChooseWidth(widthMode, widthAllowed);
            heightAllowed = ChooseHeight(heightMode, heightAllowed);

            if (!_showAlphaPanel)
            {

                height = (int)(widthAllowed - _panelSpacing - _huePanelWidth);

                //If calculated height (based on the width) is more than the allowed height.
                if (height > heightAllowed /*|| Tag.Equals("landscape")*/)
                {
                    height = heightAllowed;
                    width = (int)(height + _panelSpacing + _huePanelWidth);
                }
                else
                {
                    width = widthAllowed;
                }
            }
            else
            {

                width = (int)(heightAllowed - _alphaPanelHeight + _huePanelWidth);

                if (width > widthAllowed)
                {
                    width = widthAllowed;
                    height = (int)(widthAllowed - _huePanelWidth + _alphaPanelHeight);
                }
                else
                {
                    height = heightAllowed;
                }

            }

            SetMeasuredDimension(width, height);
        }

        private int ChooseWidth(MeasureSpecMode mode, int size)
        {
            if (mode == MeasureSpecMode.AtMost || mode == MeasureSpecMode.Exactly)
            {
                return size;
            }

            return GetPrefferedWidth();
        }

        private int ChooseHeight(MeasureSpecMode mode, int size)
        {
            if (mode == MeasureSpecMode.AtMost || mode == MeasureSpecMode.Exactly)
            {
                return size;
            }

            return GetPrefferedHeight();
        }

        private int GetPrefferedWidth()
        {
            var width = GetPrefferedHeight();

            if (_showAlphaPanel)
            {
                width -= (int)(_panelSpacing + _alphaPanelHeight);
            }

            return (int)(width + _huePanelWidth + _panelSpacing);
        }

        private int GetPrefferedHeight()
        {
            var height = (int)(200 * _density);

            if (_showAlphaPanel)
            {
                height += (int)(_panelSpacing + _alphaPanelHeight);
            }

            return height;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _satValPaint)
                {
                    _satValPaint.Dispose();
                    _satValPaint = null;
                }
                if (null != _satValTrackerPaint)
                {
                    _satValTrackerPaint.Dispose();
                    _satValTrackerPaint = null;
                }
                if (null != _huePaint)
                {
                    _huePaint.Dispose();
                    _huePaint = null;
                }
                if (null != _hueTrackerPaint)
                {
                    _hueTrackerPaint.Dispose();
                    _hueTrackerPaint = null;
                }
                if (null != _alphaPaint)
                {
                    _alphaPaint.Dispose();
                    _alphaPaint = null;
                }
                if (null != _alphaTextPaint)
                {
                    _alphaTextPaint.Dispose();
                    _alphaTextPaint = null;
                }
                if (null != _hueShader)
                {
                    _hueShader.Dispose();
                    _hueShader = null;
                }
                if (null != _borderPaint)
                {
                    _borderPaint.Dispose();
                    _borderPaint = null;
                }
                if (null != _valShader)
                {
                    _valShader.Dispose();
                    _valShader = null;
                }
                if (null != _satShader)
                {
                    _satShader.Dispose();
                    _satShader = null;
                }
                if (null != _alphaShader)
                {
                    _alphaShader.Dispose();
                    _alphaShader = null;
                }
                if (null != _alphaPattern)
                {
                    _alphaPattern.Dispose();
                    _alphaPattern = null;
                }
            }

            GC.Collect();

            base.Dispose(disposing);
        }
    }
}