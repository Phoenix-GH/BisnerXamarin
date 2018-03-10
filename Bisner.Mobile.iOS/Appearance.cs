using Bisner.Mobile.iOS.Helpers;
using UIKit;

namespace Bisner.Mobile.iOS
{
    public static class Appearance
    {
        public static void SetAppearance()
        {
            //UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

            UINavigationBar.Appearance.BarTintColor = Colors.NavBarColor;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = Colors.NavBarTextColor,
            });
            UINavigationBar.Appearance.ShadowImage = ImageHelper.GetSinglePixelBackgroundImage(Colors.NavBarShadowColor);
            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);

            UITabBar.Appearance.BarTintColor = Colors.TabBarColor;
            UITabBar.Appearance.ShadowImage = ImageHelper.GetSinglePixelBackgroundImage(Colors.TabBarShadowColor);
            UITabBar.Appearance.BackgroundImage = new UIImage();
            UITabBarItem.Appearance.TitlePositionAdjustment = new UIOffset(0, 20);
            // Tinting the icons of the tab bar
            UITabBar.Appearance.TintColor = Colors.SelectedTabColor;
            // Tint of the bar background
            UITabBar.Appearance.BarTintColor = Colors.White;

        }

        /// <summary>
        /// Custom fonts
        /// </summary>
        public static class Fonts
        {
            #region Calibri

            public static UIFont Calibri11 { get { return UIFont.FromName("Calibri", 11f); } }

            public static UIFont Calibri14 { get { return UIFont.FromName("Calibri", 14f); } }

            public static UIFont CalibriBold14 { get { return UIFont.FromName("Calibri-Bold", 14f); } }

            public static UIFont Calibri20 { get { return UIFont.FromName("Calibri", 20f); } }

            public static UIFont CalibriBold20 { get { return UIFont.FromName("Calibri-Bold", 20f); } }

            #endregion Calibri

            #region Lato

            public static UIFont LatoWithSize(float size)
            {
                return GetFontWithNameAndSize("Lato-Regular", size);
            }

            public static UIFont LatoBoldWithSize(float size)
            {
                return GetFontWithNameAndSize("Lato-Bold", size);
            }

            public static UIFont LatoBlackWithSize(float size)
            {
                return GetFontWithNameAndSize("Lato-Black", size);
            }

            private static UIFont GetFontWithNameAndSize(string name, float size)
            {
                return UIFont.FromName(name, size);
            }

            #endregion Lato
        }

        public static class Colors
        {
            // Nav bars
            public static UIColor NavBarColor { get { return UIColor.White; } }
            public static UIColor NavBarShadowColor { get { return UIColor.FromRGB(240, 240, 240); } }
            public static UIColor NavBarTextColor { get { return UIColor.FromRGB(68, 68, 68); } }

            // Tab bar
            public static UIColor TabBarColor { get { return UIColor.White; } }
            public static UIColor TabBarShadowColor { get { return UIColor.FromRGB(240, 240, 240); } }
            public static UIColor SelectedTabColor { get { return UIColor.FromRGB(0, 163, 217); } }
            public static UIColor UnselectedTabColor { get { return UIColor.FromRGB(178, 178, 178); } }

            // Buttons links
            public static UIColor BisnerBlue { get { return UIColor.FromRGB(0, 163, 217); } }


            public static UIColor BarShadowColor { get { return UIColor.FromRGB(234, 234, 234); } }

            public static UIColor White { get { return UIColor.White; } }

            public static UIColor RulerColor { get { return UIColor.FromRGB(244, 244, 244); } }

            public static UIColor Green { get { return UIColor.FromRGB(99, 193, 172); } }

            public static UIColor MenuBackground { get { return UIColor.FromRGB(38, 84, 101); } }

            public static UIColor Grey { get { return UIColor.FromRGB(124, 124, 124); } }

            public static UIColor BackgroundColor { get { return UIColor.FromRGB(249, 249, 249); } }

            public static UIColor TextFieldBorderColor { get { return UIColor.FromRGB(219, 219, 219); } }

            // Login
            public static UIColor LoginButton { get { return UIColor.FromRGB(99, 193, 172); } }
            public static UIColor LoginButtonText { get { return UIColor.White; } }

            public static UIColor LoginBottomButtonTextColor { get { return White.ColorWithAlpha(0.5f); } }
            public static UIColor LoginInputTextColor { get { return White.ColorWithAlpha(0.5f); } }
            public static UIColor LoginRulerColor { get { return White.ColorWithAlpha(0.5f); } }

            // Text
            public static UIColor DefaultTextColor { get { return UIColor.FromRGB(68, 68, 68); } }
            public static UIColor SubTextColor { get { return UIColor.FromRGB(187, 187, 187); } }
            public static UIColor ChatMessageColor { get { return UIColor.FromRGB(103, 103, 103); } }
            public static UIColor FeedButtonNormalTextColor { get { return UIColor.FromRGB(150, 150, 150); } }
            public static UIColor UserCardSubText { get { return UIColor.FromRGB(147, 147, 147); } }

            // BackPanel
            public static UIColor BackPanelShadow { get { return UIColor.FromRGB(235, 235, 235); } }
            public static UIColor BackPanelBorderTop { get { return UIColor.FromRGB(241, 241, 241); } }
            public static UIColor BackPanelBorderBottom { get { return UIColor.FromRGB(234, 234, 234); } }
            public static UIColor HeaderGreen { get { return UIColor.FromRGB(76, 175, 80); } }

            // Navigation
            public static UIColor NatificationPanel { get { return UIColor.FromRGB(18, 65, 80); } }
            public static UIColor Error { get { return UIColor.Red; } }

            // User background
            public static UIColor UserModalBackground { get { return UIColor.FromRGB(28, 65, 82); } }
        }
    }
}
