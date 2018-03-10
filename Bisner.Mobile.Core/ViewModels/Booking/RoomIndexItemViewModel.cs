using System;
using System.Windows.Input;
using Bisner.Mobile.Core.Models.Booking;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class RoomIndexItemViewModel : MvxViewModel
    {
        #region Constructor

        private readonly Guid _roomId;

        public RoomIndexItemViewModel(RoomData data)
        {
            _roomId = data.RoomId;

            ImageUrl = data.ImageUrl;
            Title = data.Title;
            IsAvailable = data.IsAvailable;

            BookRoomCommand = new MvxCommand(ShowBookRoom);
            RoomBookCommand = new MvxCommand(ShowRoomBook);
        }

        #endregion Constructor

        #region Properties

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

        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; RaisePropertyChanged(() => IsAvailable); }
        }

        #endregion Properties

        #region Commands

        public ICommand BookRoomCommand { get; }

        private void ShowBookRoom()
        {
            ShowViewModel<RoomDetailViewModel>(new { roomId = _roomId });
        }

        public ICommand RoomBookCommand { get; }

        private void ShowRoomBook()
        {
            ShowViewModel<DatePickerViewModel>(new { roomId = _roomId });
        }

        #endregion Commands
    }
}
