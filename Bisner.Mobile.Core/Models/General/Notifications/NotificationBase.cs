using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.General.Notifications
{
    public abstract class NotificationBase : ItemBase, INotification
    {
        private string _displayName;
        private string _avatarUrl;
        private Guid _userId;
        private Guid _relatedItemId;
        private DateTime _creationDateTime;
        private DateTime? _isReadOnDateTime;
        private bool _isRead;
        private string _text;
        private string _extraText;
        private string _notificationType;

        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; RaisePropertyChanged(() => UserId); }
        }

        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        public Guid RelatedItemId
        {
            get { return _relatedItemId; }
            set { _relatedItemId = value; RaisePropertyChanged(() => RelatedItemId); }
        }

        public string NotificationType
        {
            get { return _notificationType; }
            set { _notificationType = value; RaisePropertyChanged(() => NotificationType); }
        }

        public DateTime CreationDateTime
        {
            get { return _creationDateTime; }
            set { _creationDateTime = value; RaisePropertyChanged(() => CreationDateTime); }
        }

        public DateTime? IsReadOnDateTime
        {
            get { return _isReadOnDateTime; }
            set { _isReadOnDateTime = value; RaisePropertyChanged(() => IsReadOnDateTime); }
        }

        public bool IsRead
        {
            get { return _isRead; }
            set { _isRead = value; RaisePropertyChanged(() => IsRead); }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; RaisePropertyChanged(() => Text); }
        }

        public string ExtraText
        {
            get { return _extraText; }
            set { _extraText = value; RaisePropertyChanged(() => ExtraText); }
        }

        public virtual void ShowRelatedItem()
        {
            // Show!!
        }

        public virtual void Update(INotification notification)
        {
            Id = notification.Id;
            UserId = notification.UserId;
            DisplayName = notification.DisplayName;
            RelatedItemId = notification.RelatedItemId;
            Text = notification.Text;
            IsRead = notification.IsRead;
            AvatarUrl = notification.AvatarUrl;
            CreationDateTime = notification.CreationDateTime;
            IsReadOnDateTime = notification.IsReadOnDateTime;
            ExtraText = notification.ExtraText;
        }
    }
}