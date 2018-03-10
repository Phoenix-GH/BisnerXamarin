using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IFeedApi
    {
        //[Get("/Api/Feed/GetHomeFeedSticky")]
        ////Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetHomeFeedSticky();

        //[Get("/Api/Feed/GetHomeFeed")]
        //Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetHomeFeed(DateTime? olderThen);

        [Get("/Api/Feed/GetHomeFeed?olderThen={olderThenFormatted}")]
        Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetHomeFeed([AliasAs("olderThenFormatted")]string olderThenFormatted);

        //[Get("/Api/Feed/GetFeed")]
        //Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetFeed(Guid id, DateTime? olderThen);

        [Get("/Api/Feed/GetFeed?olderThen={olderThen}")]
        Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetFeed(Guid id, [AliasAs("olderThen")]string olderThen);

        //[Get("/Api/Feed/GetFeedSticky")]
        //Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetFeedSticky(Guid id);

        //[Get("/Api/Feed/GetUserFeed")]
        //Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetUserFeed(Guid id, DateTime? olderThen);

        [Get("/Api/Feed/GetPost")]
        Task<ApiResponse<ApiWhitelabelFeedPostModel, ApiRelatedItemsModel>> GetPost(Guid id);

        //[Get("/Api/Feed/MailPostToSelf")]
        //Task<ApiResponse<bool>> MailPostToSelf(Guid id);

        //[Get("/Api/Feed/GetCompanyFeed")]
        //Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetCompanyFeed(Guid id, DateTime? olderThen);

        [Get("/Api/Feed/GetCompanyFeed?olderThen={olderThen}")]
        Task<ApiResponse<List<ApiWhitelabelFeedPostModel>, ApiRelatedItemsModel>> GetCompanyFeed(Guid id, [AliasAs("olderThen")] string olderThen);

        [Post("/Api/Feed/ReportPost")]
        Task<ApiResponse> ReportPost(Guid id);

        [Post("/Api/Feed/ReportComment")]
        Task<ApiResponse> ReportComment(Guid id, Guid commentId);

        //[Post("/Api/Feed/MovePostAsync")]
        //Task<ApiResponse<ApiWhitelabelFeedPostModel>> MovePostAsync(Guid id, Guid newParentId);

        [Post("/Api/Feed/CreateTextPostAsync")]
        Task<ApiResponse<ApiWhitelabelFeedPostModel>> CreateTextPostAsync(string text, Guid? parentId, List<Guid> mentionedUserIds, ParentType parentType = ParentType.Unknown);

        [Multipart]
        [Post("/Api/Feed/CreateImagePostAsync")]
        Task<ApiResponse<ApiWhitelabelFeedPostModel>> CreateImagePostAsync(string text, IEnumerable<StreamPart> files, Guid? parentId);

        [Post("/Api/Feed/LikePostAsync")]
        Task<ApiResponse> LikePostAsync(Guid id, bool likeStatus);

        [Post("/Api/Feed/FollowPostAsync")]
        Task<ApiResponse> FollowPostAsync(Guid id, bool followStatus);

        //[Post("/Api/Feed/PushPostAsync")]
        //Task<ApiResponse> StickyPostAsync(Guid id, bool isSticky);

        //[Post("/Api/Feed/PushPostAsync")]
        //Task<ApiResponse> PushPostAsync(Guid id, string text);

        [Post("/Api/Feed/CommentAsync")]
        Task<ApiResponse> CommentAsync(Guid id, string comment, List<Guid> mentionedUserIds);

        //[Post("/Api/Feed/DeleteCommentAsync")]
        //Task<ApiResponse> DeleteCommentAsync(Guid id, Guid commentId);

        //[Post("/Api/Feed/DeleteAsync")]
        //Task<ApiResponse> DeleteAsync(Guid id);
    }
}