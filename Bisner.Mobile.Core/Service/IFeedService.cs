using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface IFeedService
    {
        Task<FeedResponseModel> GetHomeFeedAsync(ApiPriority priority, DateTime? olderThen);
        Task<FeedResponseModel> GetCompanyFeedAsync(ApiPriority priority, Guid companyId, DateTime? olderThen);
        Task<FeedResponseModel> GetGroupFeedAsync(ApiPriority priority, Guid groupId, DateTime? olderThen);
        Task<FeedResponseModel> GetPostAsync(ApiPriority priority, Guid postId);
        Task<bool> DeletePostAsync(Guid postId);
        Task<bool> CreateTextPostAsync(string inputText, Guid? feedId, List<Guid> mentionedUserIds, ParentType parentType = ParentType.Unknown);
        Task<bool> CreateImagePostAsync(string inputText, List<Stream> imageStreamList, Guid? feedId, List<Guid> mentionedUserIds, ParentType parentType = ParentType.Unknown);
        Task<bool> ReportPostAsync(Guid id);
        Task<bool> CommentAsync(Guid postId, string commentInput, List<Guid> mentionedUsers = null);
        Task<bool> FollowPostAsync(Guid id, bool isFollowing);
        Task<bool> LikePostAsync(Guid id, bool hasLiked);
        Task<bool> ReportCommentAsync(Guid id);
    }
}