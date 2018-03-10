using System;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class DatePickerViewModel : BaseViewModel
    {
        #region Constructor

        private Guid _roomId;

        public DatePickerViewModel(IPlatformService platformService) : base(platformService)
        {
            _dateTime = DateTime.MinValue;
            ContinueCommand = new MvxCommand(Continue, () => _dateTime != DateTime.MinValue);
        }

        #endregion Constructor

        #region Init

        public void Init(Guid roomId)
        {
            _roomId = roomId;
        }

        #endregion Init

        #region Logic

        private DateTime _dateTime;

        public void SelectDate(DateTime dateTime)
        {
            _dateTime = dateTime;
            ContinueCommand.RaiseCanExecuteChanged();
        }

        public MvxCommand ContinueCommand { get; }

        private void Continue()
        {
            if (_dateTime.Date < DateTime.Now.Date)
            {
                // Date is in the past
                UserDialogs.Alert("Please select a date in the future");
            }
            else
            {
                ShowViewModel<TimeSelectViewModel>(new {roomId = _roomId, dateTime = _dateTime});
            }
        }

        #endregion Logic
    }
}
