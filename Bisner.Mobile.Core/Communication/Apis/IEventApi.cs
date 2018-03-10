using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication.Models;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IEventApi
    {
        [Get("/Api/Event/GetAllCategories")]
        Task<ApiResponse<List<ApiCentralEventCategoryModel>>> GetAllCategories();

        [Get("/Api/Event/GetAll")]
        Task<ApiResponse<List<ApiWhitelabelEventModel>, EventCategoriesAndTypesModel>> GetAll(Guid? id, bool futureOnly = false);

        [Get("/Api/Event/GetUpcoming")]
        Task<ApiResponse<List<ApiWhitelabelEventModel>, EventCategoriesAndTypesModel>> GetUpcomming(int amount);

        [Get("/Api/Event/GetAllForCategory")]
        Task<ApiResponse<List<ApiWhitelabelEventModel>, EventCategoriesAndTypesModel>> GetAllForCategory(Guid id, bool futureOnly = false);

        [Get("/Api/Event/Get")]
        Task<ApiResponse<ApiWhitelabelEventModel, EventCategoryAndTypeModel>> GetEvent(Guid id);

        [Get("/Api/Event/GetDownloadUrl")]
        Task<ApiResponse<string>> GetDownloadUrl(Guid id, Guid itemId);

        //[Get("/Api/Event/ExportAttendees")]
        //Task<ApiResponse<>> ExportAttendees(Guid id);

        [Post("/Api/Event/CreateAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> CreateAsync(string title, string description, Guid categoryId, Guid typeId, DateTime? start, DateTime? end, bool isPrivate = false);

        [Post("/Api/Event/EditAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> EditAsync(EditEventApiModel editModel);

        [Post("/Api/Event/AddSpeakerAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> AddSpeakerAsync(Guid id, EventSpeakerModel model);

        [Post("/Api/Event/DeleteSpeakerAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> DeleteSpeakerAsync(Guid id, Guid speakerId);

        [Post("/Api/Event/UpdateSpeakerAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> UpdateSpeakerAsync(Guid id, Guid speakerId, EventSpeakerModel model);

        [Post("/Api/Event/AddCategory")]
        Task<ApiResponse<ApiWhitelabelEventModel>> AddCategory(Guid id, Guid categoryId);

        [Post("/Api/Event/RemoveCategory")]
        Task<ApiResponse<ApiWhitelabelEventModel>> RemoveCategory(Guid id, Guid categoryId);

        [Post("/Api/Event/AddType")]
        Task<ApiResponse<ApiWhitelabelEventModel>> AddType(Guid id, Guid typeId);

        [Post("/Api/Event/RemoveType")]
        Task<ApiResponse<ApiWhitelabelEventModel>> RemoveType(Guid id, Guid typeId);

        [Post("/Api/Event/InviteUsersAsync")]
        Task<ApiResponse> InviteUsersAsync(Guid id, List<Guid> userIds);

        [Post("/Api/Event/ChangeLogoAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> ChangeLogoAsync(Guid id, [AttachmentName("data")] Stream data);

        [Post("/Api/Event/AddSpeakerAvatar")]
        Task<ApiResponse> AddSpeakerAvatar(Guid id, Guid speakerId, [AttachmentName("data")] Stream data);

        [Post("/Api/Event/ChangeHeaderAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> ChangeHeaderAsync(Guid id, [AttachmentName("data")] Stream data);

        [Post("/Api/Event/ChangeThumbnailAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> ChangeThumbnailAsync(Guid id, [AttachmentName("data")] Stream data);

        [Post("/Api/Event/ChangeIndexHeaderAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> ChangeIndexHeaderAsync(Guid id);

        [Post("/Api/Event/AddImagesAsync")]
        [Obsolete("Unsure about ienumerable string")]
        Task<ApiResponse<ApiWhitelabelEventModel>> AddImagesAsync(Guid id, IEnumerable<string> files);

        [Post("/Api/Event/AddFilesAsync")]
        [Obsolete("Unsure about ienumerable string")]
        Task<ApiResponse<ApiWhitelabelEventModel>> AddFilesAsync(Guid id, IEnumerable<string> files);

        [Post("/Api/Event/DeleteImageAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> DeleteImageAsync(Guid id, Guid imageId);

        [Post("/Api/Event/DelteFileAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> DeleteFileAsync(Guid id, Guid fileId);

        [Post("/Api/Event/CommentAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> CommentAsync(Guid id, string comment, List<Guid> mentionedUserIds);

        [Post("/Api/Event/DeleteCommentAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> DeleteCommentAsync(Guid id, Guid commentId);

        [Post("/Api/Event/DeleteAsync")]
        Task<ApiResponse> DeleteAsync(Guid id);

        [Post("/Api/Event/PublishAsync")]
        Task<ApiResponse<bool>> PublishAsync(Guid id, bool newStatus, string text);

        [Post("/Api/Event/PublishV2Async")]
        Task<ApiResponse<bool>> PublishV2Async(Guid id, bool newStatus);

        [Post("/Api/Event/CreateFeedPostAsync")]
        Task<ApiResponse<bool>> CreateFeedPostAsync(Guid id, string text);

        [Post("/Api/Event/SendEmailAsync")]
        Task<ApiResponse<bool>> SendEmailAsync(Guid id, string text, string title);

        [Post("/Api/Event/AttendAsync")]
        Task<ApiResponse<ApiWhitelabelEventModel>> AttendAsync(Guid id, bool newStatus);

        [Get("/Api/Event/CanCreateEvent")]
        Task<ApiResponse<bool>>  CanCreateEvent();

        [Post("/Api/Event/PushEventAsync")]
        Task<ApiResponse> PushEventAsync(Guid id, string text);
    }
}
