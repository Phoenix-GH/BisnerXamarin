using System;
using Bisner.Mobile.Core.ViewModels.Manage;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Manage
{
    public class ManageTemplateSelector : MvxTemplateSelector<IManageItem>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(IManageItem forItemObject)
        {
            if (forItemObject is ManageLabel)
                return Resource.Layout.manage_item_label;

            if (forItemObject is ManageUser)
                return Resource.Layout.manage_item_user;

            // Manage item last because user inherits from it
            if (forItemObject is ManageItem)
                return Resource.Layout.manage_item;

            throw new Exception($"No layout for item of type {forItemObject.GetType().FullName}");
        }
    }
}