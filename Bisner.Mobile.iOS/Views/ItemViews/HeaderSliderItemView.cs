using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class HeaderSliderItemView : MvxCollectionViewCell
    {
        public HeaderSliderItemView(IntPtr handle) : base(handle)
        {
            SetupBindings();
        }

        public void InitStyle()
        {
            btnBookRoom.Layer.CornerRadius = btnBookRoom.Frame.Height / 2.0f;
            btnBookRoom.Layer.MasksToBounds = true;
            btnBookRoom.Layer.BorderWidth = 1.0f;
            btnBookRoom.Layer.BorderColor = UIColor.White.CGColor;

            ivContent.Transparency = 0.5f;

            lblTitle.Font = iOS.Appearance.Fonts.LatoBlackWithSize(28);
            lblDate.Font = iOS.Appearance.Fonts.LatoWithSize(16);
            lblShowAll.Font = iOS.Appearance.Fonts.LatoWithSize(14);
            lblSubTitle.Font = iOS.Appearance.Fonts.LatoWithSize(12);
            btnBookRoom.Font = iOS.Appearance.Fonts.LatoWithSize(14);
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<HeaderSliderItemView, HeaderSliderItemViewModel>();
                set.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(lblTitle).To(vm => vm.Title);
                set.Bind(lblSubTitle).To(vm => vm.SubTitle);
                set.Bind(lblSubTitle).For("Visibility").To(vm => vm.SubtitleVisible).WithConversion("Visibility");

                set.Bind(lblDate).To(vm => vm.Date).WithConversion("ShortDate");
                set.Bind(lblDate).For("Visibility").To(vm => vm.DateVisible).WithConversion("Visibility");

                set.Bind(btnDetail).To(vm => vm.DetailBtnClickedCommand);
                set.Bind(btnDetail).For("Visibility").To(vm => vm.DetailButtonVisisble).WithConversion("Visibility");

                set.Bind(lblShowAll).To(vm => vm.ShowAllText);
                set.Bind(btnShowAll).To(vm => vm.ShowAllBtnClickedCommand);

                set.Bind(btnBookRoom).For("Visibility").To(vm => vm.BookRoomButtonVisible).WithConversion("Visibility");
                set.Bind(btnBookRoom).To(vm => vm.BookRoomCommand);

                set.Bind(vwBookingStatus).For("AvailableBackground").To(vm => vm.IsAvailable);
                set.Bind(vwBookingStatus).For("Visibility").To(vm => vm.StatusVisible).WithConversion("Visibility");

                set.Apply();
            });
        }
    }
}