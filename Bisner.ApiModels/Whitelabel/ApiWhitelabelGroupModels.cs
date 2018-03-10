using System;
using System.Collections.Generic;
using Bisner.ApiModels.General;

namespace Bisner.ApiModels.Whitelabel
{

    public enum GroupType
    {
        /// <summary>
        /// Default open group
        /// </summary>
        Open = 0,

        /// <summary>
        /// Private group -> Can be found but user needs to be accepted to get access
        /// </summary>
        Private = 1,

        /// <summary>
        /// Hidden for everyone -> Users need to recieve an invite or be space admin
        /// </summary>
        Hidden = 2
    }

    public class ApiWhitelabelGroupModel
    {
        private List<Guid> _userIds;
        private List<Guid> _adminIds;
        private List<Guid> _invitedMemberIds;
        private List<Guid> _pendingMemberIds;
        private List<ApiWhitelabelPublicUserModel> _users;
        private List<ApiWhitelabelPublicUserModel> _pendingMembers;
        private List<ApiWhitelabelPublicUserModel> _admins;
        private List<ApiWhitelabelPublicUserModel> _invitedMembers;
        private List<ApiWhitelabelGroupModel> _subgroups;

        /// <summary>
        /// Group id
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// Group parent id / guid.empty if main group
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// /Group location Id
        /// </summary>
        public Guid? LocationId { get; set; }

        /// <summary>
        /// Group type
        /// </summary>
        public GroupType GroupType { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Group description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Introduction text for the index
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// Group tags
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// List of id's
        /// </summary>
        public List<Guid> UserIds
        {
            get { return _userIds ?? (_userIds = new List<Guid>()); }
            set { _userIds = value; }
        }

        /// <summary>
        /// List of admin ids
        /// </summary>
        public List<Guid> AdminIds
        {
            get { return _adminIds ?? (_adminIds = new List<Guid>()); }
            set { _adminIds = value; }
        }

        /// <summary>
        /// List of admin ids
        /// </summary>
        public List<Guid> PendingMemberIds
        {
            get { return _pendingMemberIds ?? (_pendingMemberIds = new List<Guid>()); }
            set { _pendingMemberIds = value; }
        }

        /// <summary>
        /// Invited member id's but members have not yet accepted
        /// </summary>
        public List<Guid> InvitedMemberIds
        {
            get { return _invitedMemberIds ?? (_invitedMemberIds = new List<Guid>()); }
            set { _invitedMemberIds = value; }
        }

        /// <summary>
        /// Users in this group
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Users
        {
            get { return _users ?? (_users = new List<ApiWhitelabelPublicUserModel>()); }
            set { _users = value; }
        }

        /// <summary>
        /// Pending users in this group
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> PendingMembers
        {
            get { return _pendingMembers ?? (_pendingMembers = new List<ApiWhitelabelPublicUserModel>()); }
            set { _pendingMembers = value; }
        }

        /// <summary>
        /// Admins in this group
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Admins
        {
            get { return _admins ?? (_admins = new List<ApiWhitelabelPublicUserModel>()); }
            set { _admins = value; }
        }

        /// <summary>
        /// Admins in this group
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> InvitedMembers
        {
            get { return _invitedMembers ?? (_invitedMembers = new List<ApiWhitelabelPublicUserModel>()); }
            set { _invitedMembers = value; }
        }

        /// <summary>
        /// Subgroups
        /// </summary>
        public List<ApiWhitelabelGroupModel> SubGroups
        {
            get { return _subgroups ?? (_subgroups = new List<ApiWhitelabelGroupModel>()); }
            set { _subgroups = value; }
        }

        /// <summary>
        /// Group image
        /// </summary>
        public ApiImageModel Image { get; set; }

        /// <summary>
        /// Group header
        /// </summary>
        public ApiImageModel Header { get; set; }

        public bool CanSeeGroup(Guid userId, bool isPlatformAdmin = false)
        {
            // Platform admins can always see all groups
            if (isPlatformAdmin)
            {
                return true;
            }

            // Open and private groups can be seen by everyone
            if (GroupType == GroupType.Open || GroupType == GroupType.Private)
            {
                return true;
            }

            // Hidden groups by admins and users of that group and platform admins
            if (GroupType == GroupType.Hidden)
            {
                if (AdminIds != null && AdminIds.Contains(userId))
                {
                    return true;
                }
                if (UserIds != null && UserIds.Contains(userId))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanEnterGroup(Guid userId, bool isPlatformAdmin = false)
        {
            // Platform admins can always enter all groups
            if (isPlatformAdmin)
            {
                return true;
            }

            // Open groups can be entered by everyone
            if (GroupType == GroupType.Open)
            {
                return true;
            }

            // Hidden and private groups by admins and users of that group and platform admins
            if (GroupType == GroupType.Hidden || GroupType == GroupType.Private)
            {
                if (AdminIds != null && AdminIds.Contains(userId))
                {
                    return true;
                }
                if (UserIds != null && UserIds.Contains(userId))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAdmin(Guid userId, bool isPlatformAdmin = false)
        {
            // Platform admins can always enter all groups
            if (isPlatformAdmin)
            {
                return true;
            }

            if (AdminIds != null && AdminIds.Contains(userId))
            {
                return true;
            }



            return false;
        }
    }
}