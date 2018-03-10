using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Booking;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Plugin.Connectivity;
using Polly;

namespace Bisner.Mobile.Core.Service
{
    public class BookingService : IBookingService
    {
        #region Constructor

        private const string RoomsKey = "rooms";

        private readonly IApiService<IBookingApi> _bookingApiService;

        public BookingService(IApiService<IBookingApi> bookingApiService)
        {
            _bookingApiService = bookingApiService;
        }

        #endregion Constructor

        #region Get

        public async Task<ApiRoomModel> GetRoomAsync(ApiPriority priority, Guid roomId)
        {
            var cachedRoom = await GetAllRoomsAsync(priority);

            var roomModel = cachedRoom.FirstOrDefault(r => r.Id == roomId);

            return roomModel;
        }

        public async Task<List<ApiRoomModel>> GetAllRoomsAsync(ApiPriority priority)
        {
            var cachedRooms = BlobCache.LocalMachine.GetOrFetchObject(RoomsKey, () => GetAllRoomsRemoteAsync(priority), DateTimeOffset.Now.AddHours(1));

            var roomModels = await cachedRooms.FirstOrDefaultAsync();

            return roomModels;
        }

        private async Task<List<ApiRoomModel>> GetAllRoomsRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _bookingApiService.GetApi(priority);

            var response = await api.GetRooms();

            return response.Data;
        }

        public async Task<List<RoomReservationModel>> GetReservationsAsync(ApiPriority priority, Guid roomId, bool remote = false, DateTime? date = null)
        {
            var cacheKey = $"reservations-{roomId}{date?.Year}{date?.DayOfYear}";

            // Clear cache when remote = true
            if (remote)
                await BlobCache.LocalMachine.Invalidate(cacheKey);

            // 10 minute cache
            var cachedReservations = BlobCache.LocalMachine.GetOrFetchObject(cacheKey, () => GetRemoteReservationsAsync(priority, roomId, date), DateTimeOffset.Now.AddMinutes(10));

            var reservations = await cachedReservations.FirstOrDefaultAsync();

            return reservations;
        }

        private async Task<List<RoomReservationModel>> GetRemoteReservationsAsync(ApiPriority priority, Guid roomId, DateTime? date = null)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _bookingApiService.GetApi(priority);

            var reservations = await api.CheckReservations(roomId, date?.Year ?? DateTime.Now.Year, date?.DayOfYear ?? DateTime.Now.DayOfYear);

            return reservations.Data;
        }

        public async Task<ServiceReservationsResponseModel> GetMyReservationsAsync()
        {
            var cachedReservations = BlobCache.LocalMachine.GetOrFetchObject("myreservations", GetMyReservationsRemoteAsync, DateTimeOffset.Now.AddSeconds(1));

            var reservationResponseModel = await cachedReservations.FirstOrDefaultAsync();

            return new ServiceReservationsResponseModel
            {
                Reservations = reservationResponseModel.Reservations,
                Rooms = reservationResponseModel.Rooms
            };
        }

        private async Task<ReservationsResponseModel> GetMyReservationsRemoteAsync()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _bookingApiService.UserInitiated;

            var myReservations = await api.GetAll();

            return myReservations.Data;
        }

        public async Task<List<RoomReservationModel>> GetAllReservationsForDayAsync(ApiPriority priority, int year, int dayOfYear)
        {
            var cachedReservations = BlobCache.LocalMachine.GetOrFetchObject($"allreservations-{year}-{dayOfYear}", () => GetAllReservationsForDayRemoteAsync(priority, year, dayOfYear), DateTimeOffset.Now.AddSeconds(10));

            var reservationResponseModel = await cachedReservations.FirstOrDefaultAsync();

            return reservationResponseModel;
        }

        public async Task<ApiResponse<ReserveRoomResponseModel>> ReserveRoomAsync(Guid roomId, DateTime start, DateTime end, int numberOfPersons)
        {
            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await _bookingApiService.UserInitiated.ReserveRoom(roomId, start, end, numberOfPersons));

            return response;
        }

        private async Task<List<RoomReservationModel>> GetAllReservationsForDayRemoteAsync(ApiPriority priority, int year, int dayOfYear)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _bookingApiService.GetApi(priority);

            var myReservations = await api.GetAllForDay(year, dayOfYear);

            return myReservations.Data;
        }

        #endregion Get

        #region Update

        public async Task<ApiResponse<RoomReservationModel>> UpdateReservationAsync(Guid id, string title, string description, string message, bool isPrivate)
        {
            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await _bookingApiService.UserInitiated.UpdateReservationAsync(id, title, description, message, isPrivate));

            return response;
        }

        public async Task<ApiResponse> CancelBookingAsync(Guid bookingId)
        {
            var response = await Policy.Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 2,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await _bookingApiService.UserInitiated.DeleteReservation(bookingId));

            return response;
        }

        #endregion Update
    }

    public class ServiceReservationsResponseModel
    {
        public List<RoomReservationModel> Reservations { get; set; }

        public List<ApiRoomModel> Rooms { get; set; }
    }
}