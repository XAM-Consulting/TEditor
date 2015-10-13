using System;
using System.Drawing;

#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace PopColorPicker.iOS
{
    public static class DisplayHelper
    {
        public static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public static bool Is4InchDisplay()
        {
            return UIScreen.MainScreen.Bounds.Size.Height >= 568f;
        }
    }
}

