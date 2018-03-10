using Android.Views;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.AccessControl
{
    public class AccessControlAdapter : MvxRecyclerAdapter
    {
        #region Constructor

        #endregion Constructor

        #region Adapter

        protected override View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            var view = base.InflateViewForHolder(parent, viewType, bindingContext);
            
            return view;
        }

        #endregion Adapter

        #region Setup



        #endregion Setup
    }

    public class AccessControlTemplateSelector : MvxTemplateSelector<AccessControlItemViewModel>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(AccessControlItemViewModel forItemObject)
        {
            return Resource.Layout.accesscontrol_item;
        }
    }
}