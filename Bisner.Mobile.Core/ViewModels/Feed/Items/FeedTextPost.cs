namespace Bisner.Mobile.Core.ViewModels.Feed.Items
{
    public class FeedTextPost : FeedPostBase, IFeedTextPost
    {
        public override FeedPostType Type
        {
            get { return FeedPostType.Text; }
        }

        public string Text { get; set; }
    }

    public class FeedLinkTextPost : FeedTextPost
    {
        public override FeedPostType Type
        {
            get { return FeedPostType.TextLink; }
        }
    }
}