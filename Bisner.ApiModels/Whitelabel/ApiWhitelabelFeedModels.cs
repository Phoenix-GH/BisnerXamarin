using System;
using System.Collections.Generic;
using Bisner.ApiModels.General;

namespace Bisner.ApiModels.Whitelabel
{
    public enum ParentType
    {
        Unknown = 0,
        Group = 1,
        Workspace = 2,
        Company = 3
    }

    public class ApiWhitelabelFeedPostModel
    {
        private List<ApiWhitelabelCommentModel> _comments;
        private List<ApiWhitelabelPublicUserModel> _followers;
        private List<ApiWhitelabelPublicUserModel> _likes;
        private List<ApiImageModel> _images;

        /// <summary>gesl
        /// Post id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Company feed / Group feed id / (Guid.Empty for home feed)
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Group / Workspace / Company / Home
        /// </summary>
        public ParentType ParentType { get; set; }

        /// <summary>
        /// If true this post is from the central db (Colab)
        /// </summary>
        public bool FromCentralDatabase { get; set; }

        /// <summary>
        /// Json data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Post type for template selection
        /// </summary>
        public string PostType { get; set; }

        /// <summary>
        /// Datetime of post
        /// </summary>
        public DateTime PostDateTime { get; set; }

        /// <summary>
        /// Stickey feed post
        /// </summary>
        public bool IsSticky { get; set; }

        /// <summary>
        /// Post is created by this user
        /// </summary>
        public ApiWhitelabelPublicUserModel User { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Location / checkin information
        /// </summary>
        public Guid LocationId { get; set; }


        /// <summary>
        /// Hashtags found on post
        /// </summary>
        public string HashTags { get; set; }

        /// <summary>
        /// All comments
        /// </summary>
        public List<ApiWhitelabelCommentModel> Comments
        {
            get { return _comments ?? (_comments = new List<ApiWhitelabelCommentModel>()); }
            set { _comments = value; }
        }

        /// <summary>
        /// All followers
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Followers
        {
            get { return _followers ?? (_followers = new List<ApiWhitelabelPublicUserModel>()); }
            set { _followers = value; }
        }

        /// <summary>
        /// All likers
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Likes
        {
            get { return _likes ?? (_likes = new List<ApiWhitelabelPublicUserModel>()); }
            set { _likes = value; }
        }

        /// <summary>
        /// All images in this post
        /// </summary>
        public List<ApiImageModel> Images
        {
            get { return _images ?? (_images = new List<ApiImageModel>()); }
            set { _images = value; }
        }

        /// <summary>
        /// If set to true this post is made by a wyswyg editor
        /// </summary>
        public bool HtmlOnly { get; set; }
    }

    public class ApiWhitelabelCommentModel
    {
        /// <summary>
        /// Comment id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Comment text
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Commentor
        /// </summary>
        public ApiWhitelabelPublicUserModel User { get; set; }

        /// <summary>
        /// Datetime of comment
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}