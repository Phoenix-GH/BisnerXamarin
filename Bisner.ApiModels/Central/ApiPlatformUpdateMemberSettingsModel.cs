using System;

namespace Bisner.ApiModels.Central
{
    public class ApiPlatformUpdateMemberSettingsModel
    {
        public Guid Id { get; set; }

        public bool AllowChatBetweenAllUsers { get; set; }
        
        public bool AllowUsersToInviteMembers { get; set; }

        public bool AllowUserToUserInvite { get; set; }

        public bool DisableUsersPostingMainFeed { get; set; }

        public bool UseFreeSkills { get; set; }

        public bool UsePredefinedSkills { get; set; }

        public string PrefilledSkills { get; set; }
    }
}