using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Bisner.ApiModels.Booking;
using Bisner.ApiModels.Whitelabel;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class TimeSelectViewModel : BaseViewModel
    {
        #region Constructor

        private Guid _roomId;
        private DateTime _dateTime;
        private List<ApiWhitelabelPublicUserModel> _allMembers;

        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        public TimeSelectViewModel(IPlatformService platformService, IBookingService bookingService, IUserService userService) : base(platformService)
        {
            _bookingService = bookingService;
            _userService = userService;

            BackBtnClickedCommand = new MvxCommand(Back);
            BookBtnClickedCommand = new MvxAsyncCommand(BookAsync);
        }

        #endregion Constructor

        #region Init

        public async Task Init(Guid roomId, DateTime dateTime)
        {
            _roomId = roomId;
            _dateTime = dateTime;

            try
            {
                var roomModel = await _bookingService.GetRoomAsync(ApiPriority.UserInitiated, roomId);
                var roomReservationsForToday = await _bookingService.GetAllReservationsForDayAsync(ApiPriority.UserInitiated, _dateTime.Year, _dateTime.DayOfYear);
                roomReservationsForToday = roomReservationsForToday.Where(r => r.RoomId == roomId).ToList();
                _allMembers = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);

                var startHour = 6;
                var endHour = 24;

                if (roomModel?.OpeningInformation != null)
                {
                    // Check opening hours
                    var openingDay = GetOpeningInfo(roomModel.OpeningInformation, dateTime.DayOfWeek);

                    startHour = openingDay.HourOpen;
                    endHour = openingDay.HourClose;
                }

                //if (dateTime.Year == DateTime.Now.Year && dateTime.DayOfYear == DateTime.Now.DayOfYear)
                //{
                //    // Is today check current hour
                //    if (DateTime.Now.Hour > startHour)
                //    {
                //        // Later then opening hour
                //        startHour = DateTime.Now.Hour;
                //    }
                //    else if (DateTime.Now.Hour > endHour)
                //    {
                //        // Already closed
                //        startHour = 24;
                //    }

                //    // Remove reservations that have already passed
                //    roomReservationsForToday = roomReservationsForToday.Where(r => r.ReservationEnd.Hour < startHour).ToList();

                //    //foreach (var roomReservationModel in roomReservationsForToday.wh)
                //    //{
                //    //    // Change start date to start of reservation possibilities
                //    //    roomReservationModel.ReservationStart = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, startHour, 0, 0);
                //    //}
                //}

                if (startHour != 24)
                {
                    var startTime = new TimePickerData(startHour, 0);
                    var endTime = new TimePickerData(endHour, 0);
                    StartEndTime = new TimeBlock(startTime, endTime);

                    UpdateReservations(roomReservationsForToday);
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private void UpdateReservations(List<RoomReservationModel> reservations)
        {
            var alreadyReservedList = new List<TimeBlock>();

            foreach (var roomReservationModel in reservations)
            {
                string reservationName;

                if (roomReservationModel.IsPrivate)
                {
                    reservationName = "Private meeting";
                }
                else if (string.IsNullOrWhiteSpace(roomReservationModel.Title))
                {
                    var user = _allMembers.FirstOrDefault(m => m.Id == roomReservationModel.UserId);

                    reservationName = user != null ? $"Reserved by {user.DisplayName}" : "Reserved by Unknown Member";
                }
                else
                {
                    reservationName = roomReservationModel.Title;
                }

                alreadyReservedList.Add(new TimeBlock(new TimePickerData(roomReservationModel.ReservationStart.Hour, roomReservationModel.ReservationStart.Minute), new TimePickerData(roomReservationModel.ReservationEnd.Hour, roomReservationModel.ReservationEnd.Minute), reservationName));
            }

            AlreadyReservedTimeList = alreadyReservedList;
        }

        private OpeningDay GetOpeningInfo(OpeningInformation roomModelOpeningInformation, DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Friday:
                    return roomModelOpeningInformation.Friday;
                case DayOfWeek.Monday:
                    return roomModelOpeningInformation.Monday;
                case DayOfWeek.Saturday:
                    return roomModelOpeningInformation.Saturday;
                case DayOfWeek.Sunday:
                    return roomModelOpeningInformation.Sunday;
                case DayOfWeek.Thursday:
                    return roomModelOpeningInformation.Thursday;
                case DayOfWeek.Tuesday:
                    return roomModelOpeningInformation.Tuesday;
                case DayOfWeek.Wednesday:
                    return roomModelOpeningInformation.Wednesday;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null);
            }
        }

        #endregion Init

        #region Commands

        public ICommand BackBtnClickedCommand { get; }

        public ICommand BookBtnClickedCommand { get; }

        #endregion Commands

        #region Properties

        private TimeBlockType _timeBlockType;
        public TimeBlockType TimeBlockType
        {
            get => _timeBlockType;
            set { _timeBlockType = value; RaisePropertyChanged(() => TimeBlockType); }
        }

        private TimeBlock _startEndTime;
        public TimeBlock StartEndTime
        {
            get => _startEndTime;
            set { _startEndTime = value; RaisePropertyChanged(() => StartEndTime); }
        }

        private List<TimeBlock> _alreadyReservedTimeList;
        public List<TimeBlock> AlreadyReservedTimeList
        {
            get => _alreadyReservedTimeList;
            set
            {
                _alreadyReservedTimeList = value;
                RaisePropertyChanged(() => AlreadyReservedTimeList);
            }
        }

        TimeBlock _newReservedTime;
        public TimeBlock NewReservedTime
        {
            get => _newReservedTime;
            set => _newReservedTime = value;
        }

        #endregion Properties

        #region Actions

        private void Back()
        {
            Close(this);
        }

        private async Task BookAsync()
        {
            string message = "";

            try
            {
                var startDateTime = new DateTime(_dateTime.Year, _dateTime.Month, _dateTime.Day,
                    NewReservedTime.StartTime.Hour, NewReservedTime.StartTime.Min, 0);
                var endDateTime = new DateTime(_dateTime.Year, _dateTime.Month, _dateTime.Day,
                    NewReservedTime.EndTime.Hour, NewReservedTime.EndTime.Min, 0);

                if (startDateTime < DateTime.Now)
                {
                    // Booking in the past
                    message = "You are trying to book this room in the past";
                }
                else
                {
                    var response = await _bookingService.ReserveRoomAsync(_roomId, startDateTime, endDateTime, 2);

                    if (response.Success && response.Data?.Reservation != null)
                    {
                        ShowViewModel<BookingConfirmedViewModel>(new { id = response.Data.Reservation.Id });
                        return;
                    }

                    switch (response.ErrorCode)
                    {
                        case ApiErrorCodes.RoomInUse:
                            message = "This room is not avaiable any more at the selected time, please refresh";
                            break;
                        case ApiErrorCodes.NoAccess:
                            message = "You are not allowed to book this room";
                            break;
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                message = "Date is out of range";
                ExceptionService.HandleException(ex);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ExceptionService.HandleException(ex);
            }

            await UserDialogs.AlertAsync(new AlertConfig
            {
                Title = "Error",
                Message = message,
                OkText = "OK"
            });
        }

        #endregion Actions
    }
}