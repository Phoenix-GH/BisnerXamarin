using System;

namespace Bisner.Mobile.Core.Models.Booking
{
    public class TimePickerData
    {
        public TimePickerData(DateTime _time, bool _isSelected)
        {
            //TimeString = _timeString;
            IsSelected = _isSelected;
            Time = _time;
        }

        public TimePickerData(int _hour, int _min, bool _isSelected)
        {
            Hour = _hour;
            Min = _min;
            IsSelected = _isSelected;
        }

        public TimePickerData(int _hour, int _min)
        {
            Hour = _hour;
            Min = _min;
        }

        public void SetTime(int _hour, int _min)
        {
            Hour = _hour;
            Min = _min;
        }

        public string TimeString { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public bool IsSelected { get; set; }
        public DateTime Time { get; set; }

    }
}
