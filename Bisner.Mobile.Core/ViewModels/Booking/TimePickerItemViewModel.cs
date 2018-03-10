using Bisner.Mobile.Core.Models.Booking;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class TimePickerItemViewModel : MvxViewModel
    {
        public TimePickerItemViewModel(TimePickerData data)
        {
            TimeString = data.TimeString;
            IsSelected = data.IsSelected;
        }

        public string TimeString
        {
            get => _timeString;
            set { _timeString = value; RaisePropertyChanged(() => TimeString); }
        }

        private bool _isSelected;
        private string _timeString;

        public bool IsSelected
        {
            get => _isSelected;

            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
    }
}
