using System;
using System.Collections.Generic;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.ApiModels.Collaboration
{
    public class ApiNoteModel
    {
        private List<ApiWhitelabelCommentModel> _comments;
        private List<ApiWhitelabelPublicUserModel> _assignedUsers;

        /// <summary>
        /// Note id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Parent id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Note title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Note text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Last edited by user id
        /// </summary>
        public ApiWhitelabelPublicUserModel LastEditedByUser { get; set; }

        /// <summary>
        /// Last edited on datetime
        /// </summary>
        public DateTime LastEditedOnDateTime { get; set; }

        /// <summary>
        /// Comments on this task
        /// </summary>
        public List<ApiWhitelabelCommentModel> Comments
        {
            get { return _comments ?? (_comments = new List<ApiWhitelabelCommentModel>()); }
            set { _comments = value; }
        }

        /// <summary>
        /// Wallpost related to this note
        /// </summary>
        public Guid RelatedWallPostId { get; set; }

        /// <summary>
        /// If not Guid.Empty this note is locked / edit mode
        /// </summary>
        public Guid LockedByUserId { get; set; }

        /// <summary>
        /// Assigned users to this file
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> AssignedUsers
        {
            get { return _assignedUsers ?? (_assignedUsers = new List<ApiWhitelabelPublicUserModel>()); }
            set { _assignedUsers = value; }
        }
    }
}