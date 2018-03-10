using System;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Widget;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class AccessControlTextColorBinding : MvxTargetBinding
    {
        private readonly int _closedColorId, _openingColorId, _openColorId;

        public AccessControlTextColorBinding(object target, int closedColorId, int openingColorId, int openColorId) : base(target)
        {
            _closedColorId = closedColorId;
            _openingColorId = openingColorId;
            _openColorId = openColorId;
        }

        public override void SetValue(object value)
        {
            var state = (LockState)value;

            int colorId;

            switch (state)
            {
                case LockState.Close:
                    colorId = _closedColorId;
                    break;
                case LockState.Opening:
                    colorId = _openingColorId;
                    break;
                case LockState.Open:
                    colorId = _openColorId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var color = new Color(ContextCompat.GetColor(Application.Context, colorId));

            TextView.SetTextColor(color);
        }

        private TextView TextView => Target as TextView;

        public override Type TargetType => typeof(LockState);
        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;
    }
}