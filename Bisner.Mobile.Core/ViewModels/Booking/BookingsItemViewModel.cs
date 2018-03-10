using System;
using Bisner.Mobile.Core.Models.Booking;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class BookingsItemViewModel : MvxViewModel
    {
        #region Constructor

        public BookingsItemViewModel(Guid reservationid)
        {
            Id = reservationid;
        }

        #endregion Constructor

        #region Properties

        public Guid Id { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        private string _date;
        public string Date
        {
            get => _date;
            set { _date = value; RaisePropertyChanged(() => Date); }
        }

        private string _check;
        public string Check
        {
            get => _check;
            set { _check = value; RaisePropertyChanged(() => Check); }
        }

        #endregion Properties
    }
}
