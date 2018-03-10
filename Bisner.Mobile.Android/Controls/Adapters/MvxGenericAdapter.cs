using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace Bisner.Mobile.Droid.Controls.Adapters
{
    public class MvxGenericAdapter<TItem> : MvxAdapter where TItem : class
    {
        #region Fields and Constructor

        public MvxGenericAdapter(Context context)
            : base(context)
        {
        }

        public MvxGenericAdapter(Context context, IMvxAndroidBindingContext bindingContext)
            : base(context, bindingContext)
        {
        }

        protected MvxGenericAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        #endregion Fields and Constructor

        #region Overrrides

        protected override View GetBindableView(View convertView, object dataContext, int templateId)
        {
            // Check if bound objects are of correct type
            var item = dataContext as TItem;
            if (item == null)
            {
                throw new Exception("Unable to cast item to type");
            }

            templateId = GetViewIdentifier?.Invoke(item) ?? throw new Exception("No identifier func set");

            var view = base.GetBindableView(convertView, dataContext, templateId);

            // If set modify cell
            ModifyViewFunc?.Invoke(view, dataContext as TItem);

            return view;
        }

        #endregion Overrides

        #region Delegates

        public Func<TItem, int> GetViewIdentifier { get; set; }

        public Action<View, TItem> ModifyViewFunc { get; set; }

        #endregion Delegates
    }
}