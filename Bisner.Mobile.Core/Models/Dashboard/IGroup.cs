using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.Models.Dashboard
{
    public class Group : ItemBase, IGroup
    {
        #region Variables

        private MvxCommand _showGroupCommand;
        private List<Guid> _userIds;
        private List<Guid> _adminIds;
        private List<Guid> _invitedMemberIds;
        private List<IUser> _users;
        private List<IUser> _admins;
        private List<IUser> _invitedMembers;

        #endregion Variables

        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Guid> UserIds
        {
            get { return _userIds ?? (_userIds = new List<Guid>()); }
            set { _userIds = value; }
        }

        public List<Guid> AdminIds
        {
            get { return _adminIds ?? (_adminIds = new List<Guid>()); }
            set { _adminIds = value; }
        }

        public List<Guid> InvitedMemberIds
        {
            get { return _invitedMemberIds ?? (_invitedMemberIds = new List<Guid>()); }
            set { _invitedMemberIds = value; }
        }

        public List<IUser> Users
        {
            get { return _users ?? (_users = new List<IUser>()); }
            set { _users = value; }
        }

        public List<IUser> Admins
        {
            get { return _admins ?? (_admins = new List<IUser>()); }
            set { _admins = value; }
        }

        public List<IUser> InvitedMembers
        {
            get { return _invitedMembers ?? (_invitedMembers = new List<IUser>()); }
            set { _invitedMembers = value; }
        }

        public IImage Image { get; set; }

        public IImage Header { get; set; }

        public string HeaderUrl => Header?.Large;

        #endregion Properties

        #region Show

        public string ButtonText => Settings.GetResource(ResKeys.mobile_groups_index_btn_viewgroup);

        public MvxCommand ShowGroupCommand => _showGroupCommand ?? (_showGroupCommand = new MvxCommand(Show));

        private void Show()
        {
            ShowViewModel<FeedViewModel>(new { id = Id, feedType = FeedType.Group });
        }

        #endregion Show
    }

    public interface IGroup : IItemBase
    {
        /// <summary>
        /// Group name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Group description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// List of id's
        /// </summary>
        List<Guid> UserIds { get; set; }

        /// <summary>
        /// List of admin ids
        /// </summary>
        List<Guid> AdminIds { get; set; }

        /// <summary>
        /// Invited member id's but members have not yet accepted
        /// </summary>
        List<Guid> InvitedMemberIds { get; set; }

        /// <summary>
        /// Users in this group
        /// </summary>
        List<IUser> Users { get; set; }

        /// <summary>
        /// Admins in this group
        /// </summary>
        List<IUser> Admins { get; set; }

        /// <summary>
        /// Admins in this group
        /// </summary>
        List<IUser> InvitedMembers { get; set; }

        /// <summary>
        /// Group image
        /// </summary>
        IImage Image { get; set; }

        /// <summary>
        /// Group header
        /// </summary>
        IImage Header { get; set; }

        MvxCommand ShowGroupCommand { get; }

        string ButtonText { get; }
    }
}
