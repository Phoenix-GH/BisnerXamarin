using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Bisner.Mobile.Core.Models.Booking;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class RoomTimeIndexItemViewModel : MvxViewModel
    {
        #region Constructor

        public RoomTimeIndexItemViewModel(RoomData data)
        {
            ImageUrl = data.ImageUrl;
            Title = data.Title;
            IsAvailable = data.IsAvailable;
            TimeBlockType = data.TimeBlockType;

            StartEndTime = data.StartEndTime;
            BlockTimeList = data.BlockTimeList;

            ReservedTimeList = new ObservableCollection<TimePickerData>();

            for (var i = data.StartEndTime.StartTime.Hour; i < data.StartEndTime.EndTime.Hour; i++)
            {
                var item = new TimePickerData(i, 0);
                ReservedTimeList.Add(item);
            }

            TimeLineList = new ObservableCollection<TimeLineItemViewModel>(ReservedTimeList.Select(t => new TimeLineItemViewModel(t, TimeBlockType)));

            ShowRoomCommand = new MvxCommand(() => ShowViewModel<RoomDetailViewModel>(new { roomId = data.RoomId }));
        }

        #endregion Constructor

        #region Properties

        //public TimePickerData StartTime { get; set; }
        //public TimePickerData EndTime { get; set; }
        //public TimePickerData BlockStartTime { get; set; }
        //public TimePickerData BlockEndTime { get; set; }

        private TimeBlock _startEndTime;
        public TimeBlock StartEndTime
        {
            get => _startEndTime;
            set { _startEndTime = value; RaisePropertyChanged(() => StartEndTime); }
        }

        private List<TimeBlock> _blockTimeList;
        public List<TimeBlock> BlockTimeList
        {
            get => _blockTimeList;
            set { _blockTimeList = value; RaisePropertyChanged(() => BlockTimeList); }
        }

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

        public TimeBlockType TimeBlockType { get; set; }

        public ObservableCollection<TimePickerData> ReservedTimeList { get; set; }

        private ObservableCollection<TimeLineItemViewModel> _timeLineList;
        public ObservableCollection<TimeLineItemViewModel> TimeLineList
        {
            get => _timeLineList;
            private set { _timeLineList = value; RaisePropertyChanged(() => TimeLineList); }
        }

        public ICommand ShowRoomCommand { get; }

        #endregion Properties
    }
}
