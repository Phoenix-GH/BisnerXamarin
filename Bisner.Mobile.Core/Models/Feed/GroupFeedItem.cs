using System;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.Models.Feed
{
    public class GroupFeedItem : ItemBase, IFeedItem
    {
        #region Constructor

        private string _name;
        private ApiWhitelabelGroupModel _group;
        private DateTime _dateTime;
        private string _headerUrl;
        private string _imageUrl;
        private string _description;
        private MvxCommand _joinCommand;
        private string _joinButtonText;
        private bool _hasJoined;
        private bool _isJoining;
        private bool _isNotJoining;

        public GroupFeedItem(ApiWhitelabelGroupModel group)
        {
            IsJoining = false;
            Group = group;
            if (group != null)
            {
                Id = group.Id;
                Name = group.Name;
                HeaderUrl = group.Header != null ? group.Header.Medium : Defaults.EventHeaderDefaultString;
                ImageUrl = group.Image != null ? group.Image.Medium : Defaults.GroupHeaderDefault;
                Description = group.Description;
                HasJoined = group.UserIds.Contains(Settings.UserId);
                DateTime = DateTime.MaxValue;
            }
        }

        #endregion Constructor

        #region Group

        public ApiWhitelabelGroupModel Group
        {
            get => _group;
            private set { _group = value; RaisePropertyChanged(() => Group); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        public DateTime DateTime
        {
            get => _dateTime;
            set { _dateTime = value; RaisePropertyChanged(() => DateTime); }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        public string HeaderUrl
        {
            get => _headerUrl;
            set { _headerUrl = value; RaisePropertyChanged(() => HeaderUrl); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; RaisePropertyChanged(() => Description); }
        }

        #endregion Group

        #region Join

        public bool HasJoined
        {
            get => _hasJoined;
            set
            {
                _hasJoined = value;
                SetJoinButtonText();
                RaisePropertyChanged(() => HasJoined);
            }
        }

        public bool IsJoining
        {
            get => _isJoining;
            private set
            {
                IsNotJoining = !value;
                _isJoining = value; RaisePropertyChanged(() => IsJoining);
            }
        }

        public bool IsNotJoining
        {
            get => _isNotJoining;
            private set { _isNotJoining = value; RaisePropertyChanged(() => IsNotJoining); }
        }

        public MvxCommand JoinCommand
        {
            get { return _joinCommand ?? (_joinCommand = new MvxCommand(async () => await Join())); }
        }

        public string JoinButtonText
        {
            get => _joinButtonText;
            private set { _joinButtonText = value; RaisePropertyChanged(() => JoinButtonText); }
        }

        private async Task Join()
        {
            if (!IsJoining)
            {
                IsJoining = true;

                HasJoined = !HasJoined;

                try
                {
                    if (HasJoined)
                    {
                        await Mvx.Resolve<IGroupService>().JoinGroupAsync(Id);
                    }
                    else
                    {
                        await Mvx.Resolve<IGroupService>().LeaveGroupAsync(Id);
                    }

                    Mvx.Resolve<IMvxMessenger>().Publish(new GroupJoinedMessage(this) { GroupId = Id, HasJoined = HasJoined });
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                    HasJoined = !HasJoined;
                }
                finally
                {
                    IsJoining = false;
                }
            }
        }

        private void SetJoinButtonText()
        {
            JoinButtonText = HasJoined ? Settings.GetResource(ResKeys.mobile_groups_btn_unjoin_group) : Settings.GetResource(ResKeys.mobile_groups_btn_join_group);
        }

        #endregion Join
    }
}