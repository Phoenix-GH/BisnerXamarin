using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Models.General.User;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Events
{
    public interface IEvent : IItemBase, IDisplayUser
    {
        /// <summary>
        /// Event category id
        /// </summary>
        Guid CategoryId { get; set; }

        /// <summary>
        /// Is published
        /// </summary>
        bool IsPublished { get; set; }


        /// <summary>
        /// Post id
        /// </summary>
        Guid LinkedPostedId { get; set; }

        /// <summary>
        /// Parent id
        /// </summary>
        Guid ParentId { get; set; }


        /// <summary>
        /// Event title
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Subtitle
        /// </summary>
        string SubTitle { get; set; }


        /// <summary>
        /// Event details
        /// </summary>
        string Details { get; set; }

        /// <summary>
        /// Event Summary
        /// </summary>
        string Summary { get; set; }

        /// <summary>
        /// Ids of attendees
        /// </summary>
        List<Guid> AttendeesIds { get; set; }

        /// <summary>
        /// Datetime of the event
        /// </summary>
        DateTime DateTime { get; set; }

        string DateTimeText { get; }

        /// <summary>
        /// Creation datetime
        /// </summary>
        DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Location string
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// Images
        /// </summary>
        List<IImage> Images { get; set; }

        /// <summary>
        /// Event logo
        /// </summary>
        IImage Logo { get; set; }

        /// <summary>
        /// Event header
        /// </summary>
        IImage Header { get; set; }

        List<IUser> Attendees { get; set; }

        #region Attend

        /// <summary>
        /// Indicates if you are attending this event
        /// </summary>
        bool IsAttending { get; }

        string IsAttendingString { get; }

        string NumberAttendingString { get; }

        #endregion Attend

        #region Command

        MvxCommand ShowEventCommand { get; }

        MvxCommand AttendCommand { get; }

        #endregion Command

        #region Resources

        string EventInfoHeaderText { get; }

        string EventDateLabel { get; }

        string EventTimeLabel { get; }

        string EventLocationLabel { get; }

        string AboutHeaderLabel { get; }

        #endregion Resources
    }
}