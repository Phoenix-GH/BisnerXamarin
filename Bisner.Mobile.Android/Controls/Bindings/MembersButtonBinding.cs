using System;
using System.Diagnostics;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class MembersButtonBinding : MvxAndroidTargetBinding
    {
        #region Variables

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Properties

        #endregion Properties

        private Button Button => Target as Button;

        public MembersButtonBinding(Button button) : base(button)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var boolValue = !(bool)value;

            if (Button != null)
            {
                SetButtonBackground(boolValue);
            }
        }

        public override Type TargetType => typeof(bool);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public void SetButtonBackground(bool value)
        {
            var color = new Color(ContextCompat.GetColor(Application.Context, value ? Resource.Color.white : Resource.Color.subtextcolor));

            Debug.WriteLine($"Button title : {Button.Text}, color : {color.A} {color.R} {color.G} {color.B}, value : {value}");

            Button.SetTextColor(color);

            var backgroundDrawable = ContextCompat.GetDrawable(Application.Context,
                value
                    ? Resource.Drawable.members_button_background_active
                    : Resource.Drawable.members_button_background_inactive);

            Button.Background = backgroundDrawable;
        }
    }
}