using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class DatePickerItemViewModel : MvxViewModel
    {
        #region Constructor

        public DatePickerItemViewModel()
        {
        }

        #endregion Constructor

        #region Properties

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; RaisePropertyChanged(() => IsSelected); }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private int _day;
        public int Day
        {
            get => _day;
            set { _day = value; RaisePropertyChanged(() => Day); }
        }

        #endregion Properties
    }
}
