using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class AccessControlBackgroundBinding : MvxTargetBinding
    {
        #region Constructor

        public AccessControlBackgroundBinding(object target) : base(target)
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
                        color = UIColor.White;
                        break;
                    case LockState.Opening:
                        color = UIColor.White;
                        break;
                    case LockState.Open:
                        color = Appearance.Colors.BisnerBlue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                View.BackgroundColor = color;
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