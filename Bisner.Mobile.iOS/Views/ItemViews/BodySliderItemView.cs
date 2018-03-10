using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    public partial class BodySliderItemView : MvxCollectionViewCell
    {
        public BodySliderItemView(IntPtr handle) : base(handle)
        {
            SetupBindings();
        }

        public void InitStyle(bool overlay)
        {
            ivContent.Layer.CornerRadius = 4.0f;
            ivContent.Layer.MasksToBounds = true;

            if (overlay)
            {
                ivContent.Transparency = 0.5f;
            }
            ivContent.ContentMode = UIViewContentMode.ScaleAspectFill;
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<BodySliderItemView, BodySliderItemViewModel>();
                set.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(ivContent.Tap()).For(t => t.Command).To(vm => vm.SelectCommand);
                set.Apply();
            });
        }
    }
}