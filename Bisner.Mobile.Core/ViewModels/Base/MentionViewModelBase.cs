using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.Service;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Base
{
    public abstract class MentionViewModelBase : BaseViewModel
    {
        #region Constructor

        private readonly IUserService _userService;

        private const string RegexPattern = @"(@)((?:[A-Za-z0-9-_]*))";

        protected MentionViewModelBase(IPlatformService platformService, IUserService userService) : base(platformService)
        {
            _userService = userService;
            MentionUsers = new List<MentionUser>();

            MentionCommand = new MvxAsyncCommand(Mention);
        }

        #endregion Constructor

        #region Mentions

        private bool _showMentions;
        private List<MentionUser> _mentionUsers;

        protected async Task CheckMentions(string input)
        {
            var match = Regex.Match(input, RegexPattern, RegexOptions.RightToLeft);

            if (match.Length > 0 && input.EndsWith(match.Value))
            {
                // Start mentions
                if (!ShowMentions)
                {
                    ShowMentions = true;
                }

                MentionUsers = await GetUsersForMention(match.Value);

                if (!MentionUsers.Any())
                {
                    ShowMentions = false;
                }
            }
            else
            {
                if (ShowMentions)
                {
                    ShowMentions = false;
                }
            }
        }

        public bool ShowMentions
        {
            get => _showMentions;
            set
            {
                _showMentions = value;
                RaisePropertyChanged(() => ShowMentions);

                if (value)
                {
                    Execute(OnShowMentions, action => action());
                }
                else
                {
                    Execute(OnHideMentions, action => action());
                }
            }
        }

        public List<MentionUser> MentionUsers
        {
            get => _mentionUsers;
            set { _mentionUsers = value; RaisePropertyChanged(() => MentionUsers); }
        }

        public Action OnShowMentions { get; set; }

        public Action OnHideMentions { get; set; }

        private async Task<List<MentionUser>> GetUsersForMention(string filter)
        {
            var userModels = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);

            var users = new List<IUser>();

            foreach (var apiWhitelabelPublicUserModel in userModels)
            {
                users.Add(apiWhitelabelPublicUserModel.ToModel());
            }

            // Remove own user
            users.RemoveAll(u => u.Id == Settings.UserId);

            users.Insert(0, new User
            {
                Id = Guid.Empty,
                DisplayName = "All",
            });

            var mentionUsers = new List<MentionUser>();

            var nameFilter = filter.Remove(0, 1);

            var foundUsers = users.Where(u => u.DisplayName.ToLower().StartsWith(nameFilter.ToLower()));

            foreach (var user in foundUsers.Take(10))
            {
                mentionUsers.Add(new MentionUser(user));
            }

            return mentionUsers.OrderBy(u => u.User.DisplayName).ToList();
        }

        /// <summary>
        /// Is executed when a mention has been selected
        /// </summary>
        protected Action<string> OnMentionSelected { get; set; }

        public void MentionSelected(MentionUser user)
        {
            Execute(OnMentionSelected, action => action(user.MentionName));

            if (MentionsProperty != null)
            {
                var expr = (MemberExpression)MentionsProperty.Body;
                var prop = (PropertyInfo)expr.Member;

                var currentString = prop.GetValue(this).ToString();

                var regex = new Regex(RegexPattern, RegexOptions.RightToLeft);

                currentString = regex.Replace(currentString, "@" + user.MentionName.ToLower(), 1);

                prop.SetValue(this, currentString, null);
            }

            MentionedUserIds.Add(user.User.Id);
        }

        protected Expression<Func<string>> MentionsProperty { get; set; }

        /// <summary>
        /// The user id's of the users that were mentioned in MentionsProperty
        /// </summary>
        protected readonly List<Guid> MentionedUserIds = new List<Guid>();

        public ICommand MentionCommand { get; }

        private async Task Mention()
        {
            if (MentionsProperty != null)
            {
                var expr = (MemberExpression)MentionsProperty.Body;
                var prop = (PropertyInfo)expr.Member;

                var currentString = prop.GetValue(this).ToString();

                if (!currentString.EndsWith("@"))
                {
                    currentString += "@";
                    prop.SetValue(this, currentString);
                }

                await CheckMentions(currentString);
            }
        }

        #endregion Mentions
    }

    public class MentionUser
    {
        public MentionUser(IUser user)
        {
            User = user;
        }

        public IUser User { get; }

        public string MentionName => User.DisplayName.Replace(' ', '_');
    }
}