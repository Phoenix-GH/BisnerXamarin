using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class RoomIndexItemView : MvxTableViewCell
    {
        public static NSString Identifier = new NSString("RoomIndexItemView");

        public RoomIndexItemView(IntPtr handle) : base(handle)
        {
            SetupBindings();
        }

        public void InitStyle()
        {
            btnBookRoom.Layer.CornerRadius = btnBookRoom.Frame.Height / 2.0f;
            btnBookRoom.Layer.MasksToBounds = true;
            btnBookRoom.Layer.BorderWidth = 1.0f;
            btnBookRoom.Layer.BorderColor = UIColor.White.CGColor;

            btnBookRoom.Font = iOS.Appearance.Fonts.LatoBoldWithSize(14);
            lblTitle.Font = iOS.Appearance.Fonts.LatoBlackWithSize(28);
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<RoomIndexItemView, RoomIndexItemViewModel>();
                set.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(lblTitle).To(vm => vm.Title);
                set.Bind(vwStatus).For("AvailableBackground").To(vm => vm.IsAvailable);
                set.Bind(btnBookRoom).To(vm => vm.RoomBookCommand);
                set.Bind(btnRoomBook).To(vm => vm.BookRoomCommand);
                set.Apply();
            });
        }

        public override void LayoutSubviews()
        {
            ivContent.Transparency = 0.5f;

            base.LayoutSubviews();
        }
    }
}