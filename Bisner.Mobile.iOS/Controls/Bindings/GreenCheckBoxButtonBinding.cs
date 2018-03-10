using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class GreenCheckBoxButtonBinding : CheckBoxButtonBinding
    {
        public GreenCheckBoxButtonBinding(UIButton button) : base(button)
        {
            YesBackgroundColor = Appearance.Colors.Green;
            NoBackgroundColor = Appearance.Colors.White;

            YesBorderColor = Appearance.Colors.Green;
            NoBorderColor = Appearance.Colors.Grey;

            YesBorderWidth = 2;
            NotBorderWidth = 2;
        }

        protected override bool SupportBorderWidth { get { return true; } }

        protected override bool SupportBorderColor { get { return true; } }

        protected override bool SupportBackgroundColor { get { return true; } }
    }
}