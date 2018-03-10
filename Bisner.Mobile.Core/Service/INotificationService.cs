using System;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface INotificationService
    {
        Task<NotificationResponseModel> GetAllAsync(ApiPriority priority);
        Task<int> GetNumberUnreadAsync();
        Task<bool> SetIsReadAsync(Guid id, bool isRead);
        Task<bool> SetAllReadAsync();
    }
}