using System;
using Android.Widget;
using System.Runtime.Remoting.Contexts;
using Android.Util;
using Android.App;
using Android.Graphics;

namespace TEditor
{
    public class LinearLayoutDetectsSoftKeyboard : LinearLayout
    {

        public LinearLayoutDetectsSoftKeyboard(Android.Content.Context context) : base(context)
        {
        }


        public LinearLayoutDetectsSoftKeyboard(Android.Content.Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public LinearLayoutDetectsSoftKeyboard(Android.Content.Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public LinearLayoutDetectsSoftKeyboard(Android.Content.Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {

        }

        public Action<bool, int> onKeyboardShown;

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            Activity activity = this.Context as Activity;
            Rect rect = new Rect();
            activity.Window.DecorView.GetWindowVisibleDisplayFrame(rect);
            int VisibleHeight = rect.Height();
            Point size = new Point();
            activity.WindowManager.DefaultDisplay.GetSize(size);
            int screenHeight = size.Y;
            int diff = screenHeight - VisibleHeight;
            if (onKeyboardShown != null)
            {
                // assume all soft keyboards are at least 128 pixels high
                // screenHeight - height means that when user long click the editor past and copy menu will be shown, it is the height of menu
                onKeyboardShown.Invoke((diff > 128) && VisibleHeight != 0, VisibleHeight - (screenHeight - height));
            }
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }
    }
}

