using System;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public partial class LoginButton : UIButton
    {
        public LoginButton()
        {
            SetupAppearance();
        }

        public LoginButton(IntPtr handle)
            : base(handle)
        {
            SetupAppearance();
        }

        private void SetupAppearance()
        {
            SetTitleColor(iOS.Appearance.Colors.LoginButtonText, UIControlState.Normal);
            Layer.BackgroundColor = iOS.Appearance.Colors.LoginButton.CGColor;
            Layer.CornerRadius = 4;
        }
    }

    public class BlueButton : UIButton
    {
        public BlueButton()
        {
            SetupAppearance();
        }

        public BlueButton(IntPtr handle)
            : base(handle)
        {
            SetupAppearance();
        }

        private void SetupAppearance()
        {
            Font = iOS.Appearance.Fonts.LatoWithSize(18);
            SetTitleColor(iOS.Appearance.Colors.White, UIControlState.Normal);
            Layer.BackgroundColor = iOS.Appearance.Colors.BisnerBlue.CGColor;
            Layer.CornerRadius = 4;
        }
    }
}
