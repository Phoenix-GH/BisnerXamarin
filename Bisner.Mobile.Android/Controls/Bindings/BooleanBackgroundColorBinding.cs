using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class BooleanBackgroundColorBinding : MvxTargetBinding
    {
        #region Variables

        private readonly Color _yesColor, _noColor;

        #endregion Variables

        #region Constructor

        public BooleanBackgroundColorBinding(int trueColorId, int falseColorId, View view) : base(view)
        {
            _yesColor = new Color(ContextCompat.GetColor(Application.Context, trueColorId));
            _noColor = new Color(ContextCompat.GetColor(Application.Context, falseColorId));
        }

        #endregion Constructor

        #region Background

        private void SetBackground(bool value)
        {
            if (View != null)
                View.Background = new ColorDrawable(value ? _yesColor : _noColor);
        }

        private View View => Target as View;

        #endregion Background

        #region Binding

        public override void SetValue(object value)
        {
            var boolValue = (bool)value;

            SetBackground(boolValue);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {

            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType => typeof(ColorDrawable);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        #endregion Binding
    }
}