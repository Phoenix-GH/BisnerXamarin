using System;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Feed.Items
{
    public interface IFeedPost
    {
        FeedPostType Type { get; }
        Guid Id { get; set; }
        DateTime DateTime { get; set; }
        string AvatarUrl { get; set; }
        string DisplayName { get; set; }
        bool HasCommented { get; }
        string CommentButtonText { get; }
        MvxCommand CommentCommand { get; }
        bool HasLiked { get; }
        string LikeButtonText { get; }
        MvxCommand LikeCommand { get; }
        MvxCommand UserCommand { get; }
    }
}