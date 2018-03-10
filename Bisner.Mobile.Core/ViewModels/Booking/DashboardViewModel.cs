//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Bisner.Mobile.Communication;
//using Bisner.Mobile.Core.Models.Booking;
//using Bisner.Mobile.Core.Service;
//using Bisner.Mobile.Core.ViewModels.Base;
//using MvvmCross.Core.ViewModels;

//namespace Bisner.Mobile.Core.ViewModels.Booking
//{
//    public class DashboardViewModel : BaseViewModel
//    {
//        #region Constructor

//        private readonly IEventService _eventService;

//        public DashboardViewModel(IPlatformService platformService, IEventService eventService) : base(platformService)
//        {
//            _eventService = eventService;
//        }

//        #endregion Constructor

//        #region Init

//        public async Task Init()
//        {
//            try
//            {
//                var nextUpcomingEvent = await _eventService.GetUpcomingAsync(ApiPriority.UserInitiated, 1);
                
//            }
//            catch (Exception ex)
//            {
//                ExceptionService.HandleException(ex);
//            }

//            HeaderSliderList = new ObservableCollection<HeaderSliderItemViewModel>();
//            BodySliderList = new ObservableCollection<BodySliderItemViewModel>();
//        }

//        #endregion Init

//        #region Header

//        public ObservableCollection<HeaderSliderItemViewModel> HeaderSliderList
//        {
//            get;
//            private set;
//        }

//        #endregion Header

//        #region Body

//        public ObservableCollection<BodySliderItemViewModel> BodySliderList
//        {
//            get;
//            private set;
//        }

//        #endregion Body

//        public ICommand JobboardBtnClickedCommand => new MvxCommand(NavigateToJobboard);

//        public ICommand EventsBtnClickedCommand => new MvxCommand(NavigateToEvents);

//        // TODO | Yann:  This is the fake data.
//        public void FetchData()
//        {
//            var headerSliderDataList = new ObservableCollection<HeaderSliderData>();
//            headerSliderDataList.CollectionChanged += OnHeaderSliderCollectionChanged;
//            var headerItem1 = new HeaderSliderData("DummyImage", "Lunch Vendor @ Venture Cafe", "next upcoming event", "April 24th", HeaderSliderItemType.EVENT);
//            var headerItem2 = new HeaderSliderData("DummyImage", "The great room", HeaderSliderItemType.BOOKROOM);
//            headerSliderDataList.Add(headerItem1);
//            headerSliderDataList.Add(headerItem2);

//            var bodySliderDataList = new ObservableCollection<BodySliderData>();
//            bodySliderDataList.CollectionChanged += OnBodySliderCollectionChanged;
//            var bodyItem1 = new BodySliderData("DummyImage");
//            var bodyItem2 = new BodySliderData("DummyImage");
//            var bodyItem3 = new BodySliderData("DummyImage");
//            var bodyItem4 = new BodySliderData("DummyImage");
//            var bodyItem5 = new BodySliderData("DummyImage");
//            bodySliderDataList.Add(bodyItem1);
//            bodySliderDataList.Add(bodyItem2);
//            bodySliderDataList.Add(bodyItem3);
//            bodySliderDataList.Add(bodyItem4);
//            bodySliderDataList.Add(bodyItem5);
//        }

//        private void OnHeaderSliderCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                    var list = e.NewItems;
//                    foreach (HeaderSliderData item in list)
//                    {
//                        var model = new HeaderSliderItemViewModel(item);
//                        this.HeaderSliderList.Add(model);
//                    }
//                    break;
//            }
//        }

//        private void OnBodySliderCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                    var list = e.NewItems;
//                    foreach (BodySliderData item in list)
//                    {
//                        var model = new BodySliderItemViewModel(item);
//                        this.BodySliderList.Add(model);
//                    }
//                    break;
//            }
//        }

//        private void NavigateToJobboard()
//        {
//            this.ShowViewModel<JobboardViewModel>();
//        }

//        private void NavigateToEvents()
//        {
//            //TODO | Yann: This is the wrong navigation. It's just only for test.
//            this.ShowViewModel<RoomTimeIndexViewModel>();
//        }
//    }
//}
