using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class AccessControlBorderBinding : MvxTargetBinding
    {
        #region Constructor

        public AccessControlBorderBinding(object target) : base(target)
        {
        }

        #endregion Constructor

        #region Binding

        private UIView View => Target as UIView;

        public override void SetValue(object value)
        {
            try
            {
                var state = (LockState)value;

                UIColor color;

                switch (state)
                {
                    case LockState.Close:
                        color = UIColor.FromRGB(230, 230, 230);
                        break;
                    case LockState.Opening:
                        color = Appearance.Colors.BisnerBlue;
                        break;
                    case LockState.Open:
                        color = Appearance.Colors.BisnerBlue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                View.Layer.BorderColor = color.CGColor;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion Binding

        #region Properties

        public override Type TargetType => typeof(UIColor);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        #endregion Properties
    }
}