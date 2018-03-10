using Android.Graphics;

namespace Bisner.Mobile.Droid.Controls
{
    public static class Appearance
    {
        public static class Colors
        {
            public static Color White { get { return Color.White; } }

            public static Color WhiteTransparent40 { get { return new Color(255, 255, 255, 102); } }

            public static Color WhiteTransparent10 { get { return new Color(255, 255, 255, 25); } }

            public static Color RulerColor { get { return new Color(244, 244, 244); } }

            public static Color Green { get { return new Color(99, 193, 172); } }

            public static Color MenuBackground { get { return new Color(38, 84, 101); } }

            public static Color Grey { get { return new Color(124, 124, 124); } }

            public static Color BackgroundColor { get { return new Color(249, 249, 249); } }

            // Text
            public static Color DefaultTextColor { get { return new Color(105, 105, 105); } }
            public static Color SubTextColor { get { return new Color(187, 187, 187); } }
            public static Color FeedButtonNormalTextColor { get { return new Color(150, 150, 150); } }
            public static Color FeedButtonActiveTextColor { get { return new Color(0, 163, 217); } }
            public static Color BarShadowColor { get { return new Color(234, 234, 234); } }
        }
    }
}