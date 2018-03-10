using System;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Service;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform;
using SDWebImage;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class ImageViewUrlBinding : MvxTargetBinding
    {
        #region Variables

        private readonly UIImage _alternativeImage;

        #endregion Variables

        #region Constructor

        public ImageViewUrlBinding(object target, string alternativeBundleImage = null) : base(target)
        {
            _alternativeImage = !string.IsNullOrWhiteSpace(alternativeBundleImage)
                ? UIImage.FromBundle(alternativeBundleImage)
                : null;
        }

        #endregion Constructor

        #region Binding

        private UIImageView ImageView => Target as UIImageView;

        public override void SetValue(object value)
        {
            try
            {
                var url = value as string;

                if (!string.IsNullOrWhiteSpace(url))
                {
                    ImageView.SetImage(new NSUrl(url), _alternativeImage, SDWebImageOptions.ProgressiveDownload);
                }
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        public override Type TargetType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        #endregion Binding
    }
}