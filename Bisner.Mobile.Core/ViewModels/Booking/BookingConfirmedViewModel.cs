using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Notifications;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class BookingConfirmedViewModel : BaseViewModel
    {
        #region Constructor

        private Guid _id;

        private string _imageUrl;
        private string _reservationCode;
        private string _date;
        private string _checkin;
        private string _checkout;
        private string _room;
        private string _location;

        private readonly IBookingService _bookingService;
        private ObservableCollection<BodySliderItemViewModel> _roomList;
        private bool _isPrivate;
        private string _message;

        public BookingConfirmedViewModel(IPlatformService platformService, IBookingService bookingService) : base(platformService)
        {
            _bookingService = bookingService;

            MoreCommand = new MvxAsyncCommand(More);
            SaveCommand = new MvxAsyncCommand(Save);

            // Assing in constructor for property listeners in views
            RoomList = new ObservableCollection<BodySliderItemViewModel>();
        }

        #endregion Constructor

        #region Init

        public async Task Init(Guid id)
        {
            try
            {
                _id = id;

                var myreservations = await _bookingService.GetMyReservationsAsync();

                var reservation = myreservations?.Reservations?.FirstOrDefault(r => r.Id == id);

                if (reservation != null)
                {
                    var roomModel = myreservations.Rooms?.FirstOrDefault(r => r.Id == reservation.RoomId);

                    if (roomModel != null)
                    {
                        ImageUrl = roomModel.Header != null ? roomModel.Header.Large : Defaults.RoomHeaderDefault;
                        ReservationCode = reservation.ReservationCode;
                        Date = reservation.ReservationStart.ToString("D");
                        Checkin = reservation.ReservationStart.ToString("t");
                        Checkout = reservation.ReservationEnd.ToString("t");
                        Room = roomModel.Name;
                        IsPrivate = reservation.IsPrivate;
                        Message = reservation.Message;
                        Title = reservation.Title;

                        foreach (var roomModelImage in roomModel.Images)
                        {
                            RoomList.Add(new BodySliderItemViewModel(new BodySliderData(roomModelImage.Medium), () =>
                            {
                                ShowViewModel<ImageZoomViewModel>(new { url = roomModelImage.Large });
                            }));
                        }

                        // Get location
                        var locationmodel = await PlatformService.GetLocationAsync(ApiPriority.UserInitiated, roomModel.LocationId);
                        Location = locationmodel?.Name;
                    }
                }
                else
                {
                    // TODO : Show some error?
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Init

        #region commands

        public ICommand MoreCommand { get; }

        public ICommand SaveCommand { get; }

        #endregion

        #region properties

        public ObservableCollection<BodySliderItemViewModel> RoomList
        {
            get => _roomList;
            private set { _roomList = value; RaisePropertyChanged(() => RoomList); }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        public string ReservationCode
        {
            get => _reservationCode;
            set { _reservationCode = value; RaisePropertyChanged(() => ReservationCode); }
        }

        public string Date
        {
            get => _date;
            set { _date = value; RaisePropertyChanged(() => Date); }
        }

        public string Checkin
        {
            get => _checkin;
            set { _checkin = value; RaisePropertyChanged(() => Checkin); }
        }

        public string Checkout
        {
            get => _checkout;
            set { _checkout = value; RaisePropertyChanged(() => Checkout); }
        }

        public string Room
        {
            get => _room;
            set { _room = value; RaisePropertyChanged(() => Room); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; RaisePropertyChanged(() => Location); }
        }

        public bool IsPrivate
        {
            get => _isPrivate;
            set { _isPrivate = value; RaisePropertyChanged(() => IsPrivate); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; RaisePropertyChanged(() => Message); }
        }

        #endregion

        #region Functions

        private async Task More()
        {
            try
            {
                var result = await UserDialogs.ActionSheetAsync("More", "Close", null, null, "Cancel Booking");

                switch (result)
                {
                    case "Cancel Booking":
                        await CancelBookingAsync();
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private async Task Save()
        {
            try
            {
                var response = await _bookingService.UpdateReservationAsync(_id, Title, Message, Message, IsPrivate);

                if (!response.Success)
                {
                    await UserDialogs.AlertAsync(new AlertConfig
                    {
                        Title = "Error",
                        Message = "An error occured, please try again",
                        OkText = "Ok"
                    });
                }
                else
                {
                    Close(this);
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private async Task CancelBookingAsync()
        {
            var result = await UserDialogs.ConfirmAsync(new ConfirmConfig
            {
                OkText = "Continue",
                Title = "Are you sure?",
                Message = null,
                CancelText = "Cancel"
            });

            if (result)
            {
                try
                {
                    var apiResult = await _bookingService.CancelBookingAsync(_id);

                    if (apiResult.Success)
                    {
                        Mvx.Resolve<IMvxMessenger>().Publish(new UpdateRervationsMessage(this));
                        Close(this);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }
            }
        }


        #endregion Functions
    }
}
