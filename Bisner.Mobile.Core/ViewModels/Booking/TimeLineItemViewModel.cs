using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Booking;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class TimeLineItemViewModel : MvxViewModel
    {
        public TimeLineItemViewModel(TimePickerData data, TimeBlockType timeBlockType)
        {
            if (!Settings.AmPmNotation)
            {
                var min = data.Min == 0 ? "00" : data.Min.ToString();
                var hour = data.Hour;
                TimeString = hour + ":" + min;
                TimeBlockType = timeBlockType;
            }
            else
            {
                var min = (data.Min == 0) ? "00" : data.Min.ToString();
                var hour = ((data.Hour % 12) == 0) ? "12" : (data.Hour % 12).ToString();
                TimeString = hour + ":" + min;
                TimeBlockType = timeBlockType;
            }
        }

        #region property

        public string TimeString { get; set; }

        public TimeBlockType TimeBlockType { get; set; }

        #endregion
    }
}
