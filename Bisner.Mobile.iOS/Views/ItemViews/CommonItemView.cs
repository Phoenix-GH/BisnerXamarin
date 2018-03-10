using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class CommonItemView : MvxTableViewCell
    {
        public static readonly NSString Identifier = new NSString("CommonItemView");

        public CommonItemView(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<CommonItemView, CommonItemViewModel>();
                set.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(lblTitle).To(vm => vm.Title);
                set.Apply();
            });
        }

        public void InitStyle()
        {
            ivContent.BackgroundColor = UIColor.FromRGB(226, 226, 226);
            ivContent.Layer.CornerRadius = 5.0f;
            lblTitle.TextColor = UIColor.FromRGB(68, 68, 68);
            lblTitle.Font = iOS.Appearance.Fonts.LatoWithSize(14);
        }
    }
}