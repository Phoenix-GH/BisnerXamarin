using System;
using System.Diagnostics;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class EditTextErrorBinding : MvxTargetBinding
    {
        #region Variables

        private readonly int _yesColorId, _noColorId;

        #endregion Variables

        #region Constructor

        public EditTextErrorBinding(int yesColorId, int noColorId, object target) : base(target)
        {
            _yesColorId = yesColorId;
            _noColorId = noColorId;
        }

        #endregion Constructor

        #region Properties

        private EditText View => (EditText)Target;

        #endregion Properties

        #region Binding

        public override void SetValue(object value)
        {
            var boolvalue = (bool)value;

            var drawable = View.Background;

            Debug.WriteLine(drawable.GetType().FullName);

            var backgroundDrawable = ContextCompat.GetDrawable(Application.Context, Resource.Drawable.input_box);

            var shapeDrawable = backgroundDrawable as ShapeDrawable;

            if (shapeDrawable != null)
            {
                shapeDrawable.Paint.Color = new Color(ContextCompat.GetColor(Application.Context, boolvalue ? _yesColorId : _noColorId));
            }

            View.Background = shapeDrawable;
        }

        public override Type TargetType => typeof(View);

        public override MvxBindingMode DefaultMode => MvxBindingMode.Default;

        #endregion Binding
    }
}