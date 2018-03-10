using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Core.ViewModels.Members;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class EventViewModel : BaseRefreshViewModel
    {
        #region Constructor

        private Guid _id;

        private MvxCommand _joinCommand;
        private bool _isAttending;
        private bool _isBusy, _isNotBusy;
        private ExtendedObservableCollection<IItemBase> _items;
        private string _input;
        private MvxCommand _commentCommand;
        private bool _isCommenting;
        private bool _isNotCommenting;

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

        public EventViewModel(IPlatformService platformService, IEventService eventService) : base(platformService)
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

        public async Task Init(Guid id)
        {
            _id = id;

            if (IsRefreshing)
                return;

            IsRefreshing = true;

            var items = new List<IItemBase>();

            try
            {
                var eventModel = await GetEventAsync(id);

                if (eventModel != null)
                {
                    Event = eventModel.ToModel(_eventClosedText, _unattendButtonText, _attendButtonText, _peopleAttendingText, _eventInfoText, _eventDateText, _eventTimeLabel, _eventLocationLabel, _aboutHeaderLabel);
                    IsAttending = Event.AttendeesIds.Contains(Settings.UserId);

                    items.Add(Event);

                    items.AddRange(eventModel.Comments.Select(c => c.ToModel()).OrderBy(c => c.DateTime));
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }

            Items = new ExtendedObservableCollection<IItemBase>(items);

            IsRefreshing = false;
        }

        private async Task<ApiWhitelabelEventModel> GetEventAsync(Guid id)
        {
            try
            {
                var eventModel = await _eventService.GetAsync(ApiPriority.UserInitiated, id);

                return eventModel;
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                return null;
            }
        }

        #endregion Init

        #region Event

        private void UpdateEvent(ApiWhitelabelEventModel model)
        {
            if (model.Id == Event.Id)
            {
                InvokeOnMainThread(async () =>
                {
                    var eventModel = await _eventService.GetAsync(ApiPriority.Background, model.Id);

                    var evnt = eventModel.ToModel(_eventClosedText, _unattendButtonText, _attendButtonText, _peopleAttendingText, _eventInfoText, _eventDateText, _eventTimeLabel, _eventLocationLabel, _aboutHeaderLabel);

                    Event.Id = evnt.Id;
                    Event.Attendees = evnt.Attendees;
                    Event.AttendeesIds = evnt.AttendeesIds;
                    Event.CategoryId = evnt.CategoryId;
                    Event.DateTime = evnt.DateTime;
                    Event.CreationDateTime = evnt.CreationDateTime;
                    Event.Details = evnt.Details;
                    Event.Header = evnt.Header;
                    Event.Logo = evnt.Logo;
                    Event.Images = evnt.Images;
                    Event.Location = evnt.Location;
                    Event.IsPublished = evnt.IsPublished;
                    Event.SubTitle = evnt.SubTitle;
                    Event.Summary = evnt.Summary;
                    Event.Title = evnt.Title;

                    IsAttending = Event.AttendeesIds.Contains(Settings.UserId);

                    var comments = eventModel.Comments?.Select(c => c.ToModel());

                    AddNewComments(comments);
                });
            }
        }

        private void AddNewComments(IEnumerable<IComment> comments)
        {
            var newComments = comments.Where(c => Items.All(i => i.Id != c.Id));

            Items.AddRange(newComments.OrderBy(c => c.DateTime));
        }

        #endregion Event

        #region Items

        public ExtendedObservableCollection<IItemBase> Items
        {
            get => _items;
            private set { _items = value; RaisePropertyChanged(() => Items); }
        }

        #endregion Items

        #region Attend

        public IEvent Event { get; set; }

        public bool IsAttending
        {
            get => _isAttending;
            private set
            {
                _isAttending = value;
                RaisePropertyChanged(() => IsAttending);
            }
        }

        public MvxCommand AttendCommand
        {
            get { return _joinCommand ?? (_joinCommand = new MvxCommand(async () => { await Attend(); })); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value; RaisePropertyChanged(() => IsBusy);
                IsNotBusy = !value;
            }
        }

        public bool IsNotBusy
        {
            get => _isNotBusy;
            private set { _isNotBusy = value; RaisePropertyChanged(() => IsNotBusy); }
        }

        private async Task Attend()
        {
            if (!IsBusy)
            {
                IsBusy = true;

                try
                {
                    var eventModel = await _eventService.AttendEventAsync(Event.Id, !IsAttending);

                    IsAttending = eventModel.AttendeesIds.Contains(Settings.UserId);
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        #endregion Attend

        #region Comment

        public bool IsCommenting
        {
            get => _isCommenting;
            private set
            {
                IsNotCommenting = !value;
                _isCommenting = value; RaisePropertyChanged(() => IsCommenting);
            }
        }

        public bool IsNotCommenting
        {
            get => _isNotCommenting;
            private set
            {
                _isNotCommenting = value; RaisePropertyChanged(() => IsNotCommenting);
            }
        }

        public string Input
        {
            get => _input;
            set { _input = value; RaisePropertyChanged(() => Input); }
        }

        public MvxCommand CommentCommand
        {
            get { return _commentCommand ?? (_commentCommand = new MvxCommand(async () => await Comment())); }
        }

        private async Task Comment()
        {
            if (!IsCommenting)
            {
                IsCommenting = true;

                try
                {
                    var result = await _eventService.CommentEventAsync(Event.Id, Input);

                    UpdateEvent(result);

                    Input = "";

                    OnCommmented();
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                }
                finally
                {
                    IsCommenting = false;
                }
            }
        }

        public Action Commented { get; set; }

        private void OnCommmented()
        {
            Commented?.Invoke();
        }

        #endregion Comment

        #region Handlers

        private IDisposable _attendEventToken;

        public override void Attach()
        {
            base.Attach();

            Mvx.Resolve<ISignalRClient>().UpdateEvent += UpdateEvent;

            _attendEventToken = Mvx.Resolve<IMvxMessenger>().Subscribe<AttendEventMessage>(async (m) =>
            {
                await _eventService.AttendEventAsync(m.EventId, m.IsAttending);
            });
        }

        public override void Unattach()
        {
            base.Unattach();

            Mvx.Resolve<ISignalRClient>().UpdateEvent -= UpdateEvent;

            _attendEventToken?.Dispose();
        }

        #endregion Handlers

        #region Refresh

        protected override async Task RefreshAsync()
        {
            await Init(_id);
        }

        public string CommentButtonText => GetResource(ResKeys.comment_btn_post);

        #endregion Refresh
    }
}