using Bisner.Mobile.iOS.Extensions;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class CommentButtonBinding : CheckBoxButtonBinding
    {
        public CommentButtonBinding(UIButton button)
            : base(button)
        {
            YesTitleColor = Appearance.Colors.BisnerBlue;
            NoTitleColor = Appearance.Colors.FeedButtonNormalTextColor;

            YesImage = UIImage.FromFile("Icons/comment_post_active.png").ImageWithColor(Appearance.Colors.BisnerBlue);
            NoImage = UIImage.FromFile("Icons/comment_post_normal.png").ImageWithColor(Appearance.Colors.FeedButtonNormalTextColor);
        }

        protected override bool SupportImage { get { return true; } }

        protected override bool SupportTitleColor { get { return true; } }
    }
}