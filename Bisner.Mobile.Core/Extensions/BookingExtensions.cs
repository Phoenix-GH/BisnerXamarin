using System;
using Bisner.ApiModels.Booking;
using Bisner.Mobile.Core.ViewModels.Booking;

namespace Bisner.Mobile.Core.Extensions
{
    public static class BookingExtensions
    {
        public static TimeBlockType ToTimeBlockType(this RoomTimeUnit roomTimeUnit)
        {
            switch (roomTimeUnit)
            {
                case RoomTimeUnit.Hour:
                    return TimeBlockType.SIXTY;
                case RoomTimeUnit.Minutes15:
                    return TimeBlockType.FIFTEEN;
                case RoomTimeUnit.Minutes30:
                    return TimeBlockType.THIRTY;
                //case RoomTimeUnit.Hour2:
                //    break;
                //case RoomTimeUnit.Hour4:
                //    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roomTimeUnit), roomTimeUnit, null);
            }
        }
    }
}
