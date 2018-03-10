using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class EventsViewModel : BaseViewModel
    {
        #region Constructor

        private List<IEvent> _items;
        private Guid _categoryId;
        private bool _isLoading;
        private MvxCommand _refreshCommand;

        private readonly IEventService _eventService;

        private readonly string _eventClosedText,
            _unattendButtonText,
            _attendButtonText,
            _peopleAttendingText,
            _eventInfoText,
            _eventDateText,
            _eventTimeLabel,
            _eventLocationLabel,
            _aboutHeaderLabel;

        public EventsViewModel(IPlatformService platformService, IEventService eventService) : base(platformService)
        {
            _eventService = eventService;

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

        public async Task Init(Guid categoryId)
        {
            try
            {
                _categoryId = categoryId;

                await SetTitleAsync();

                await LoadEvents();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private async Task SetTitleAsync()
        {
            if (_categoryId == Guid.Empty)
            {
                Title = GetResource(ResKeys.mobile_events_title_all_events);
            }
            else
            {
                try
                {
                    var category = await PlatformService.GetEventCategoryAsync(ApiPriority.UserInitiated, _categoryId);

                    if (category != null)
                    {
                        Title = category.Name;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }
            }
        }

        public async Task LoadEvents()
        {
            IsLoading = true;

            try
            {
                var eventModels = await _eventService.GetAllAsync(ApiPriority.UserInitiated, _categoryId != Guid.Empty ? _categoryId : (Guid?)null);

                var events = eventModels.Select(e => e.ToModel(_eventClosedText, _unattendButtonText, _attendButtonText, _peopleAttendingText, _eventInfoText, _eventDateText, _eventTimeLabel, _eventLocationLabel, _aboutHeaderLabel));

                Items = new List<IEvent>(events);
            }
            catch (Exception ex)
            {
                // TODO: Wat dan?
                ExceptionService.HandleException(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion Init

        #region Items

        public List<IEvent> Items
        {
            get => _items;
            private set { _items = value; RaisePropertyChanged(() => Items); }
        }

        private ICommand _itemSelectedCommand;

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new MvxCommand<IEvent>(SelectEvent));

        public void EventSelected(Guid id)
        {
            ShowViewModel<EventViewModel>(new { id = id });
        }

        private void SelectEvent(IEvent @event)
        {
            ShowViewModel<EventViewModel>(new { id = @event.Id });
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
        }

        public MvxCommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => await LoadEvents())); }
        }

        private void UpdateList(IEnumerable<IEvent> events)
        {
            var hasChanges = false;

            foreach (var @event in events)
            {
                var indexOf = Items.FindIndex(i => i.Id == @event.Id);

                if (indexOf == -1)
                {
                    // Item not in list
                    InsertEvent(@event);
                    hasChanges = true;
                }
                else
                {
                    var oldEvent = Items[indexOf];

                    // Update index in list
                    Items[indexOf] = @event;

                    // Check if date changed
                    // Nullcheck not really needed since the event has already been found
                    if (oldEvent != null && oldEvent.DateTime != @event.DateTime)
                    {
                        // Move event
                        MoveEvent(@event);
                        hasChanges = true;
                    }
                }
            }

            if (hasChanges)
                RaisePropertyChanged(() => Items);
        }

        private void MoveEvent(IEvent @event)
        {
            var currentIndex = Items.IndexOf(@event);

            var newIndex = GetIndexBasedOnDate(@event);

            if (currentIndex != newIndex)
            {
                Items.RemoveAt(currentIndex);

                Items.Insert(newIndex, @event);
            }
        }

        private void InsertEvent(IEvent @event)
        {
            // Item is not in list
            var newIndex = GetIndexBasedOnDate(@event);

            Items.Insert(newIndex, @event);
        }

        private int GetIndexBasedOnDate(IEvent @event)
        {
            var closestEvent = Items.OrderBy(t => Math.Abs((t.DateTime - @event.DateTime).Ticks)).FirstOrDefault();

            var closestEventIndex = Items.IndexOf(closestEvent);
            int newIndex;

            if (@event.DateTime > closestEvent.DateTime)
            {
                // Younger then
                newIndex = closestEventIndex;
            }
            else
            {
                // Older then
                newIndex = closestEventIndex + 1;
            }

            return newIndex;
        }

        #endregion Items

        #region Event hooks

        public override void Attach()
        {
            base.Attach();

            Mvx.Resolve<ISignalRClient>().UpdateEvent += UpdateEvent;
            Mvx.Resolve<ISignalRClient>().DeleteEvent += DeleteEvent;
        }

        public override void Unattach()
        {
            base.Unattach();

            Mvx.Resolve<ISignalRClient>().UpdateEvent -= UpdateEvent;
            Mvx.Resolve<ISignalRClient>().DeleteEvent -= DeleteEvent;
        }

        #endregion Event hooks

        #region SignalR

        private void UpdateEvent(ApiWhitelabelEventModel model)
        {
            InvokeOnMainThread(async () =>
            {
                if (model.CategoryId == _categoryId)
                {
                    var events = await _eventService.GetAllAsync(ApiPriority.Background, _categoryId != Guid.Empty ? _categoryId : (Guid?)null);
                    Items = new List<IEvent>(events.OrderByDescending(e => e.DateTime).Select(e => e.ToModel(_eventClosedText, _unattendButtonText, _attendButtonText, _peopleAttendingText, _eventInfoText, _eventDateText, _eventTimeLabel, _eventLocationLabel, _aboutHeaderLabel)));
                }
            });
        }

        private void DeleteEvent(Guid eventId)
        {
            if (Items.Any(i => i.Id == eventId))
            {
                InvokeOnMainThread(() =>
                {
                    Items.RemoveAll(i => i.Id == eventId);
                    RaisePropertyChanged(() => Items);
                });
            }
        }

        #endregion SignalR
    }
}
