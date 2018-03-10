using System;
using System.Collections.Generic;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.ApiModels.Collaboration
{
    public class ApiFileModel
    {
        private List<ApiFileItemModel> _files;
        private List<ApiWhitelabelPublicUserModel> _assignedUsers;
        private List<ApiWhitelabelCommentModel> _comments;

        /// <summary>
        /// File id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// File parent id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Filename / title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// File description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Files + version
        /// </summary>
        public List<ApiFileItemModel> Files
        {
            get { return _files ?? (_files = new List<ApiFileItemModel>()); }
            set { _files = value; }
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
        /// Comments
        /// </summary>
        public List<ApiWhitelabelCommentModel> Comments
        {
            get { return _comments ?? ( _comments = new List<ApiWhitelabelCommentModel>()); }
            set { _comments = value; }
        }
    }

    public class ApiFileItemModel
    {
        /// <summary>
        /// File item id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// File extension
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// File contenttype
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File version
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// File size
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Upload datetime
        /// </summary>
        public DateTime UploadDateTime { get; set; }

        /// <summary>
        /// Uploaded by user id
        /// </summary>
        public ApiWhitelabelPublicUserModel User { get; set; }
    }
}