using System;
using System.Collections.Generic;
using Bisner.ApiModels.General;
using Bisner.ApiModels.Whitelabel;
using Bisner.Constants;

namespace Bisner.ApiModels.Collaboration
{
    public class WorkspaceNewPostCount
    {
        /// <summary>
        /// Workspace id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Number of new posts
        /// </summary>
        public long NewPostCount { get; set; }
    }

    public class WorkspaceUserInfo
    {
        /// <summary>
        /// User id of this info
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// User has this workspace as favorite
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Last workspace visit for user
        /// </summary>
        public DateTime UserLastVisit { get; set; }
    }

    public class ApiWorkspaceModelChanged
    {
        /// <summary>
        /// Workspace has changes
        /// </summary>
        public bool HasChanges { get; set; }

        /// <summary>
        /// Workspace model
        /// </summary>
        public ApiWorkspaceModel Workspace { get; set; }
    }
    public class ApiWorkspaceModel
    {
        /// <summary>
        /// Workspace id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Workspace name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Workspace description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Last workspace activity
        /// </summary>
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Workspace image
        /// </summary>
        public ApiImageModel Logo { get; set; }

        /// <summary>
        /// Parent id for this workspace
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Type workspace (Open or closed)
        /// </summary>
        public WorkspaceType Type { get; set; }

        /// <summary>
        /// Admin user ids
        /// </summary>
        public List<Guid> AdminIds { get; set; } = new List<Guid>();

        /// <summary>
        /// User ids
        /// </summary>
        public List<Guid> UserIds { get; set; } = new List<Guid>();
       
        /// <summary>
        /// List of pending users -> Users added but not accepted yet
        /// </summary>
        public List<Guid> PendingUserIds { get; set; }= new List<Guid>();
        
        /// <summary>
        /// User info
        /// </summary>
        public List<WorkspaceUserInfo> UserInfo { get; set; } = new List<WorkspaceUserInfo>();
       
        /// <summary>
        /// Workspace archiving
        /// </summary>
        public bool Archived { get; set; }
    }

    public class ApiWorkspaceDetailModel
    {
        private List<ApiTaskModel> _tasks;
        private List<ApiFileModel> _files;
        private List<ApiNoteModel> _notes;
        private List<ApiWhitelabelFeedPostModel> _posts;
        private List<ApiWhitelabelPublicUserModel> _users;

        /// <summary>
        /// Workspace id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Workspace
        /// </summary>
        public ApiWorkspaceModel Workspace { get; set; }

        /// <summary>
        /// Tasks in this workspace
        /// </summary>
        public List<ApiTaskModel> Tasks
        {
            get { return _tasks ?? (_tasks = new List<ApiTaskModel>()); }
            set { _tasks = value; }
        }

        /// <summary>
        /// Files in this workspace
        /// </summary>
        public List<ApiFileModel> Files
        {
            get { return _files ?? (_files = new List<ApiFileModel>()); }
            set { _files = value; }
        }

        /// <summary>
        /// Notes in this workspace
        /// </summary>
        public List<ApiNoteModel> Notes
        {
            get { return _notes ?? (_notes = new List<ApiNoteModel>()); }
            set { _notes = value; }
        }

        /// <summary>
        /// Feedposts in this workspace (Upto 25)
        /// </summary>
        public List<ApiWhitelabelFeedPostModel> Posts
        {
            get { return _posts ?? (_posts = new List<ApiWhitelabelFeedPostModel>()); }
            set { _posts = value; }
        }

        /// <summary>
        /// Company
        /// </summary>
        public ApiWhitelabelCompanyModel Company { get; set; }

        /// <summary>
        /// Workspace users
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Users
        {
            get { return _users ?? (_users = new List<ApiWhitelabelPublicUserModel>()); }
            set { _users = value; }
        }
    }

}