using Bisner.Constants;
using Bisner.Mobile.Core.Models.Feed;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Feed
{
    public class FeedTemplateSelector : MvxTemplateSelector<IFeedItem>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(IFeedItem forItemObject)
        {
            if (forItemObject is GroupFeedItem)
            {
                return Resource.Layout.feed_item_group;
            }

            if (forItemObject is CompanyFeedItem)
            {
                return Resource.Layout.feed_item_company;
            }

            if (forItemObject is FeedPost)
            {
                var feedPost = forItemObject as FeedPost;

                if (feedPost.PostType == ProviderNames.WhitelabelEventPostProvider)
                    return Resource.Layout.feed_item_event;

                if (feedPost.NumberOfImages == 1)
                {
                    return Resource.Layout.feed_item_image;
                }
                else if (feedPost.NumberOfImages == 2)
                {
                    return Resource.Layout.feed_item_2_images;
                }
                else if (feedPost.NumberOfImages > 2)
                {
                    return Resource.Layout.feed_item_multiple_images;
                }
            }

            return Resource.Layout.feed_item_simple;
        }
    }
}