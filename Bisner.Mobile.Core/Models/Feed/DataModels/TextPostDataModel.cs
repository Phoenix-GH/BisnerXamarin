using System;

namespace Bisner.Mobile.Core.Models.Feed.DataModels
{
    /// <summary>
    /// General data model to deserialize all notification data models (should contain all possible data fields)
    /// </summary>
    public class FeedPostDataModel
    {
        public string Text { get; set; }

        /// <summary>
        /// ID of the event
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// ID of the task
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Group id if provided
        /// </summary>
        public Guid GroupId { get; set; }
    }
}