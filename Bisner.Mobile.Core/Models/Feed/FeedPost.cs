using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Bisner.Mobile.Core.Extensions;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.Models.Feed
{
    public class FeedPost : ItemBase, IFeedPost
    {
        #region Variables

        private string _avatarUrl;
        private string _displayName;
        private bool _hasCommented;
        private string _commentButtonText;
        private MvxCommand _commentCommand;
        private bool _hasLiked;
        private string _likeButtonText;
        private MvxCommand _likeCommand;
        private int _numberOfLikes;
        private Guid _userId;
        private MvxCommand _userCommand;
        private DateTime _dateTime;
        private int _numberOfComments;
        private string _text;
        private string _mainImageUrl;
        private List<IImage> _images;
        private MvxCommand _mainImageCommand;
        private MvxCommand _leftSubImageCommand;
        private MvxCommand _rightSubImageCommand;
        private string _rightSubImageUrl;
        private string _leftSubImageUrl;
        private Guid _rightSubImageId;
        private string _title;
        private string _location;
        private DateTime _itemDateTime;
        private Guid _relatedItemId;
        private string _postType;
        private ICommand _followCommand;

        #region Resources

        private readonly string _numberCommentTextSingle;
        private readonly string _numberCommentTextMultiple;
        private readonly string _numberLikeTextSingle;
        private readonly string _numberLikeTextMultiple;
        private readonly string _serverErrorText;
        private readonly string _reportMessageText;
        private readonly string _reportMessageTitle;
        private readonly string _yesText;
        private readonly string _noText;
        private readonly string _reportSuccessText;
        private readonly string _reportFailedText;
        private readonly string _okText;

        #endregion Resources

        #endregion Variables

        #region Constructor

        public FeedPost(string numberCommentTextSingle, string numberCommentTextMultiple, string numberLikeTextSingle, string numberLikeTextMultiple, string serverErrorText, string reportMessageText, string reportMessageTitle, string yesText, string noText, string reportSuccessText, string reportFailedText, string okText)
        {
            _numberCommentTextSingle = numberCommentTextSingle;
            _numberCommentTextMultiple = numberCommentTextMultiple;

            _numberLikeTextSingle = numberLikeTextSingle;
            _numberLikeTextMultiple = numberLikeTextMultiple;

            _serverErrorText = serverErrorText;

            _reportMessageText = reportMessageText;
            _reportMessageTitle = reportMessageTitle;

            _yesText = yesText;
            _noText = noText;

            _reportSuccessText = reportSuccessText;
            _reportFailedText = reportFailedText;
            _okText = okText;

            NumberOfLikes = 0;
        }

        #endregion Constructor

        #region Post

        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                RaisePropertyChanged(() => DateTime);
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged(() => Text);
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public DateTime ItemDateTime
        {
            get => _itemDateTime;
            set
            {
                _itemDateTime = value;
                RaisePropertyChanged(() => ItemDateTime);
            }
        }

        private string _itemDateTimeText;
        public string ItemDateTimeText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_itemDateTimeText))
                {
                    _itemDateTimeText = ItemDateTime.ToChatTime(Settings.AmPmNotation);
                }

                return _itemDateTimeText;
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        public int NumberOfImages => Images.Count;

        public List<IImage> Images
        {
            get => _images ?? (_images = new List<IImage>());
            set
            {
                if (value != null)
                {
                    if (value.Count == 1)
                    {
                        // Only main image
                        MainImageUrl = value[0].Medium;
                        _mainImageId = value[0].Id;
                    }

                    if (value.Count == 2)
                    {
                        LeftSubImageUrl = value[0].Medium;
                        _leftSubImageId = value[0].Id;
                        RightSubImageUrl = value[1].Medium;
                        _rightSubImageId = value[1].Id;
                    }

                    if (value.Count > 2)
                    {
                        MainImageUrl = value[0].Medium;
                        _mainImageId = value[0].Id;
                        LeftSubImageUrl = value[1].Medium;
                        _leftSubImageId = value[1].Id;
                        RightSubImageUrl = value[2].Medium;
                        _rightSubImageId = value[2].Id;
                    }
                }

                _images = value;
                RaisePropertyChanged(() => Images);
            }
        }

        public string MainImageUrl
        {
            get => _mainImageUrl;
            private set
            {
                _mainImageUrl = value;
                RaisePropertyChanged(() => MainImageUrl);
            }
        }

        private Guid _mainImageId;

        public MvxCommand MainImageCommand
        {
            get { return _mainImageCommand ?? (_mainImageCommand = new MvxCommand(() => ShowImage(_mainImageId))); }
        }

        private void ShowImage(Guid imageId)
        {
            // Get the image
            var image = Images.FirstOrDefault(i => i.Id == imageId);

            if (image != null)
                ShowViewModel<ImageZoomViewModel>(new
                {
                    url = image.Large
                });
        }

        public string LeftSubImageUrl
        {
            get => _leftSubImageUrl;
            private set
            {
                _leftSubImageUrl = value;
                RaisePropertyChanged(() => LeftSubImageUrl);
            }
        }

        private Guid _leftSubImageId;

        public MvxCommand LeftSubImageCommand
        {
            get
            {
                return _leftSubImageCommand ?? (_leftSubImageCommand = new MvxCommand(() => ShowImage(_leftSubImageId)));
            }
        }

        public string RightSubImageUrl
        {
            get => _rightSubImageUrl;
            private set
            {
                _rightSubImageUrl = value;
                RaisePropertyChanged(() => RightSubImageUrl);
            }
        }

        public MvxCommand RightSubImageCommand
        {
            get
            {
                return _rightSubImageCommand ??
                       (_rightSubImageCommand = new MvxCommand(() => ShowImage(_rightSubImageId)));
            }
        }

        public virtual void Update(IFeedPost item)
        {
            if (item.Id != Id)
            {
                Debug.WriteLine("You are trying to update post with Id {0} with the value of post with Id {1}", Id,
                    item.Id);
                return;
            }

            NumberOfComments = item.NumberOfComments;
            HasCommented = item.HasCommented;
            NumberOfLikes = item.NumberOfLikes;
            HasLiked = item.HasLiked;
            AvatarUrl = item.AvatarUrl;
            DisplayName = item.DisplayName;
            DateTime = item.DateTime;
            Text = item.Text;
            Images = item.Images;
        }

        #endregion Post

        #region Comments

        public int NumberOfComments
        {
            get => _numberOfComments;
            set
            {
                _numberOfComments = value;
                RaisePropertyChanged(() => NumberOfComments);
                CommentButtonText = string.Format(value > 1 ? _numberCommentTextMultiple : _numberCommentTextSingle, value);
            }
        }

        public bool HasCommented
        {
            get => _hasCommented;
            set
            {
                _hasCommented = value;
                RaisePropertyChanged(() => HasCommented);
            }
        }

        public string CommentButtonText
        {
            get => _commentButtonText;
            private set
            {
                _commentButtonText = value;
                RaisePropertyChanged(() => CommentButtonText);
            }
        }

        public MvxCommand CommentCommand => _commentCommand ?? (_commentCommand = new MvxCommand(Comment, CanComment));

        protected virtual void Comment()
        {
            // TODO: Temp workaround hier moet iets van post type komen om op te checken ipv titel, die is nu alleen nog niet null bij event posts
            if (Title != null)
            {
                ShowViewModel<EventViewModel>(new { id = RelatedItemId });
            }
            else
            {
                ShowViewModel<DetailsViewModel>(new { postId = Id });
            }
        }

        private bool CanComment()
        {
            return true;
        }

        #endregion Comments

        #region Likes

        public int NumberOfLikes
        {
            get => _numberOfLikes;
            set
            {
                _numberOfLikes = value;
                RaisePropertyChanged(() => NumberOfLikes);
                LikeButtonText = string.Format(value > 1 ? _numberLikeTextMultiple : _numberLikeTextSingle, value);
            }
        }

        public bool HasLiked
        {
            get => _hasLiked;
            set
            {
                _hasLiked = value;
                RaisePropertyChanged(() => HasLiked);
            }
        }

        public string LikeButtonText
        {
            get => _likeButtonText;
            private set
            {
                _likeButtonText = value;
                RaisePropertyChanged(() => LikeButtonText);
            }
        }

        public MvxCommand LikeCommand
        {
            get { return _likeCommand ?? (_likeCommand = new MvxCommand(async () => await LikeAsync(), CanLike)); }
        }

        private void ShowUser()
        {
            ShowViewModel<UserViewModel>(new { userId = UserId });
        }

        private bool _isLiking;
        private ICommand _reportCommand;
        private bool _isFollowing;

        private async Task LikeAsync()
        {
            if (!_isLiking)
            {
                _isLiking = true;

                SwitchLike();

                try
                {
                    if (!await Mvx.Resolve<IFeedService>().LikePostAsync(Id, HasLiked))
                    {
                        InvokeOnMainThread(async () =>
                        {
                            await Mvx.Resolve<IUserDialogs>().AlertAsync(_serverErrorText);
                        });

                        SwitchLike();
                    }
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                }

                _isLiking = false;
            }
        }

        private void SwitchLike()
        {
            if (HasLiked)
            {
                NumberOfLikes--;
                HasLiked = false;
            }
            else
            {
                NumberOfLikes++;
                HasLiked = true;
            }
        }

        private bool CanLike()
        {
            return true;
        }

        #endregion Likes

        #region Actions

        public ICommand ReportCommand => _reportCommand ?? (_reportCommand = new MvxAsyncCommand(ReportPostAsync));

        public async Task ReportPostAsync()
        {
            var answer = await UserDialogs.ConfirmAsync(_reportMessageText, _reportMessageTitle, _yesText, _noText);

            // If answer is yes, report post
            if (answer)
            {
                try
                {
                    if (await Mvx.Resolve<IFeedService>().ReportPostAsync(Id))
                    {
                        // Reporting succeeded
                        await UserDialogs.AlertAsync(_reportSuccessText, "", _okText);
                    }
                    else
                    {
                        // Reporting failed
                        await UserDialogs.AlertAsync(_reportFailedText, "", _okText);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public bool IsFollowing
        {
            get => _isFollowing;
            set { _isFollowing = value; RaisePropertyChanged(() => IsFollowing); }
        }

        public ICommand FollowCommand => _followCommand ?? (_followCommand = new MvxAsyncCommand(FollowPostAsync));

        private bool _isTryingToFollow = false;

        public async Task FollowPostAsync()
        {
            if (!_isTryingToFollow)
            {
                _isTryingToFollow = true;

                SwitchFollow();

                try
                {
                    if (!await Mvx.Resolve<IFeedService>().FollowPostAsync(Id, IsFollowing))
                    {
                        await Mvx.Resolve<IUserDialogs>().AlertAsync(_serverErrorText);

                        SwitchFollow();
                    }
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                }
                finally
                {
                    _isTryingToFollow = false;
                }
            }
        }

        private void SwitchFollow()
        {
            IsFollowing = !IsFollowing;
        }

        #endregion Actions

        #region User

        public Guid UserId
        {
            get => _userId;
            set { _userId = value; RaisePropertyChanged(() => UserId); }
        }

        public string AvatarUrl
        {
            get => _avatarUrl;
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        public MvxCommand UserCommand => _userCommand ?? (_userCommand = new MvxCommand(ShowUser));

        #endregion User

        #region RelatedItem

        public Guid RelatedItemId
        {
            get => _relatedItemId;
            set { _relatedItemId = value; RaisePropertyChanged(() => RelatedItemId); }
        }

        public string PostType
        {
            get => _postType;
            set { _postType = value; RaisePropertyChanged(() => PostType); }
        }

        #endregion RelatedItem

        #region Interaction

        protected IUserDialogs UserDialogs => Mvx.Resolve<IUserDialogs>();

        #endregion Interaction

        public void ShowUrlWarning(string url)
        {
            Task.Run(async () =>
            {
                var result = await UserDialogs.ConfirmAsync(
                    $"You just clicked a link to go to another website ({url}). If you continue, you will leave this app and go to a site run by someone else.",
                    "You are leaving the community", "Continue", "Go back");

                if (result)
                {
                    InvokeOnMainThread(() =>
                    {
                        // Open browser
                        Mvx.Resolve<INetworkManager>().OpenUrl(url);
                    });
                }
            });
        }
    }
}
