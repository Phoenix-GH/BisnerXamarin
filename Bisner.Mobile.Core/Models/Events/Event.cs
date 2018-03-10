using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Extensions;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Members;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using ApiPriority = Bisner.Mobile.Core.Communication.ApiPriority;

namespace Bisner.Mobile.Core.Models.Events
{
    public class Event : BusyItemBase, IEvent
    {
        #region Constructor

        private Guid _categoryId;
        private bool _isPublished;
        private Guid _linkedPostedId;
        private Guid _parentId;
        private string _title;
        private string _subTitle;
        private string _details;
        private string _summary;
        private List<Guid> _attendeesIds;
        private DateTime _dateTime;
        private DateTime _creationDateTime;
        private string _location;
        private List<IImage> _images;
        private IImage _logo;
        private IImage _header;
        private bool _isAttending;
        private bool _hasAttendees;
        private string _isAttendingString;
        private List<IUser> _attendees;
        private MvxCommand _showEventCommand;
        private bool _hasCommented;
        private string _commentButtonText;
        private MvxCommand _attendCommand;
        private bool _hasAbout;

        #region Resources

        private readonly string _eventClosedText;
        private readonly string _unattendButtonText;
        private readonly string _attendButtonText;
        private readonly string _peopleAttendingText;

        #endregion Resources

        public Event(string eventClosedText, string unattendButtonText, string attendButtonText, string peopleAttendingText, string eventInfoText, string eventDateText, string eventTimeLabel, string eventLocationLabel, string aboutHeaderLabel)
        {
            _eventClosedText = eventClosedText;
            _unattendButtonText = unattendButtonText;
            _attendButtonText = attendButtonText;
            _peopleAttendingText = peopleAttendingText;
            EventInfoHeaderText = eventInfoText;
            EventDateLabel = eventDateText;
            EventTimeLabel = eventTimeLabel;
            EventLocationLabel = eventLocationLabel;
            AboutHeaderLabel = aboutHeaderLabel;
        }

        #endregion Constructor

        #region Properties

        public Guid CategoryId
        {
            get => _categoryId;
            set { _categoryId = value; RaisePropertyChanged(() => CategoryId); }
        }

        public bool IsPublished
        {
            get => _isPublished;
            set { _isPublished = value; RaisePropertyChanged(() => IsPublished); }
        }

        public Guid LinkedPostedId
        {
            get => _linkedPostedId;
            set { _linkedPostedId = value; RaisePropertyChanged(() => LinkedPostedId); }
        }

        public Guid ParentId
        {
            get => _parentId;
            set { _parentId = value; RaisePropertyChanged(() => ParentId); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        public string SubTitle
        {
            get => _subTitle;
            set { _subTitle = value; RaisePropertyChanged(() => SubTitle); }
        }

        public string Details
        {
            get => _details;
            set { _details = value; RaisePropertyChanged(() => Details); }
        }

        public string Summary
        {
            get => _summary;
            set
            {
                _summary = value; RaisePropertyChanged(() => Summary);
                HasAbout = !string.IsNullOrWhiteSpace(value);
            }
        }

        public List<Guid> AttendeesIds
        {
            get => _attendeesIds ?? (_attendeesIds = new List<Guid>());
            set
            {
                _attendeesIds = value; RaisePropertyChanged(() => AttendeesIds);
                IsAttending = AttendeesIds.Contains(Settings.UserId);
                RaisePropertyChanged(() => NumberAttendingString);
            }
        }

        public DateTime DateTime
        {
            get => _dateTime;
            set { _dateTime = value; RaisePropertyChanged(() => DateTime); }
        }

        private string _dateTimeText;
        public string DateTimeText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_dateTimeText))
                {
                    _dateTimeText = DateTime.ToChatTime();
                }

                return _dateTimeText;
            }
        }

        public DateTime CreationDateTime
        {
            get => _creationDateTime;
            set { _creationDateTime = value; RaisePropertyChanged(() => CreationDateTime); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; RaisePropertyChanged(() => Location); }
        }

        public List<IImage> Images
        {
            get => _images;
            set { _images = value; RaisePropertyChanged(() => Images); }
        }

        public IImage Logo
        {
            get => _logo ?? (_logo = new Image
            {
                Large = Defaults.EventHeaderDefaultString,
                Medium = Defaults.EventHeaderDefaultString,
                Small = Defaults.EventHeaderDefaultString,
            });
            set { _logo = value; RaisePropertyChanged(() => Logo); }
        }

        public string LogoUrl => Logo?.Small;

        public IImage Header
        {
            get => _header ?? (_header = new Image
            {
                Large = Defaults.EventHeaderDefaultString,
                Medium = Defaults.EventHeaderDefaultString,
                Small = Defaults.EventHeaderDefaultString,
            });
            set { _header = value; RaisePropertyChanged(() => Header); }
        }

        public string HeaderUrl => Header?.Medium;

        public List<IUser> Attendees
        {
            get => _attendees;
            set
            {
                _attendees = value;
                RaisePropertyChanged(() => Attendees);
                RaisePropertyChanged(() => HasAttendees);
            }
        }

        public bool HasAttendees
        {
            get => _hasAttendees;
            set { _hasAttendees = value; RaisePropertyChanged(() => HasAttendees); }
        }

        #endregion Properties

        #region Attend

        public bool IsAttending
        {
            get => _isAttending;
            private set
            {
                _isAttending = value; RaisePropertyChanged(() => IsAttending);

                string text;

                if (DateTime < DateTime.UtcNow)
                    text = _eventClosedText;
                else
                    text = value ? _unattendButtonText : _attendButtonText;

                IsAttendingString = text;
            }
        }

        public string IsAttendingString
        {
            get => _isAttendingString;
            private set { _isAttendingString = value; RaisePropertyChanged(() => IsAttendingString); }
        }

        // TODO : Brackets in resource, see trello
        public string NumberAttendingString => AttendeesIds.Count + "" + _peopleAttendingText;

        public MvxCommand AttendCommand => _attendCommand ?? (_attendCommand = new MvxCommand(async () => await Attend()));

        private async Task Attend()
        {
            if (IsAttending)
            {
                var newList = new List<IUser>(Attendees);
                newList.RemoveAll(a => a.Id == Settings.UserId);

                Attendees = newList;

                AttendeesIds.Remove(Settings.UserId);
                IsAttending = false;
            }
            else
            {
                AttendeesIds.Add(Settings.UserId);

                var ownUser = await Mvx.Resolve<IUserService>().GetPersonalModelAsync(ApiPriority.UserInitiated);

                if (ownUser != null)
                {
                    var newList = new List<IUser>(Attendees) { ownUser.ToModel() };

                    Attendees = newList;
                }

                IsAttending = true;
            }

            RaisePropertyChanged(() => AttendeesIds);
            RaisePropertyChanged(() => IsAttendingString);
            RaisePropertyChanged(() => Attendees);
            RaisePropertyChanged(() => NumberAttendingString);

            Mvx.Resolve<IMvxMessenger>().Publish(new AttendEventMessage(this) { EventId = Id, IsAttending = IsAttending });
        }


        #endregion Attend

        #region DisplayUser

        public Guid UserId { get; set; }

        public string AvatarUrl { get; set; }

        public string DisplayName { get; set; }

        #endregion DisplayUser

        #region Command

        public MvxCommand ShowEventCommand => _showEventCommand ?? (_showEventCommand = new MvxCommand(Show));

        private void Show()
        {
            ShowViewModel<EventViewModel>(new { id = Id });
        }

        #endregion Command

        #region Comment

        public bool HasAbout
        {
            get => _hasAbout;
            set { _hasAbout = value; RaisePropertyChanged(() => HasAbout); }
        }

        // Only used in event detail page
        public bool HasCommented
        {
            get => _hasCommented;
            set { _hasCommented = value; RaisePropertyChanged(() => HasCommented); }
        }

        // Only used in event detail page
        public string CommentButtonText
        {
            get => _commentButtonText;
            set { _commentButtonText = value; RaisePropertyChanged(() => CommentButtonText); }
        }

        #endregion Comment

        #region Resources

        public string EventInfoHeaderText { get; }

        public string EventDateLabel { get; }

        public string EventTimeLabel { get; }

        public string EventLocationLabel { get; }

        public string AboutHeaderLabel { get; }

        #endregion Resources
    }
}
