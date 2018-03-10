using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class BookingsItemView : MvxTableViewCell
    {
        public static NSString Identifier = new NSString("BookingsItemView");

        public BookingsItemView(IntPtr handle) : base(handle)
        {
            SetupBindings();
        }

        public void InitStyle()
        {
            ivContent.Layer.CornerRadius = 6f;
            ivContent.Layer.MasksToBounds = true;

            lblTitle.Font = iOS.Appearance.Fonts.LatoBoldWithSize(14);
            lblCheck.Font = iOS.Appearance.Fonts.LatoWithSize(14);
            lblDate.Font = iOS.Appearance.Fonts.LatoWithSize(14);

            var separatorLineView =
                new UIView(new CGRect(115, 0, this.ContentView.Frame.Width, 1))
                {
                    BackgroundColor = UIColor.FromRGB(232, 232, 232)
                };
            // #e8e8e8
            ContentView.AddSubview(separatorLineView);
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<BookingsItemView, BookingsItemViewModel>();
                set.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(lblTitle).To(vm => vm.Title);
                set.Bind(lblDate).To(vm => vm.Date);
                set.Bind(lblCheck).To(vm => vm.Check);
                set.Apply();
            });
        }
    }
}