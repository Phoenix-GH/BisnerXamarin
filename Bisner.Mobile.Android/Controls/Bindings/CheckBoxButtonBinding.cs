using System;
using System.Diagnostics;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public abstract class CheckBoxButtonBinding
        : MvxTargetBinding
    {
        protected virtual bool SupportTitleColor => false;

        protected int YesTitleColor;
        protected int NoTitleColor;

        protected virtual bool SupportBackgroundColor => false;

        protected int YesBackgroundColor;
        protected int NoBackgroundColor;

        protected virtual bool SupportBorder => false;

        protected int YesBorderWidth;
        protected int NoBorderWidth;

        protected int YesBorderColor;
        protected int NoBorderColor;

        public void SetButtonBackground(Button button, bool value)
        {
            var backgroundDrawable = button.Background as GradientDrawable;

            if (SupportTitleColor)
            {
                var colorId = !value ? YesTitleColor : NoTitleColor;

                var color = new Color(ContextCompat.GetColor(Application.Context, colorId));

                Debug.WriteLine($"Button title : {button.Text}, color : {color.A} {color.R} {color.G} {color.B}, value : {value}");

                button.SetTextColor(color);
            }

            if (SupportBackgroundColor)
            {
                var colorId = !value ? YesBackgroundColor : NoBackgroundColor;

                var color = new Color(ContextCompat.GetColor(Application.Context, colorId));

                backgroundDrawable?.SetColor(color);
            }

            if (SupportBorder)
            {
                var colorId = !value ? YesBorderColor : NoBorderColor;

                var color = new Color(ContextCompat.GetColor(Application.Context, colorId));

                var width = !value ? YesBorderWidth : NoBorderWidth;

                backgroundDrawable?.SetStroke(width, color);
            }
        }

        private Button Button => Target as Button;

        private bool _currentValue;

        protected CheckBoxButtonBinding(Button button)
            : base(button)
        {
            //button.Click += ButtonOnClick;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            //SetButtonBackground();
            FireValueChanged(_currentValue);
        }

        public override void SetValue(object value)
        {
            var boolValue = (bool)value;
            _currentValue = boolValue;

            var button = Button;
            if (button != null)
            {
                SetButtonBackground(button, boolValue);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var button = Button;
                if (button != null)
                {
                    //button.Click -= ButtonOnClick;
                }
            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType => typeof(bool);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;
    }
}