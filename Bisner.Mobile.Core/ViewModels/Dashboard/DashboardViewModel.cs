using Bisner.ApiModels.Central;
using Bisner.ApiModels.Security.Roles;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Booking;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.Core.ViewModels.Members;
using Bisner.Mobile.Core.ViewModels.Notifications;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class DashboardViewModel : BaseRefreshViewModel
    {
        #region Constructor   

        private readonly IEventService _eventService;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IIntegrationService _integrationService;

        public DashboardViewModel(IPlatformService platformService, IEventService eventService, IBookingService bookingService, IUserService userService, IGroupService groupService, IIntegrationService integrationService) : base(platformService)
        {
            _eventService = eventService;
            _bookingService = bookingService;
            _userService = userService;
            _groupService = groupService;
            _integrationService = integrationService;

            ShowMembersCommand = new MvxCommand(() => ShowViewModel<MembersViewModel>());
            ItemSelectedCommand = new MvxCommand<CommonItemViewModel>(SelectItem);

            HeaderSliderList = new MvxObservableCollection<HeaderSliderItemViewModel>();
            BodySliderList = new ObservableCollection<BodySliderItemViewModel>();
            RouterItems = new ObservableCollection<CommonItemViewModel>();
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            await InvokeRefreshAsync();
        }

        #endregion Init

        #region Header

        private async Task FillHeaderAsync()
        {
            try
            {
                var platformModel = await PlatformService.GetPublicPlatformAsync(ApiPriority.UserInitiated);
                var customMenu = await PlatformService.GetPlatformCustomMenuAsync(ApiPriority.UserInitiated);

                platformModel.AllowBookingSystem = true;

                FillMenu(customMenu, platformModel);

                if (customMenu.Events && Settings.UserRoles.Any(r => r == Event.View.ToLower()))
                {
                    var nextEvent = await _eventService.GetUpcomingAsync(ApiPriority.UserInitiated, 1);

                    if (nextEvent != null && nextEvent.Any())
                    {
                        var headerUrl = nextEvent[0].Header == null
                            ? Defaults.EventHeaderDefaultString
                            : nextEvent[0].Header?.Medium;

                        HeaderSliderList.Add(new HeaderSliderItemViewModel(new HeaderSliderData(nextEvent[0].Id,
                            headerUrl, nextEvent[0].Title, nextEvent[0].SubTitle, nextEvent[0].DateTime.ToString(),
                            HeaderSliderItemType.EVENT)));
                    }
                }

                if (platformModel.AllowBookingSystem && Settings.UserRoles.Any(r => r == ApiModels.Security.Roles.Booking.View.ToLower()))
                {
                    var allRooms = await _bookingService.GetAllRoomsAsync(ApiPriority.UserInitiated);

                    // Get random room
                    var room = allRooms.FirstOrDefault();

                    if (room != null)
                    {
                        // Get default header if header is null
                        var headerUrl = room.Header == null ? Defaults.RoomHeaderDefault : room.Header.Medium;

                        var headerViewModel = new HeaderSliderItemViewModel(new HeaderSliderData(room.Id, headerUrl,
                            room.Name, HeaderSliderItemType.BOOKROOM));

                        Task.Factory.StartNew(async () =>
                        {
                            var reservations = await _bookingService.GetReservationsAsync(ApiPriority.Background, room.Id, true);

                            if (reservations.Any(r => r.ReservationStart < DateTime.Now &&
                                                    r.ReservationEnd > DateTime.Now))
                            {
                                headerViewModel.IsAvailable = false;
                            }
                        }).ConfigureAwait(false);

                        HeaderSliderList.Add(headerViewModel);
                    }
                }

                if (!customMenu.Booking && !customMenu.Events && customMenu.Groups && Settings.UserRoles.Any(r => r == Group.View.ToLower()))
                {
                    // Show group in header
                    var allGroups = await _groupService.GetAllAsync(ApiPriority.UserInitiated);

                    var group = allGroups.FirstOrDefault();

                    if (group != null)
                    {
                        HeaderSliderList.Add(new HeaderSliderItemViewModel(new HeaderSliderData(group.Id, group.Header == null ? Defaults.GroupHeaderDefault : group.Header.Medium, group.Name, HeaderSliderItemType.GROUP)));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private MvxObservableCollection<HeaderSliderItemViewModel> _headerSliderList;
        public MvxObservableCollection<HeaderSliderItemViewModel> HeaderSliderList
        {
            get => _headerSliderList;
            private set { _headerSliderList = value; RaisePropertyChanged(() => HeaderSliderList); }
        }

        #endregion Header

        #region Body

        private async Task FillBodyAsync()
        {
            if (Settings.UserRoles.All(r => r != ApiModels.Security.Roles.Members.View.ToLower()))
                return;

            try
            {
                var allPublicUsers = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);

                // Get latest online
                var lastOnlineUsers = allPublicUsers.OrderByDescending(u => u.LastLoginDate).Take(10).ToList();

                var viewModels = new List<BodySliderItemViewModel>();

                foreach (var userModel in lastOnlineUsers)
                {
                    var bodySliderData = new BodySliderData(userModel.Avatar?.Small);

                    var bodySliderItemViewModel = new BodySliderItemViewModel(bodySliderData, () => ShowViewModel<UserViewModel>(new { userId = userModel.Id }));

                    viewModels.Add(bodySliderItemViewModel);
                }

                foreach (var bodySliderItemViewModel in viewModels)
                {
                    BodySliderList.Add(bodySliderItemViewModel);
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private ObservableCollection<BodySliderItemViewModel> _bodySliderList;
        public ObservableCollection<BodySliderItemViewModel> BodySliderList
        {
            get => _bodySliderList;
            private set { _bodySliderList = value; RaisePropertyChanged(() => BodySliderList); }
        }

        #endregion Body

        #region Navigation

        private void FillMenu(PlatformCustomMenuModel customMenu, ApiCentralPublicPlatformModel platformModel)
        {
            try
            {
                var routerItems = new List<CommonItemViewModel>();

                if (App.AppPlatform == AppPlatform.Android && Settings.UserRoles.Any(r => r == ApiModels.Security.Roles.Members.View.ToLower()))
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(() => ShowViewModel<MembersViewModel>())) { ImageUrl = "", Title = "Members" });
                }

                if (customMenu.Events && Settings.UserRoles.Any(r => r == Event.View.ToLower()))
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(() => ShowViewModel<EventCategoriesViewModel>())) { ImageUrl = "", Title = "Events" });
                }

                if (customMenu.Groups && Settings.UserRoles.Any(r => r == Group.View.ToLower()))
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(() => ShowViewModel<GroupsViewModel>())) { ImageUrl = "", Title = "Groups" });
                }

                if (Settings.AccessControlEnabled)
                {
                    // Notification tab is removed so we place it on the dashboard
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(() =>
                    {
                        if (App.AppPlatform == AppPlatform.Android)
                            ShowViewModel<AndroidNotificationsViewModel>();
                        else
                            ShowViewModel<NotificationsViewModel>();

                    }))
                    { ImageUrl = "", Title = "Notifications" });
                }

                if (platformModel.AllowBookingSystem && customMenu.Booking && App.AppPlatform != AppPlatform.Android && Settings.UserRoles.Any(r => r == ApiModels.Security.Roles.Booking.View.ToLower()))
                {
                    if (Settings.UserRoles.Any(r => r == Event.Create.ToLower()))
                        routerItems.Add(new CommonItemViewModel(new MvxCommand(() => ShowViewModel<RoomIndexViewModel>())) { ImageUrl = "", Title = "Booking" });
                }

                if (platformModel.RoomzillaEnabled)
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(async () =>
                    {
                        var roomzillaUrl = await _integrationService.GetRoomzillaUrlAsync();

                        ShowViewModel<WebBrowserViewModel>(new { url = roomzillaUrl });
                    }))
                    { ImageUrl = "", Title = "" });
                }

                if (platformModel.AllowBookingSystem && customMenu.Booking && App.AppPlatform != AppPlatform.Android && Settings.UserRoles.Any(r => r == ApiModels.Security.Roles.Booking.View.ToLower()))
                {
                    routerItems.Add(
                        new CommonItemViewModel(new MvxCommand(() => ShowViewModel<JobboardViewModel>()))
                        {
                            ImageUrl = "",
                            Title = "Manage my bookings"
                        });
                }

                // Nexudus
                if (platformModel.EnableIntegrationBookingOption || platformModel.EnableIntegrationBookingOptionInNav)
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(async () =>
                    {
                        // Get the url
                        var nexudusUrl = await _integrationService.GetNexudusUrlAsync("bookings");

                        ShowViewModel<WebBrowserViewModel>(new { url = nexudusUrl });
                    }))
                    { ImageUrl = "", Title = GetResource(ResKeys.navigation_nexudus_bookings) });
                }

                if (platformModel.EnableIntegrationBookingSearchOption || platformModel.EnableIntegrationBookingSearchOptionInNav)
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(async () =>
                        {
                            // Get the url
                            var nexudusUrl = await _integrationService.GetNexudusUrlAsync("bookings/search");

                            ShowViewModel<WebBrowserViewModel>(new { url = nexudusUrl });
                        }))
                    { ImageUrl = "", Title = GetResource(ResKeys.navigation_nexudus_bookings_search) });
                }

                if (platformModel.EnableIntegrationBookingCalendarOption || platformModel.EnableIntegrationBookingCalendarOptionInNav)
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(async () =>
                        {
                            // Get the url
                            var nexudusUrl = await _integrationService.GetNexudusUrlAsync("bookings/calendar");

                            ShowViewModel<WebBrowserViewModel>(new { url = nexudusUrl });
                        }))
                    { ImageUrl = "", Title = GetResource(ResKeys.navigation_nexudus_bookings_calendar) });
                }

                if (platformModel.EnableIntegrationAllowanceOption || platformModel.EnableIntegrationAllowanceOptionInNav)
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(async () =>
                    {
                        // Get the url
                        var nexudusUrl = await _integrationService.GetNexudusUrlAsync("allowances");

                        ShowViewModel<WebBrowserViewModel>(new { url = nexudusUrl });
                    }))
                    { ImageUrl = "", Title = GetResource(ResKeys.navigation_nexudus_allowances) });
                }

                if (platformModel.EnableIntegrationInvoicesOption || platformModel.EnableIntegrationInvoicesOptionInNav)
                {

                    routerItems.Add(new CommonItemViewModel(new MvxCommand(async () =>
                    {
                        // Get the url
                        var nexudusUrl = await _integrationService.GetNexudusUrlAsync("invoices");

                        ShowViewModel<WebBrowserViewModel>(new { url = nexudusUrl });
                    }))
                    { ImageUrl = "", Title = GetResource(ResKeys.navigation_nexudus_invoices) });
                }

                // TODO : Collaborate
                foreach (var platformCustomMenuItemModel in customMenu.CustomItems)
                {
                    if (!platformCustomMenuItemModel.InternalLink && platformCustomMenuItemModel.ShowOnMobile)
                    {
                        routerItems.Add(new CommonItemViewModel(new MvxCommand(() => ShowViewModel<WebBrowserViewModel>(new { url = platformCustomMenuItemModel.Url })))
                        {
                            ImageUrl = platformCustomMenuItemModel.Icon?.Small,
                            Title = platformCustomMenuItemModel.Name
                        });
                    }
                }

                if (App.AppPlatform == AppPlatform.Android && Mvx.Resolve<IConfiguration>().TestMode)
                {
                    routerItems.Add(new CommonItemViewModel(new MvxCommand(() => ShowViewModel<DevViewModel>())) { Title = "Dev", ImageUrl = null });
                }

                foreach (var commonItemViewModel in routerItems)
                {
                    RouterItems.Add(commonItemViewModel);
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }

        }

        private ObservableCollection<CommonItemViewModel> _routerItems;
        public ObservableCollection<CommonItemViewModel> RouterItems
        {
            get => _routerItems;
            private set { _routerItems = value; RaisePropertyChanged(() => RouterItems); }
        }

        public ICommand ShowMembersCommand { get; }

        public ICommand ItemSelectedCommand { get; }

        public void SelectItem(CommonItemViewModel commonItemViewModel)
        {
            commonItemViewModel.SelectCommand.Execute(null);
        }

        #endregion Navigation

        protected override async Task RefreshAsync()
        {
            await FillHeaderAsync();
            await FillBodyAsync();
        }
    }
}