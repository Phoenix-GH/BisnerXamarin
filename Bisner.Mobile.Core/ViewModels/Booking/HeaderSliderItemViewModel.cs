using System;
using System.Linq;
using System.Windows.Input;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class HeaderSliderItemViewModel : MvxViewModel
    {
        private readonly Guid _id;

        public HeaderSliderItemViewModel(HeaderSliderData data)
        {
            _id = data.Id;
            ImageUrl = data.ImageUrl;
            Title = data.Title;
            SubTitle = data.SubTitle;
            Date = data.Date;
            Type = data.Type;
            IsAvailable = data.IsAvailable;
        }

        #region command

        public ICommand DetailBtnClickedCommand => new MvxCommand(NavigateToDetail);

        public ICommand ShowAllBtnClickedCommand => new MvxCommand(ShowAll);

        #endregion

        #region property

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private string _subTitle;
        public string SubTitle
        {
            get => _subTitle;
            set { _subTitle = value; RaisePropertyChanged(() => SubTitle); }
        }

        private string _date;
        public string Date
        {
            get => _date;
            set { _date = value; RaisePropertyChanged(() => Date); }
        }

        private HeaderSliderItemType _type;
        public HeaderSliderItemType Type
        {
            get => _type;
            set { _type = value; SetupActions(); RaisePropertyChanged(() => Type); }
        }

        #endregion Properties

        #region Functions

        private void SetupActions()
        {
            if (_type == HeaderSliderItemType.EVENT)
            {
                SubtitleVisible = true;
                DateVisible = true;
                DetailButtonVisisble = false;
                StatusVisible = false;
                BookRoomButtonVisible = false;
                ShowAllText = "Show all events";
            }
            else if (_type == HeaderSliderItemType.BOOKROOM)
            {
                SubtitleVisible = false;
                DateVisible = false;
                DetailButtonVisisble = true;
                StatusVisible = true;
                if (Settings.UserRoles.Any(r => r == ApiModels.Security.Roles.Booking.Create.ToLower()))
                {
                    BookRoomButtonVisible = true;
                }
                ShowAllText = "Show all meeting rooms";
            }
            else if (_type == HeaderSliderItemType.GROUP)
            {
                SubtitleVisible = false;
                DateVisible = false;
                DetailButtonVisisble = false;
                StatusVisible = false;
                BookRoomButtonVisible = false;
                ShowAllText = "Show all groups";
            }
        }

        private string _showAllText;
        public string ShowAllText
        {
            get => _showAllText;
            set { _showAllText = value; RaisePropertyChanged(() => ShowAllText); }
        }

        private bool _subtitleVisible;
        public bool SubtitleVisible
        {
            get => _subtitleVisible;
            set { _subtitleVisible = value; RaisePropertyChanged(() => SubtitleVisible); }
        }

        private bool _detailButtonVisisble;
        public bool DetailButtonVisisble
        {
            get => _detailButtonVisisble;
            set { _detailButtonVisisble = value; RaisePropertyChanged(() => DetailButtonVisisble); }
        }

        private bool _dateVisible;
        public bool DateVisible
        {
            get => _dateVisible;
            set { _dateVisible = value; RaisePropertyChanged(() => DateVisible); }
        }

        private bool _statusVisible;
        private bool _bookRoomButtonVisible;
        private bool _isAvailable;

        public bool StatusVisible
        {
            get => _statusVisible;
            set { _statusVisible = value; RaisePropertyChanged(() => StatusVisible); }
        }

        public ICommand BookRoomCommand => new MvxCommand(BookRoom);

        public bool BookRoomButtonVisible
        {
            get => _bookRoomButtonVisible;
            set { _bookRoomButtonVisible = value; RaisePropertyChanged(() => BookRoomButtonVisible); }
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; RaisePropertyChanged(() => IsAvailable); }
        }

        public void BookRoom()
        {
            ShowViewModel<DatePickerViewModel>(new { roomId = _id });
        }

        public void NavigateToDetail()
        {
            if (Type == HeaderSliderItemType.EVENT)
            {
                ShowViewModel<EventViewModel>(new { id = _id });
            }
            else if (Type == HeaderSliderItemType.BOOKROOM)
            {
                // Navigate to Room Detial
                ShowViewModel<RoomDetailViewModel>(new { roomId = _id });
            }
            else if (Type == HeaderSliderItemType.GROUP)
            {
                // Navigate to Room Detial
                ShowViewModel<FeedViewModel>(new { id = _id, feedType = FeedType.Group });
            }
        }

        public void ShowAll()
        {
            if (Type == HeaderSliderItemType.EVENT)
            {
                ShowViewModel<EventsViewModel>();
            }
            else if (Type == HeaderSliderItemType.BOOKROOM)
            {
                // Show all meeting rooms. 
                ShowViewModel<RoomIndexViewModel>();
            }
            else if (Type == HeaderSliderItemType.GROUP)
            {
                ShowViewModel<GroupsViewModel>();
            }
        }

        #endregion
    }
}
