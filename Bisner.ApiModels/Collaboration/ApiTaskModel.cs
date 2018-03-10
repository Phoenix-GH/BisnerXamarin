using System;
using System.Collections.Generic;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.ApiModels.Collaboration
{
    public class ApiTaskModel
    {
        private List<ApiWhitelabelCommentModel> _comments;
        private List<ApiWhitelabelPublicUserModel> _assignedUsers;
        private List<Guid> _subTaskIds;
        private List<ApiTaskModel> _subTasks;
        private List<Guid> _followers;

        /// <summary>
        /// Task Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Parent ID (Workspace)
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Created by user ID
        /// </summary>
        public ApiWhitelabelPublicUserModel CreatedByUser { get; set; }

        /// <summary>
        /// Task order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Task Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Task description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Creationdatetime
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Last updated datetime
        /// </summary>
        public DateTime LastUpdateDateTime { get; set; }

        /// <summary>
        /// Task completed on datetime
        /// </summary>
        public DateTime? FinishedDateTime { get; set; }

        /// <summary>
        /// Task start datetime
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Task due datetime
        /// </summary>
        public DateTime? DueDateTime { get; set; }

        /// <summary>
        /// Task status
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Task priority
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Comment count
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Comments on this task
        /// </summary>
        public List<ApiWhitelabelCommentModel> Comments
        {
            get { return _comments ?? (_comments = new List<ApiWhitelabelCommentModel>()); }
            set { _comments = value; }
        }

        /// <summary>
        /// Assigned users to this file
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> AssignedUsers
        {
            get { return _assignedUsers ?? (_assignedUsers = new List<ApiWhitelabelPublicUserModel>()); }
            set { _assignedUsers = value; }
        }

        /// <summary>
        /// Related wallposts 
        /// </summary>
        public Guid RelatedWallPostId { get; set; }

        /// <summary>
        /// Ids of the subtasks
        /// </summary>
        public List<Guid> SubTaskIds
        {
            get { return _subTaskIds ?? (_subTaskIds = new List<Guid>()); }
            set { _subTaskIds = value; }
        }

        /// <summary>
        /// Subtasks
        /// </summary>
        public List<ApiTaskModel> SubTasks
        {
            get { return _subTasks ?? (_subTasks = new List<ApiTaskModel>()); }
            set { _subTasks = value; }
        }

        /// <summary>
        /// Followers on task
        /// </summary>
        public List<Guid> Followers
        {
            get { return _followers ?? (_followers = new List<Guid>()); }
            set { _followers = value; }
        }
    }

    public enum TaskStatus
    {
        Open = 1,
        InProgress = 2,
        Closed = 4
    }

    public enum TaskPriority
    {
        High = 1,
        Normal = 2,
        Low = 4
    }

    public class ApiCreateTaskModel
    {
        /// <summary>
        /// Parent id (workspace id)
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// User id creating the task
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Task title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Task description
        /// </summary>
        public string Description { get; set; }
    }
}