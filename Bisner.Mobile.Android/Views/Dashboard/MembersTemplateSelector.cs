using System;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Company;
using Bisner.Mobile.Core.Models.General.User;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    public class MembersTemplateSelector : MvxTemplateSelector<IItemBase>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            switch (fromViewType)
            {
                case 0:
                    return Resource.Layout.members_user_item;
                case 1:
                    return Resource.Layout.members_company_item;
                case 2:
                    return Resource.Layout.members_button_item;
                default:
                    throw new ArgumentException("Unknown layout type in MembersTemplateSelector");
            }
        }

        protected override int SelectItemViewType(IItemBase forItemObject)
        {
            if (forItemObject is IUser)
                return 0;

            if (forItemObject is ICompany)
                return 1;

            // Members button item
            return 2;
        }
    }
}