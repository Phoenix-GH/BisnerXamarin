using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Notifications;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class JobboardViewModel : BaseRefreshViewModel
    {
        #region Constructor

        private readonly IBookingService _bookingService;

        public JobboardViewModel(IPlatformService platformService, IBookingService bookingService) : base(platformService)
        {
            _bookingService = bookingService;

            ItemSelectedCommand = new MvxCommand<BookingsItemViewModel>(ItemSelected);
            BookingCommand = new MvxCommand(ShowBooking);
        }

        #endregion Constructor

        #region Init

        public MvxSubscriptionToken MessageSubscription;

        public async Task Init()
        {
            MessageSubscription = Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<UpdateRervationsMessage>(async message =>
            {
                await InvokeRefreshAsync();
            });

            await InvokeRefreshAsync();
        }

        #endregion Init

        #region Items

        private ObservableCollection<BookingsItemViewModel> _bookingsList;
        public ObservableCollection<BookingsItemViewModel> BookingsList
        {
            get => _bookingsList;
            private set { _bookingsList = value; RaisePropertyChanged(() => BookingsList); }
        }

        public ICommand ItemSelectedCommand { get; }
        private void ItemSelected(BookingsItemViewModel item)
        {
            ShowViewModel<BookingConfirmedViewModel>(new { id = item.Id });
        }

        #endregion Items

        #region Navigation

        public ICommand BookingCommand { get; }

        public string BookingButtonTitle => "Book a new room";

        private void ShowBooking()
        {
            ShowViewModel<RoomIndexViewModel>();
        }

        #endregion Navigation

        #region Refresh

        protected override async Task RefreshAsync()
        {
            try
            {
                var myReservations = await _bookingService.GetMyReservationsAsync();

                var items = new List<BookingsItemViewModel>();

                foreach (var myReservation in myReservations.Reservations.OrderByDescending(r => r.ReservationStart))
                {
                    var roomModel = myReservations.Rooms.FirstOrDefault(r => r.Id == myReservation.RoomId);

                    if (roomModel != null)
                    {
                        var headerUrl = roomModel.Header != null ? roomModel.Header.Medium : Defaults.RoomHeaderDefault;

                        items.Add(new BookingsItemViewModel(myReservation.Id)
                        {
                            ImageUrl = headerUrl,
                            Title = roomModel.Name,
                            Date = myReservation.ReservationStart.ToString("D"),
                            Check = $"{myReservation.ReservationStart.ToString("t")} until {myReservation.ReservationEnd.ToString("t")}",
                        });
                    }
                }

                BookingsList = new ObservableCollection<BookingsItemViewModel>(items);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Refresh
    }
}
