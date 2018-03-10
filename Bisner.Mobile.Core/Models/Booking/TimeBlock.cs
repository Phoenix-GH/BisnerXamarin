namespace Bisner.Mobile.Core.Models.Booking
{
    public class TimeBlock
    {
        public TimeBlock(TimePickerData startTime, TimePickerData endTime, string reservedByName = null)
        {
            StartTime = startTime;
            EndTime = endTime;
            ReservedByName = reservedByName;
        }

        public TimePickerData StartTime
        {
            get;
            set;
        }

        public TimePickerData EndTime
        {
            get;
            set;
        }

        public string ReservedByName { get; set; }
    }
}