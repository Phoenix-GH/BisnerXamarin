using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Booking;

namespace Bisner.Mobile.Core.Models.Booking
{
    public class RoomData
    {
        public RoomData(Guid roomId, string imageUrl, string title, bool isAvailable, TimeBlockType timeBlockType, List<TimeBlock> blockTimeList) : this(roomId, imageUrl, title, isAvailable)
        {
            TimeBlockType = timeBlockType;
            BlockTimeList = blockTimeList;
            StartEndTime = new TimeBlock(new TimePickerData(6, 0), new TimePickerData(24, 0));

            //ReservedTimeList = new List<TimePickerData>();
            //for (var i = 9; i < 24; i++)
            //{
            //	var hour = ((i % 12) == 0) ? 12 : (i % 12);
            //	var timeItem = new TimePickerData(i, 0, false);
            //	ReservedTimeList.Add(timeItem);
            //}
        }

        public RoomData(Guid roomId, string imageUrl, string title, bool isAvailable)
        {
            RoomId = roomId;
            ImageUrl = imageUrl;
            Title = title;
            IsAvailable = isAvailable;
            //ReservedTimeList = _reservedTimeList;
        }

        #region property

        public Guid RoomId { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        public TimeBlockType TimeBlockType { get; set; }

        public TimeBlock StartEndTime { get; set; }
        public List<TimeBlock> BlockTimeList { get; set; }

        public TimePickerData BlockStartTime { get; set; }
        public TimePickerData BlockEndTime { get; set; }
        //public List<TimePickerData> ReservedTimeList { get; set; }

        #endregion


    }
}