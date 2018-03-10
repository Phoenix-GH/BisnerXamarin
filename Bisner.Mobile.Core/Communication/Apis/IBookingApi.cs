using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Booking;
using Bisner.ApiModels.Central;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    public class ReservationsResponseModel
    {
        public List<ApiRoomModel> Rooms { get; set; }

        public List<RoomReservationModel> Reservations { get; set; }
    }

    public class ReservationResponseModel
    {
        public List<ApiRoomModel> Rooms { get; set; }

        public RoomReservationModel Reservation { get; set; }
     }

    public class ReserveRoomResponseModel
    {
        public RoomReservationModel Reservation { get; set; }

        public bool ShowPayment { get; set; }

        public bool ShowConfirmation { get; set; }

        public bool ShowLogin { get; set; }
    }

    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IBookingApi
    {
        [Get("/Api/Platform/Get")]
        Task<ApiResponse<ApiCentralPublicPlatformModel>> GetPlatform();

        [Get("/Api/Booking/Room/GetAll")]
        Task<ApiResponse<List<ApiRoomModel>>> GetRooms();

        [Get("/Api/Booking/Room/Get")]
        Task<ApiResponse<ApiRoomModel>> GetRoom(Guid id);

        [Get("/Api/Booking/Room/CheckReservations")]
        Task<ApiResponse<List<RoomReservationModel>>> CheckReservations(Guid id, int year, int dayOfYear);

        /// <summary>
        /// Get all user reservations upcomming and in the past
        /// </summary>
        /// <returns></returns>
        [Get("/Api/Booking/Reservation/GetAll")]
        Task<ApiResponse<ReservationsResponseModel>> GetAll();

        /// <summary>
        /// Get all the reservations for a specific day of a year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="dayOfYear"></param>
        /// <returns></returns>
        [Get("/Api/Booking/Reservation/GetAllForDay")]
        Task<ApiResponse<List<RoomReservationModel>>> GetAllForDay(int year, int dayOfYear);

        /// <summary>
        /// Get a single reservation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Get("/Api/Booking/Reservation/Get")]
        Task<ApiResponse<ReservationResponseModel>> Get(Guid id);

        /// <summary>
        /// Check room reservation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <param name="dayOfYear"></param>
        /// <returns></returns>
        [Get("/Api/Booking/Reservation/CheckReservations")]
        Task<ApiResponse<List<RoomReservationModel>>> CheckReservations2(Guid id, int year, int dayOfYear);

        /// <summary>
        /// Update a reservation details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="message"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        [Post("/Api/Booking/Reservation/UpdateReservationAsync")]
        Task<ApiResponse<RoomReservationModel>> UpdateReservationAsync(Guid id, string title, string description, string message, bool isPrivate);

        /// <summary>
        /// Delete a reservation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Post("/Api/Booking/Reservation/DeleteReservation")]
        Task<ApiResponse> DeleteReservation(Guid id);

        /// <summary>
        /// make a reservation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="numberOfPersons"></param>
        /// <returns></returns>
        [Post("/Api/Booking/Reservation/ReserveRoom")]
        Task<ApiResponse<ReserveRoomResponseModel>> ReserveRoom(Guid id, DateTime start, DateTime end, int numberOfPersons);
    }
}