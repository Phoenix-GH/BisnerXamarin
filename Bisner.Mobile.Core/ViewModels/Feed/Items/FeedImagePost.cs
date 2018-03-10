namespace Bisner.Mobile.Core.ViewModels.Feed.Items
{
    public class FeedImagePost : FeedTextPost, IFeedImagePost
    {
        public override FeedPostType Type
        {
            get { return FeedPostType.Image; }
        }
    }

    public class FeedDoubleImagePost : FeedTextPost, IFeedImagePost
    {
        public override FeedPostType Type { get { return FeedPostType.TwoImages; } }
    }

    public class FeedMultiImagePost : FeedTextPost, IFeedImagePost
    {
        public override FeedPostType Type { get { return FeedPostType.ThreeOrMoreImages; } }
    }
}