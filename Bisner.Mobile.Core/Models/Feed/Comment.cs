using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.Models.Feed
{
    public class Comment : MvxNavigatingObject, IComment
    {
        #region Constructor

        private string _avatarUrl;
        private string _displayName;
        private string _text;
        private DateTime _dateTime;
        private ICommand _reportCommand;

        #endregion Constructor

        #region Properties

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string AvatarUrl
        {
            get => _avatarUrl;
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        public string Text
        {
            get => _text;
            set { _text = value; RaisePropertyChanged(() => Text); }
        }

        public DateTime DateTime
        {
            get => _dateTime;
            set { _dateTime = value; RaisePropertyChanged(() => DateTime); }
        }

        #endregion Properties

        #region Functions

        public void Update(IComment item)
        {
            if (item.Id != Id)
            {
                Debug.WriteLine("You are trying to update a comment with id {0} with values of a comment with id {1}", Id, item.Id);
                return;
            }

            AvatarUrl = item.AvatarUrl;
            DisplayName = item.DisplayName;
            Text = item.Text;
            DateTime = item.DateTime;
        }

        public ICommand ReportCommand => _reportCommand ?? (_reportCommand = new MvxAsyncCommand(ReportComment));

        public async Task ReportComment()
        {
            //ReportCommandAction?.Invoke(Id);

            try
            {
                await Mvx.Resolve<IFeedService>().ReportCommentAsync(Id);
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);

                await Mvx.Resolve<IUserDialogs>().AlertAsync(Settings.GetResource(ResKeys.mobile_error_server_error));
            }
        }

        public Action<Guid> ReportCommandAction;

        private MvxCommand _userCommand;
        public MvxCommand UserCommand => _userCommand ?? (_userCommand = new MvxCommand(ShowUser));

        private void ShowUser()
        {
            ShowViewModel<UserViewModel>(new { userId = UserId, enableChat = true });
        }

        #endregion Functions
    }
}