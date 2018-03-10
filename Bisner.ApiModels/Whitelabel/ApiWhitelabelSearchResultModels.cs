using System.Collections.Generic;
using Bisner.ApiModels.Collaboration;

namespace Bisner.ApiModels.Whitelabel
{
    public class ApiWhitelabelSearchResultModel
    {
        private List<ApiWhitelabelPublicUserModel> _users;
        private List<ApiWhitelabelGroupModel> _groups;
        private List<ApiWhitelabelEventModel> _events;
        private List<ApiWhitelabelCompanyModel> _companies;
        private List<ApiFileModel> _files;
        private List<ApiNoteModel> _notes;
        private List<ApiTaskModel> _tasks;

        /// <summary>
        /// Users found matching the search string
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Users
        {
            get { return _users ?? (_users = new List<ApiWhitelabelPublicUserModel>()); }
            set { _users = value; }
        }

        /// <summary>
        /// Groups found matching the search string
        /// </summary>
        public List<ApiWhitelabelGroupModel> Groups
        {
            get { return _groups ?? (_groups = new List<ApiWhitelabelGroupModel>()); }
            set { _groups = value; }
        }

        /// <summary>
        /// Events found matching the search string
        /// </summary>
        public List<ApiWhitelabelEventModel> Events
        {
            get { return _events ?? (_events = new List<ApiWhitelabelEventModel>()); }
            set { _events = value; }
        }

        /// <summary>
        /// Companies matching the search string
        /// </summary>
        public List<ApiWhitelabelCompanyModel> Companies
        {
            get { return _companies ?? (_companies = new List<ApiWhitelabelCompanyModel>()); }
            set { _companies = value; }
        }

        /// <summary>
        /// Files that matched the search string if collaboration is enabled
        /// </summary>
        public List<ApiFileModel> Files
        {
            get { return _files ?? (_files = new List<ApiFileModel>()); }
            set { _files = value; }
        }

        /// <summary>
        /// Notes that matched the search string if collaboration is enabled
        /// </summary>
        public List<ApiNoteModel> Notes
        {
            get { return _notes ?? (_notes = new List<ApiNoteModel>()); }
            set { _notes = value; }
        }

        /// <summary>
        /// Notes that matched the search string if collaboration is enabled
        /// </summary>
        public List<ApiTaskModel> Tasks
        {
            get { return _tasks ?? (_tasks = new List<ApiTaskModel>()); }
            set { _tasks = value; }
        }

        public List<ApiWhitelabelFeedPostModel> Posts { get; set; } = new List<ApiWhitelabelFeedPostModel>();

        public List<ApiWhitelabelFeedPostModel> CollaboratePosts { get; set; } = new List<ApiWhitelabelFeedPostModel>();
    }
}