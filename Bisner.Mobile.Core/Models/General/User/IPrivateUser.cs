using System;
using System.Collections.Generic;

namespace Bisner.Mobile.Core.Models.General.User
{
    public interface IPrivateUser : IUser
    {
        /// <summary>
        /// User language setting
        /// </summary>
        Guid? LanguageId { get; set; }
        
        /// <summary>
        /// If true user has enabled collaboration sync
        /// </summary>
        bool CollaborationEnabled { get; set; }

        /// <summary>
        /// User company Ids
        /// </summary>
        List<Guid> CompanyIds { get; set; }

        /// <summary>
        /// User company Ids
        /// </summary>
        List<Guid> GuestCompanyIds { get; set; }

        /// <summary>
        /// User contacts
        /// </summary>
        List<IUser> Contacts { get; set; }

        /// <summary>
        /// User can recieve a daily digest mail
        /// </summary>
        bool DailyDigestMails { get; set; }

        /// <summary>
        /// User can recieve a weekly digest mail
        /// </summary>
        bool WeeklyDigestMails { get; set; }

        /// <summary>
        /// User can recieve mention mails (send immediatly)
        /// </summary>
        bool MentionMails { get; set; }

        /// <summary>
        /// Contact invite mails
        /// </summary>
        bool ContactInvites { get; set; }

        /// <summary>
        /// User can recieve workspace invite mails (send immediatly)
        /// </summary>
        bool WorkspaceInviteMails { get; set; }

        /// <summary>
        /// Usre can recieve mails when assigned to file / note / task
        /// </summary>
        bool AssignedMails { get; set; }
    }
}