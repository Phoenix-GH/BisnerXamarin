using System;
using System.Diagnostics;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class ViewBackgroundBoolBinding : MvxTargetBinding
    {
        #region Constructor

        public ViewBackgroundBoolBinding(object target, UIColor trueColor, UIColor falseColor) : base(target)
        {
            TrueColor = trueColor;
            FalseColor = falseColor;
        }

        #endregion Constructor

        #region Binding

        private UIView View => Target as UIView;

        public override void SetValue(object value)
        {
            try
            {
                var boolValue = (bool)value;

                View.BackgroundColor = boolValue ? TrueColor : FalseColor;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion Binding

        #region Properties

        public UIColor TrueColor { get; private set; }

        public UIColor FalseColor { get; private set; }

        public override Type TargetType => typeof(UIColor);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        #endregion Properties
    }
}