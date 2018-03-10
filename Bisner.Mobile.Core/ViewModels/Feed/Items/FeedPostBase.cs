using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Feed.Items
{
    public abstract class FeedPostBase : MvxViewModel, IFeedPost
    {
        #region Constructor

        private Guid _id;
        private string _avatarUrl;
        private string _displayName;
        private bool _hasCommented;
        private string _commentButtonText;
        private MvxCommand _commentCommand;
        private bool _hasLiked;
        private string _likeButtonText;
        private MvxCommand _likeCommand;
        private int _numberOfComments;
        private int _numberOfLikes;
        private MvxCommand _userCommand;

        protected FeedPostBase()
        {
            CommentCommand = new MvxCommand(Comment, CanComment);
            LikeCommand = new MvxCommand(Like, CanLike);
            UserCommand = new MvxCommand(ShowUser);
            NumberOfComments = 0;
            NumberOfLikes = 0;
        }

        #endregion Constructor

        public abstract FeedPostType Type { get; }

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime DateTime { get; set; }

        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        #region Comments

        public int NumberOfComments
        {
            get { return _numberOfComments; }
            set
            {
                _numberOfComments = value; RaisePropertyChanged(() => NumberOfComments);
                HasCommented = value > 0;
                CommentButtonText = value + " comments";
            }
        }

        public bool HasCommented
        {
            get { return _hasCommented; }
            private set { _hasCommented = value; RaisePropertyChanged(() => HasCommented); }
        }

        public string CommentButtonText
        {
            get { return _commentButtonText; }
            private set { _commentButtonText = value; RaisePropertyChanged(() => CommentButtonText); }
        }

        public MvxCommand CommentCommand
        {
            get { return _commentCommand; }
            private set { _commentCommand = value; RaisePropertyChanged(() => CommentCommand); }
        }

        private void Comment()
        {
            NumberOfComments++;
            //ShowViewModel<CommentViewModel>(new { id = Id });
        }

        private bool CanComment()
        {
            return true;
        }

        #endregion Comments

        #region Likes

        public int NumberOfLikes
        {
            get { return _numberOfLikes; }
            set
            {
                _numberOfLikes = value; RaisePropertyChanged(() => NumberOfLikes);
                HasLiked = value > 0;
                LikeButtonText = value + " likes";
            }
        }

        public bool HasLiked
        {
            get { return _hasLiked; }
            private set { _hasLiked = value; RaisePropertyChanged(() => HasLiked); }
        }

        public string LikeButtonText
        {
            get { return _likeButtonText; }
            private set { _likeButtonText = value; RaisePropertyChanged(() => LikeButtonText); }
        }

        public MvxCommand LikeCommand
        {
            get { return _likeCommand; }
            private set { _likeCommand = value; RaisePropertyChanged(() => LikeCommand); }
        }

        private void Like()
        {
            NumberOfLikes++;
            //ShowViewModel<CommentViewModel>(new { id = Id });
        }

        private bool CanLike()
        {
            return true;
        }

        #endregion Likes

        #region User

        public MvxCommand UserCommand
        {
            get { return _userCommand; }
            private set { _userCommand = value; RaisePropertyChanged(() => UserCommand); }
        }


        private void ShowUser()
        {
            ShowViewModel<UserViewModel>();
        }

        #endregion User
    }
}