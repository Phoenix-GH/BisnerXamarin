using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Booking;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Refit;

namespace Bisner.Mobile.Core.Service
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IBookingService
    {
        Task<ApiRoomModel> GetRoomAsync(ApiPriority priority, Guid roomId);

        Task<List<ApiRoomModel>> GetAllRoomsAsync(ApiPriority priority);

        Task<List<RoomReservationModel>> GetReservationsAsync(ApiPriority priority, Guid roomId, bool remote = false, DateTime? date = null);

        Task<ServiceReservationsResponseModel> GetMyReservationsAsync();

        Task<List<RoomReservationModel>> GetAllReservationsForDayAsync(ApiPriority priority, int year, int dayOfYear);

        Task<ApiResponse<ReserveRoomResponseModel>> ReserveRoomAsync(Guid roomId, DateTime start, DateTime end, int numberOfPersons);

        Task<ApiResponse<RoomReservationModel>> UpdateReservationAsync(Guid id, string title, string description, string message, bool isPrivate);

        Task<ApiResponse> CancelBookingAsync(Guid bookingId);
    }
}