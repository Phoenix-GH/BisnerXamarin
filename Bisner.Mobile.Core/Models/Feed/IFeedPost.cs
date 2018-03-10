using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Feed
{
    public interface IFeedPost : IFeedItem, IDisplayUser
    {
        #region Post
        
        string Text { get; set; }
        string Title { get; set; }
        DateTime ItemDateTime { get; set; }
        string ItemDateTimeText { get; }
        string Location { get; set; }
        int NumberOfImages { get; }
        List<IImage> Images { get; set; }
        string MainImageUrl { get; }
        MvxCommand MainImageCommand { get; }
        string LeftSubImageUrl { get; }
        MvxCommand LeftSubImageCommand { get; }
        string RightSubImageUrl { get; }
        MvxCommand RightSubImageCommand { get; }

        #endregion Post

        #region Comments

        int NumberOfComments { get; set; }
        bool HasCommented { get; set; }
        string CommentButtonText { get; }
        MvxCommand CommentCommand { get; }

        #endregion Comments

        #region Likes

        int NumberOfLikes { get; set; }
        bool HasLiked { get; set; }
        string LikeButtonText { get; }
        MvxCommand LikeCommand { get; }

        #endregion Likes

        #region User

        MvxCommand UserCommand { get; }

        #endregion User

        #region Update

        void Update(IFeedPost post);

        #endregion Update

        #region RelatedItem

        Guid RelatedItemId { get; set; }

        string PostType { get; set; }

        #endregion RelatedItem
    }
}