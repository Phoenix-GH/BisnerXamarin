using System;
using System.Collections.Generic;

namespace Bisner.Mobile.Core.Models.General.User
{
    public class PrivateUser : User, IPrivateUser
    {
        public Guid? LanguageId { get; set; }
        public string Email { get; set; }
        public bool CollaborationEnabled { get; set; }
        public List<Guid> CompanyIds { get; set; }
        public List<Guid> GuestCompanyIds { get; set; }
        public List<IUser> Contacts { get; set; }
        public bool DailyDigestMails { get; set; }
        public bool WeeklyDigestMails { get; set; }
        public bool MentionMails { get; set; }
        public bool ContactInvites { get; set; }
        public bool WorkspaceInviteMails { get; set; }
        public bool AssignedMails { get; set; }
    }
}