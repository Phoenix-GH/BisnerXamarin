using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class TimePickerItemView : MvxTableViewCell
    {
        public static NSString Identifier = new NSString("TimePickerItemView");

        public TimePickerItemView(IntPtr handle) : base(handle)
        {
        }

        public void InitStyle()
        {
            this.txvReserve.Layer.BorderColor = UIColor.FromRGB(249, 173, 136).CGColor;
            this.txvReserve.Layer.BorderWidth = 1;
            this.txvReserve.Layer.CornerRadius = 4f;
            this.txvReserve.Layer.MasksToBounds = true;
        }

        public void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var bindingSet = this.CreateBindingSet<TimePickerItemView, TimePickerItemViewModel>();
                bindingSet.Bind(lblTime).To(vm => vm.TimeString);
                bindingSet.Bind(txvReserve).For("Visibility").To(vm => vm.IsSelected).WithConversion("Visibility");
                bindingSet.Apply();
            });
        }

    }
}