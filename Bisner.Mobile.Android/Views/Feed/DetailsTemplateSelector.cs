using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Feed;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Feed
{
    public class DetailsTemplateSelector : MvxTemplateSelector<IItemBase>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(IItemBase forItemObject)
        {
            if (forItemObject is IComment)
                return Resource.Layout.comment_item;

            return Resource.Layout.feed_item_simple;
        }
    }
}