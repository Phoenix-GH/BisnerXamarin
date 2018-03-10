using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class MembersButtonBinding : CheckBoxButtonBinding
    {
        public MembersButtonBinding(UIButton button) : base(button)
        {
            YesTitleColor = Appearance.Colors.White;
            NoTitleColor = Appearance.Colors.SubTextColor;

            YesBackgroundColor = Appearance.Colors.BisnerBlue;
            NoBackgroundColor = UIColor.Clear;

            NotBorderWidth = 1;
            YesBorderWidth = 0;

            NoBorderColor = Appearance.Colors.SubTextColor;
            YesBorderColor = UIColor.Clear;
        }

        protected override bool SupportBorderColor { get { return true; } }

        protected override bool SupportBorderWidth { get { return true; } }

        protected override bool SupportImage { get { return false; } }

        protected override bool SupportTitleColor { get { return true; } }

        protected override bool SupportBackgroundColor { get { return true; } }
    }
}