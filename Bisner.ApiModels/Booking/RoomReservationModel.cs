using System;

namespace Bisner.ApiModels.Booking
{
    public enum RoomReservationStatus
    {
        /// <summary>
        /// Free to book
        /// </summary>
        Free = 0,

        /// <summary>
        /// Pending is temp for 5 minutes so we don't have 2 people booking same room at the same time
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Room is reserverd but not yet paid for
        /// </summary>
        Reserved = 2,

        /// <summary>
        /// Room is reserved and paid for 
        /// </summary>
        Booked = 3
    }

    public class RoomReservationModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Room status
        /// </summary>
        public RoomReservationStatus Status { get; set; }

        /// <summary>
        /// Entity first created
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Message to a space
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Admin only message for internal use
        /// </summary>
        public string AdminMessage { get; set; }

        /// <summary>
        /// Reservation details are private
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Entity last updated
        /// </summary>
        public DateTime LastUpdateDateTime { get; set; }

        /// <summary>
        /// Start time of the room reservation
        /// </summary>
        public DateTime ReservationStart { get; set; }

        /// <summary>
        /// End time of the room reservation
        /// </summary>
        public DateTime ReservationEnd { get; set; }

        /// <summary>
        /// Reserved by
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Room ID
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Connected payment id
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// Number of guests
        /// </summary>
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Unique reservation code
        /// </summary>
        public string ReservationCode { get; set; }
    }
}