using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface INotificationApi
    {
        [Get("/Api/Notification/Get")]
        Task<ApiResponse<ApiWhitelabelNotificationModel, ApiRelatedItemsModel>> Get(Guid id);

        [Get("/Api/Notification/GetAll")]
        Task<ApiResponse<List<ApiWhitelabelNotificationModel>, ApiRelatedItemsModel>> GetAll();

        [Post("/Api/Notification/SetReadAsync")]
        Task<ApiResponse> SetReadAsync(Guid id, bool status);

        [Post("/Api/Notification/SetAllReadAsync")]
        Task<ApiResponse<List<ApiWhitelabelNotificationModel>>> SetAllReadAsync(bool status);

        [Post("/Api/Notification/DeleteAsync")]
        Task<ApiResponse> DeleteAsync(Guid id);

        [Post("/Api/Notification/DeleteAllAsync")]
        Task<ApiResponse> DeleteAllAsync();

        [Post("/Api/Notification/DeleteAllReadAsync")]
        Task<ApiResponse> DeleteAllReadAsync();

        [Post("/Api/Notification/DeleteAllUnReadAsync")]
        Task<ApiResponse> DeleteAllUnReadAsync();

    }
}