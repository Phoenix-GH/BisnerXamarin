using System;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Controls.Adapters
{
    public class MvxGenericRecyclerAdapter<TItem> : MvxRecyclerAdapter
    {
        #region Variables

        #endregion Variables

        #region Constructor

        public MvxGenericRecyclerAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
        }

        #endregion Constructor

        #region Delegates

        public Action<View> ModifyViewFunc { get; set; }

        #endregion Delegates

        #region Adapter

        protected override View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            var view = base.InflateViewForHolder(parent, viewType, bindingContext);

            ModifyViewFunc?.Invoke(view);

            return view;
        }

        #endregion Adapter
    }
}