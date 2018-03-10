using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Booking;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class RoomDetailViewModel : BaseViewModel
    {
        #region Constructor

        private Guid _roomId;

        private readonly IBookingService _bookingService;

        public RoomDetailViewModel(IPlatformService platformService, IBookingService bookingService) : base(platformService)
        {
            _bookingService = bookingService;

            CheckBtnClickedCommand = new MvxCommand(Check);

            // Assing in constructor for property listeners in views
            RoomList = new ObservableCollection<BodySliderItemViewModel>();
        }

        #endregion Constructor

        #region Init

        public async Task Init(Guid roomId)
        {
            try
            {
                _roomId = roomId;

                var roomModel = await _bookingService.GetRoomAsync(ApiPriority.UserInitiated, roomId);
                var roomReservations = await _bookingService.GetReservationsAsync(ApiPriority.UserInitiated, roomId);

                if (roomModel != null)
                {
                    // Room info
                    Title = roomModel.Name;
                    AboutRoomTitle = $"About {roomModel.Name}";
                    AboutRoomText = roomModel.Description;
                    AboutRoomVisible = !string.IsNullOrWhiteSpace(roomModel.Description);
                    NumberOfPersons = $"{roomModel.NumberOfGuests} Persons";

                    // Room images
                    var imageList = roomModel.Images.Select(roomModelImage => new BodySliderData(roomModelImage.Medium)).ToList();
                    foreach (var bodySliderData in imageList)
                    {
                        RoomList.Add(new BodySliderItemViewModel(bodySliderData, () =>
                        {
                            ShowViewModel<ImageZoomViewModel>(new { url = bodySliderData.ImageUrl });
                        }));
                    }

                    ImageUrl = roomModel.Header == null ? Defaults.RoomHeaderDefault : roomModel.Header.Medium;
                    // Check reservations at this current time
                    IsAvailable =
                        !roomReservations.Any(t => t.ReservationStart < DateTime.Now &&
                                                   t.ReservationEnd > DateTime.Now);

                    var locationModel = await PlatformService.GetLocationAsync(ApiPriority.UserInitiated, roomModel.LocationId);
                    if (locationModel != null)
                    {
                        LocationName = locationModel.Name;
                        AboutLocationTitle = $"About {locationModel.Name}";
                        AboutLocationText = locationModel.AboutText;
                        AboutLocationVisible = !string.IsNullOrWhiteSpace(locationModel.AboutText);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Init

        #region Commands

        public ICommand CheckBtnClickedCommand { get; }

        #endregion Commands

        #region Properties

        private ObservableCollection<BodySliderItemViewModel> _roomList;
        public ObservableCollection<BodySliderItemViewModel> RoomList
        {
            get => _roomList;
            private set { _roomList = value; RaisePropertyChanged(() => RoomList); }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; RaisePropertyChanged(() => IsAvailable); }
        }

        private string _locationName;
        public string LocationName
        {
            get => _locationName;
            set { _locationName = value; RaisePropertyChanged(() => LocationName); }
        }

        private string _aboutLocationTitle;
        public string AboutLocationTitle
        {
            get => _aboutLocationTitle;
            set { _aboutLocationTitle = value; RaisePropertyChanged(() => AboutLocationTitle); }
        }

        private string _aboutLocationText;
        public string AboutLocationText
        {
            get => _aboutLocationText;
            set { _aboutLocationText = value; RaisePropertyChanged(() => AboutLocationText); }
        }

        private bool _aboutLocationVisible;
        public bool AboutLocationVisible
        {
            get => _aboutLocationVisible;
            set { _aboutLocationVisible = value; RaisePropertyChanged(() => AboutLocationVisible); }
        }

        private string _aboutRoomTitle;
        public string AboutRoomTitle
        {
            get => _aboutRoomTitle;
            set { _aboutRoomTitle = value; RaisePropertyChanged(() => AboutRoomTitle); }
        }

        private string _aboutRoomText;
        public string AboutRoomText
        {
            get => _aboutRoomText;
            set { _aboutRoomText = value; RaisePropertyChanged(() => AboutRoomText); }
        }

        private bool _aboutRoomVisible;
        public bool AboutRoomVisible
        {
            get => _aboutRoomVisible;
            set { _aboutRoomVisible = value; RaisePropertyChanged(() => AboutRoomVisible); }
        }

        private string _numberOfPersons;
        public string NumberOfPersons
        {
            get => _numberOfPersons;
            set { _numberOfPersons = value; RaisePropertyChanged(() => NumberOfPersons); }
        }

        #endregion

        #region funcitons

        private void Check()
        {
            ShowViewModel<DatePickerViewModel>(new { roomId = _roomId });
        }

        #endregion
    }
}
