using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface IEventService
    {
        Task<ApiWhitelabelEventModel> GetAsync(ApiPriority priority, Guid eventId);

        Task<List<ApiWhitelabelEventModel>> GetAllAsync(ApiPriority priority, Guid? categoryId = null, bool futureOnly = false);

        Task<List<ApiWhitelabelEventModel>> GetUpcomingAsync(ApiPriority priority, int number);
        Task<ApiWhitelabelEventModel> AttendEventAsync(Guid eventId, bool objIsAttending);
        Task<ApiWhitelabelEventModel> CommentEventAsync(Guid eventId, string input, List<Guid> mentionedUsers = null);
    }
}