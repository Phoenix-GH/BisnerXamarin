using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Extensions;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class RoomTimeIndexViewModel : BaseRefreshViewModel
    {
        #region Constructor

        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        public RoomTimeIndexViewModel(IPlatformService platformService, IBookingService bookingService, IUserService userService) : base(platformService)
        {
            _bookingService = bookingService;
            _userService = userService;

            _selectedDate = DateTime.Now;

            ChangeDateCommand = new MvxAsyncCommand(ChangeDateAsync);
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            await InvokeRefreshAsync();
        }

        #endregion Init

        #region Items

        private async Task GetRoomsAsync()
        {
            try
            {
                // Get all rooms
                var allRooms = await _bookingService.GetAllRoomsAsync(ApiPriority.UserInitiated);

                // Get all reservations for today
                var allReservations = await _bookingService.GetAllReservationsForDayAsync(ApiPriority.UserInitiated, _selectedDate.Year, _selectedDate.DayOfYear);
                var allMembers = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);

                var roomTimeIndexList = new List<RoomTimeIndexItemViewModel>();

                foreach (var apiRoomModel in allRooms)
                {
                    // Get the room image
                    var headerUrl = apiRoomModel.Header != null
                        ? apiRoomModel.Header.Medium
                        : Defaults.RoomHeaderDefault;

                    var reservationTimeBlocks = new List<TimeBlock>();

                    // Create reservation timeblocks for the timeline
                    foreach (var roomReservationModel in allReservations.Where(r => r.RoomId == apiRoomModel.Id))
                    {
                        string reservationName;

                        if (roomReservationModel.IsPrivate)
                        {
                            reservationName = "Private meeting";
                        }
                        else if (string.IsNullOrWhiteSpace(roomReservationModel.Title))
                        {
                            var user = allMembers.FirstOrDefault(m => m.Id == roomReservationModel.UserId);

                            reservationName = user != null ? $"Reserved by {user.DisplayName}" : "Reserved by Unknown Member";
                        }
                        else
                        {
                            reservationName = roomReservationModel.Title;
                        }
                        
                        reservationTimeBlocks.Add(
                            new TimeBlock(
                                new TimePickerData(roomReservationModel.ReservationStart.Hour,
                                    roomReservationModel.ReservationStart.Minute),
                                new TimePickerData(roomReservationModel.ReservationEnd.Hour,
                                    roomReservationModel.ReservationEnd.Minute), reservationName));
                    }

                    var currentReservations = allReservations.Where(r => r.RoomId == apiRoomModel.Id && r.ReservationStart < DateTime.Now && r.ReservationEnd > DateTime.Now).ToList();

                    var available = !currentReservations.Any();

                    var roomData = new RoomData(apiRoomModel.Id, headerUrl, apiRoomModel.Name, available, apiRoomModel.TimeUnit.ToTimeBlockType(), reservationTimeBlocks);

                    roomTimeIndexList.Add(new RoomTimeIndexItemViewModel(roomData));
                }

                RoomTimeIndexList = new ObservableCollection<RoomTimeIndexItemViewModel>(roomTimeIndexList);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private ObservableCollection<RoomTimeIndexItemViewModel> _roomTimeIndexList;
        public ObservableCollection<RoomTimeIndexItemViewModel> RoomTimeIndexList
        {
            get => _roomTimeIndexList;
            private set { _roomTimeIndexList = value; RaisePropertyChanged(() => RoomTimeIndexList); }
        }

        #endregion Items

        #region Date

        private DateTime _selectedDate;

        public ICommand ChangeDateCommand { get; }

        private async Task ChangeDateAsync()
        {
            var result = await UserDialogs.DatePromptAsync(new DatePromptConfig
            {
                Title = "Select a date",
                CancelText = "Cancel",
                SelectedDate = _selectedDate,
                IsCancellable = true,
            });

            try
            {
                _selectedDate = result.SelectedDate;

                RefreshCommand.Execute(null);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Date

        #region Refresh

        protected override async Task RefreshAsync()
        {
            await GetRoomsAsync();
        }

        #endregion Refresh
    }
}
