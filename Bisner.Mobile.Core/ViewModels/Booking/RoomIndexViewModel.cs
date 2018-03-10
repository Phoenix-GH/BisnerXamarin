using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class RoomIndexViewModel : BaseRefreshViewModel
    {
        #region Constructor

        private readonly IBookingService _bookingService;

        public RoomIndexViewModel(IPlatformService platformService, IBookingService bookingService) : base(platformService)
        {
            _bookingService = bookingService;

            RoomList = new ObservableCollection<RoomIndexItemViewModel>();
            OverViewCommand = new MvxCommand(() => ShowViewModel<RoomTimeIndexViewModel>());
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            await InvokeRefreshAsync();
        }

        #endregion Init

        #region Rooms

        private async Task GetRoomsAsync()
        {
            try
            {
                var roomModels = await _bookingService.GetAllRoomsAsync(ApiPriority.UserInitiated);
                var reservations = await _bookingService.GetAllReservationsForDayAsync(ApiPriority.UserInitiated, DateTime.Now.Year, DateTime.Now.DayOfYear);

                var roomViewModels = new List<RoomIndexItemViewModel>();

                roomModels = roomModels.OrderBy(r => r.Order).ToList();

                foreach (var apiRoomModel in roomModels)
                {
                    // Get the room image
                    var headerUrl = apiRoomModel.Header != null
                        ? apiRoomModel.Header.Medium
                        : Defaults.RoomHeaderDefault;

                    var currentReservations = reservations.Where(r => r.RoomId == apiRoomModel.Id && r.ReservationStart < DateTime.Now && r.ReservationEnd > DateTime.Now).ToList();

                    var available = !currentReservations.Any();

                    roomViewModels.Add(new RoomIndexItemViewModel(new RoomData(apiRoomModel.Id, headerUrl, apiRoomModel.Name, available)));
                }

                RoomList = new ObservableCollection<RoomIndexItemViewModel>(roomViewModels);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private ObservableCollection<RoomIndexItemViewModel> _roomList;
        public ObservableCollection<RoomIndexItemViewModel> RoomList
        {
            get => _roomList;
            private set { _roomList = value; RaisePropertyChanged(() => RoomList); }
        }

        public ICommand OverViewCommand { get; }

        #endregion Rooms

        #region Refresh

        protected override async Task RefreshAsync()
        {
            await GetRoomsAsync();
        }

        #endregion Refresh
    }
}
