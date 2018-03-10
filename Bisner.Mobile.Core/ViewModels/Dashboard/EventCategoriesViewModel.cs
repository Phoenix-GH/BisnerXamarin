using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class EventCategoriesViewModel : BaseViewModel
    {
        #region Constructor

        private List<IItemBase> _items;
        private readonly IEventService _eventService;
        private readonly ISignalRClient _signalRClient;

        private readonly string _eventClosedText,
            _unattendButtonText,
            _attendButtonText,
            _peopleAttendingText,
            _eventInfoText,
            _eventDateText,
            _eventTimeLabel,
            _eventLocationLabel,
            _aboutHeaderLabel;

        public EventCategoriesViewModel(IPlatformService platformService, IEventService eventService, ISignalRClient signalRClient) : base(platformService)
        {
            _eventService = eventService;
            _signalRClient = signalRClient;

            _eventClosedText = Settings.GetResource(ResKeys.event_detail_btn_event_closed);
            _unattendButtonText = Settings.GetResource(ResKeys.event_detail_btn_unattend);
            _attendButtonText = Settings.GetResource(ResKeys.event_detail_btn_attend);
            _peopleAttendingText = Settings.GetResource(ResKeys.event_detail_people_attending);
            _eventInfoText = Settings.GetResource(ResKeys.mobile_events_event_info);
            _eventDateText = Settings.GetResource(ResKeys.mobile_events_event_date);
            _eventTimeLabel = Settings.GetResource(ResKeys.mobile_events_event_time);
            _eventLocationLabel = Settings.GetResource(ResKeys.mobile_events_location);
            _aboutHeaderLabel = Settings.GetResource(ResKeys.mobile_events_about_event);
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            List<IEvent> events;

            try
            {
                var firstFour = await _eventService.GetUpcomingAsync(ApiPriority.UserInitiated, 4);

                events = firstFour.Select(e => e.ToModel(_eventClosedText, _unattendButtonText, _attendButtonText, _peopleAttendingText, _eventInfoText, _eventDateText, _eventTimeLabel, _eventLocationLabel, _aboutHeaderLabel)).ToList();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                events = new List<IEvent>();
            }

            await BuildEventCategories(events);
        }

        #endregion Init

        #region Handlers

        public override void Attach()
        {
            base.Attach();

            _signalRClient.UpdateEvent += OnUpdateEvent;
        }

        public override void Unattach()
        {
            base.Unattach();

            _signalRClient.UpdateEvent -= OnUpdateEvent;
        }

        private void OnUpdateEvent(ApiWhitelabelEventModel apiWhitelabelEventModel)
        {
            InvokeOnMainThread(async () =>
            {
                var firstFour = await _eventService.GetUpcomingAsync(ApiPriority.Background, 4);

                await BuildEventCategories(firstFour.Select(t => t.ToModel(_eventClosedText, _unattendButtonText, _attendButtonText, _peopleAttendingText, _eventInfoText, _eventDateText, _eventTimeLabel, _eventLocationLabel, _aboutHeaderLabel)));
            });
        }

        #endregion Handlers

        #region Refresh

        private MvxCommand _refreshCommand;
        private bool _isRefreshing;

        public MvxCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => await RefreshAsync()));
            }
        }

        private async Task RefreshAsync()
        {
            if (!IsRefreshing)
            {
                try
                {
                    var firstFour = await _eventService.GetUpcomingAsync(ApiPriority.UserInitiated, 4);

                    await BuildEventCategories(firstFour.Select(e => e.ToModel(_eventClosedText, _unattendButtonText, _attendButtonText, _peopleAttendingText, _eventInfoText, _eventDateText, _eventTimeLabel, _eventLocationLabel, _aboutHeaderLabel)));
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }
                finally
                {
                    IsRefreshing = false;
                }
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; RaisePropertyChanged(() => IsRefreshing); }
        }

        #endregion Refresh

        #region Items

        private ICommand _itemSelectedCommand;

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new MvxCommand<IItemBase>(SelectItem));

        public List<IItemBase> Items
        {
            get => _items;
            private set { _items = value; RaisePropertyChanged(() => Items); }
        }

        private async Task BuildEventCategories(IEnumerable<IEvent> firstFour)
        {
            try
            {
                var items = new List<IItemBase>(firstFour?.OrderBy(e => e.DateTime));

                var allEventitem = new AllEventsItem
                {
                    Text = GetResource(ResKeys.mobile_events_title_all_events),
                };

                items.Add(allEventitem);

                var apiEventCategories = await PlatformService.GetAllEventCategoriesAsync(ApiPriority.UserInitiated);

                var eventCategories = apiEventCategories.Select(c => c.ToModel()).ToList();

                if (eventCategories.Count == 1)
                {
                    var eventCategory = eventCategories.First();

                    // Use this when there is only 1 category
                    items.Add(new HeaderImage
                    {
                        Id = eventCategory.Id,
                        Image =
                            eventCategory.Image != null ? eventCategory.Image.Large : Defaults.EventHeaderDefaultString,
                        TitleText = eventCategory.Name,
                    });
                }
                else if (eventCategories.Count > 1)
                {
                    var leftOver = eventCategories.Count % 2;
                    var count = eventCategories.Count / 2 + leftOver;

                    for (var i = 0; i < count; i += 1)
                    {
                        var firstTwoEvents = eventCategories.Skip(i * 2).Take(2).ToList();

                        var firstEvent = firstTwoEvents.First();

                        var eventCategoryDouble = new EventCategoryDouble
                        {
                            Id1 = firstEvent.Id,
                            Title1 = firstEvent.Name,
                            Image1 = firstEvent.Image != null
                                ? firstEvent.Image.Large
                                : Defaults.EventHeaderDefaultString,
                            Image1Action = () =>
                            {
                                CategorySelected(firstEvent.Id);
                            },
                        };

                        if (firstTwoEvents.Count == 2)
                        {
                            var secondEvent = firstTwoEvents.Skip(1).First();


                            eventCategoryDouble.Id2 = secondEvent.Id;
                            eventCategoryDouble.Title2 = secondEvent.Name;
                            eventCategoryDouble.Image2 = secondEvent.Image != null
                                ? secondEvent.Image.Large
                                : Defaults.EventHeaderDefaultString;
                            eventCategoryDouble.Image2Action = () =>
                            {
                                CategorySelected(secondEvent.Id);
                            };
                        }

                        items.Add(eventCategoryDouble);
                    }
                }

                //var otherCategories = eventCategories.Skip(2).ToList();

                //if (otherCategories.Any())
                //{
                //    items.AddRange(otherCategories);
                //}

                Items = new List<IItemBase>(items);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        public void CategorySelected(Guid id)
        {
            ShowViewModel<EventsViewModel>(new { categoryId = id });
        }

        private void SelectItem(IItemBase item)
        {
            if (item is Event)
            {
                ((Event)item).ShowEventCommand.Execute();
            }

            if (item is EventCategory)
            {
                ((EventCategory)item).SelectedCommand.Execute();
            }

            if (item is EventCategoryDouble)
            {
                // TODO : Bepalen welke van de 2 is aangeklikt
            }

            if (item is AllEventsItem)
            {
                ((AllEventsItem)item).ShowCommand.Execute();
            }
        }

        #endregion Items
    }
}