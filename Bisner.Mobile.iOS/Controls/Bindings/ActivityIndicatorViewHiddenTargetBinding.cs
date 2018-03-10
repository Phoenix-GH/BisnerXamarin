using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    /// <summary>
    /// Custom Binding for UIActivityIndicator Hidden. 
    /// This binding will ensure the indicator animates when shown and stops when hidden
    /// </summary>
    public class ActivityIndicatorViewHiddenTargetBinding : MvxConvertingTargetBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityIndicatorViewHiddenTargetBinding"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public ActivityIndicatorViewHiddenTargetBinding(UIActivityIndicatorView target)
            : base(target)
        {
            if (target == null)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Error,
                    "Error - UIActivityIndicatorView is null in ActivityIndicatorViewHiddenTargetBinding");
            }
        }

        /// <summary>
        /// Gets the default binding mode.
        /// </summary>
        /// <value>
        /// The default mode.
        /// </value>
        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        /// <summary>
        /// Gets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public override System.Type TargetType
        {
            get { return typeof(bool); }
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        protected UIActivityIndicatorView View
        {
            get { return Target as UIActivityIndicatorView; }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIActivityIndicatorView)target;
            if (view == null)
            {
                return;
            }

            view.Hidden = (bool)value;

            if (view.Hidden)
            {
                view.StopAnimating();
            }
            else
            {
                view.StartAnimating();
            }
        }
    }
}