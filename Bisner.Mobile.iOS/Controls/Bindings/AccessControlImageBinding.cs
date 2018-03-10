using System;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class AccessControlImageBinding : MvxTargetBinding
    {
        #region Variables
        
        #endregion Variables

        #region Constructor

        public AccessControlImageBinding(object target) : base(target)
        {
        }

        #endregion Constructor

        #region Binding

        private UIImageView ImageView => Target as UIImageView;

        public override void SetValue(object value)
        {
            try
            {
                UIImage image;
                var state = (LockState)value;

                switch (state)
                {
                    case LockState.Close:
                        image = UIImage.FromFile("Icons/doorlock_icon_001.png");
                        break;
                    case LockState.Opening:
                        image = UIImage.FromFile("Icons/doorlock_icon_002.png");
                        break;
                    case LockState.Open:
                        image = UIImage.FromFile("Image/doorlock_icon_003.png");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                ImageView.Image = image;
                ImageView.ClipsToBounds = true;
                ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        public override Type TargetType => typeof(LockState);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        #endregion Binding
    }
}