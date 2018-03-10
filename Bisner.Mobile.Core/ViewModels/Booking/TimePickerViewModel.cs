using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class TimePickerViewModel : BaseViewModel
    {
        #region Constructor

        private readonly IBookingService _bookingService;

        public TimePickerViewModel(IPlatformService platformService, IBookingService bookingService) : base(platformService)
        {
            _bookingService = bookingService;

            BackBtnClickedCommand = new MvxCommand(Back);
            CloseBtnClickedCommand = new MvxCommand(Close);
            BookBtnClickedCommand = new MvxCommand(Book, CanBook);
        }

        #endregion Constructor

        #region Init

        public async Task Init(Guid roomId, DateTime dateTime)
        {
            try
            {
                var timeList = new List<TimePickerData>();
                for (var i = 9; i < 24; i++)
                {
                    var hour = ((i % 12) == 0) ? 12 : (i % 12);
                    var timeItem = new TimePickerData(i, 0, false);
                    timeList.Add(timeItem);
                }

                var viewModelList = new List<TimePickerItemViewModel>();

                foreach (var timePickerData in _availableTimeList)
                {
                    viewModelList.Add(new TimePickerItemViewModel(timePickerData));
                }

                TimeList = new ObservableCollection<TimePickerItemViewModel>(viewModelList);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Init

        #region Commands


        public ICommand BackBtnClickedCommand { get; }

        public ICommand CloseBtnClickedCommand { get; }

        public ICommand BookBtnClickedCommand { get; }

        #endregion Commands

        /// <summary>
        /// The available hours to book for this day
        /// </summary>
        private readonly List<TimePickerData> _availableTimeList = new List<TimePickerData>();

        private ObservableCollection<TimePickerItemViewModel> _timeList;
        /// <summary>
        /// The list of viewmodels for selectable times
        /// </summary>
        public ObservableCollection<TimePickerItemViewModel> TimeList
        {
            get => _timeList;
            set { _timeList = value; RaisePropertyChanged(() => TimeList); }
        }

        public void SelectItem(int index)
        {
            var item = _availableTimeList[index];
            item.IsSelected = !item.IsSelected;
            TimeList[index].IsSelected = !TimeList[index].IsSelected;
        }

        private void Back()
        {
            Close(this);
        }

        private void Close()
        {
            //TODO
        }

        private void Book()
        {
            ShowViewModel<BookingConfirmedViewModel>();
        }

        private bool CanBook()
        {
            return true;
        }
    }
}
