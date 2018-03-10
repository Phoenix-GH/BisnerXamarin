using System;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    public class GroupsTemplateSelector : MvxTemplateSelector<IGroup>
    {
        private readonly WeakReference<GroupsViewModel> _viewModelReference;

        public GroupsTemplateSelector(GroupsViewModel viewModel)
        {
            _viewModelReference = new WeakReference<GroupsViewModel>(viewModel);
        }

        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(IGroup forItemObject)
        {
            GroupsViewModel groupsViewModel;

            if (_viewModelReference.TryGetTarget(out groupsViewModel))
            {
                var index = groupsViewModel.Items.IndexOf(forItemObject);

                if (index == 0)
                {
                    return Resource.Layout.group_item_image;
                }
            }

            return Resource.Layout.group_item;
        }
    }
}