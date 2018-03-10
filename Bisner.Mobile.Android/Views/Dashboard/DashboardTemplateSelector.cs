using System;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Company;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    public class DashboardTemplateSelector : MvxTemplateSelector<CommonItemViewModel>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(CommonItemViewModel forItemObject)
        {
            return Resource.Layout.dashboard_item_menu;

            // Old dashboard
            //if (forItemObject is DashboardButtonItem)
            //    return Resource.Layout.dashboard_item_button;

            //if (forItemObject is ICompany)
            //    return Resource.Layout.dashboard_item_company;

            throw new Exception($"Not template for item of type {forItemObject.GetType().FullName}");
        }
    }
}