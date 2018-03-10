using System.Collections.Generic;
using Bisner.ApiModels.Collaboration;

namespace Bisner.ApiModels.Whitelabel
{
    public class ApiRelatedItemsModel
    {
        private List<ApiWhitelabelEventModel> _events;
        private List<ApiFileModel> _files;
        private List<ApiNoteModel> _notes;
        private List<ApiTaskModel> _tasks;
        private List<ApiWhitelabelFeedPostModel> _posts;
        private List<ApiWorkspaceModel> _workspaces;
        private List<ApiWhitelabelGroupModel> _groups;

        /// <summary>
        /// Events
        /// </summary>
        public List<ApiWhitelabelEventModel> Events
        {
            get { return _events ?? (_events = new List<ApiWhitelabelEventModel>()); }
            set { _events = value; }
        }

        public List<ApiWhitelabelGroupModel> Groups
        {
            get { return _groups ?? (_groups = new List<ApiWhitelabelGroupModel>()); }
            set { _groups = value; }
        }

        /// <summary>
        /// Files
        /// </summary>
        public List<ApiFileModel> Files
        {
            get { return _files ?? (_files = new List<ApiFileModel>()); }
            set { _files = value; }
        }

        /// <summary>
        /// Notes
        /// </summary>
        public List<ApiNoteModel> Notes
        {
            get { return _notes ?? (_notes = new List<ApiNoteModel>()); }
            set { _notes = value; }
        }

        /// <summary>
        /// Tasks
        /// </summary>
        public List<ApiTaskModel> Tasks
        {
            get { return _tasks ?? (_tasks = new List<ApiTaskModel>()); }
            set { _tasks = value; }
        }

        /// <summary>
        /// Feed posts
        /// </summary>
        public List<ApiWhitelabelFeedPostModel> Posts
        {
            get { return _posts ?? (_posts = new List<ApiWhitelabelFeedPostModel>()); }
            set { _posts = value; }
        }

        /// <summary>
        /// Workspaces of the files/notes/tasks
        /// </summary>
        public List<ApiWorkspaceModel> Workspaces
        {
            get { return _workspaces ?? (_workspaces = new List<ApiWorkspaceModel>()); }
            set { _workspaces = value; }
        }
    }
}