using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.Models.Feed.DataModels;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace Bisner.Mobile.Core.ViewModels.Feed
{
    public class DetailsViewModel : MentionViewModelBase
    {
        #region Constructor

        private ExtendedObservableCollection<IItemBase> _items;
        private MvxCommand _commentCommand;
        private string _commentInput;
        private bool _isLoading;
        private bool _isNotLoading;

        private readonly IFeedService _feedService;

        #region Resources

        private string _numberCommentTextSingle;
        private string _numberCommentTextMultiple;
        private string _numberLikeTextSingle;
        private string _numberLikeTextMultiple;
        private string _serverErrorText;
        private string _reportMessageText;
        private string _reportMessageTitle;
        private string _yesText;
        private string _noText;
        private string _reportSuccessText;
        private string _reportFailedText;
        private string _okText;

        #endregion Resources

        public DetailsViewModel(IPlatformService platformService, IFeedService feedService, IUserService userService) : base(platformService, userService)
        {
            _feedService = feedService;

            IsLoading = false;
            _commentCommand = new MvxCommand(async () => await Comment(), CanComment);

            MentionsProperty = () => CommentInput;
        }

        public override void Attach()
        {
            base.Attach();

            Mvx.Resolve<ISignalRClient>().NewFeedPost += BisnerClientOnNewFeedPost;
        }

        public override void Unattach()
        {
            base.Unattach();

            Mvx.Resolve<ISignalRClient>().NewFeedPost -= BisnerClientOnNewFeedPost;
        }

        private void BisnerClientOnNewFeedPost(ApiWhitelabelFeedPostModel model)
        {
            if (model.Id == PostId)
            {
                // This is the current post
                InvokeOnMainThread(async () =>
                {
                    var postModel = await _feedService.GetPostAsync(ApiPriority.Background, model.Id);

                    var posts = BuildItems(postModel);

                    var post = posts.FirstOrDefault();

                    var currentPost = Items[0] as IFeedPost;

                    currentPost?.Update(post);

                    var comments = postModel.Posts.FirstOrDefault()?.Comments.Select(c => c.ToModel());

                    // add new comments
                    var newComments = comments.Where(c => Items.All(i => i.Id != c.Id)).OrderBy(c => c.DateTime).ToList();

                    foreach (var newComment in newComments.OrderBy(c => c.DateTime))
                    {
                        Items.Add(newComment);
                    }

                    if (newComments.Any())
                        Mvx.Resolve<IMvxMessenger>().Publish(new NewCommentMessage(this));
                });
            }
        }

        public async Task Init(Guid postId)
        {
            PostId = postId;

            try
            {
                SetResources();

                var feedResponseModel = await _feedService.GetPostAsync(ApiPriority.UserInitiated, postId);

                var posts = BuildItems(feedResponseModel);

                var post = posts.FirstOrDefault();

                if (post == null)
                {
                    await UserDialogs.AlertAsync(ResKeys.mobile_error_server_error);

                    Debug.WriteLine("Could not find post with id {0} in repository", PostId);

                    Close(this);
                    return;
                }

                var comments = feedResponseModel.Posts.FirstOrDefault()?.Comments.Select(c => c.ToModel());

                // Add post
                var items = new List<IItemBase>(posts);

                // Add comments
                items.AddRange(comments.OrderBy(p => p.DateTime).ToList());

                Items = new ExtendedObservableCollection<IItemBase>(items);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private void SetResources()
        {
            _numberCommentTextSingle = Settings.GetResource(ResKeys.mobile_feed_comments);
            _numberCommentTextMultiple = Settings.GetResource(ResKeys.mobile_feed_comment);

            _numberLikeTextSingle = Settings.GetResource(ResKeys.mobile_feed_likes);
            _numberLikeTextMultiple = Settings.GetResource(ResKeys.mobile_feed_like);

            _serverErrorText = Settings.GetResource(ResKeys.mobile_error_server_error);

            _reportMessageText = Settings.GetResource(ResKeys.mobile_report_post_message);
            _reportMessageTitle = Settings.GetResource(ResKeys.mobile_report_post_title);

            _yesText = Settings.GetResource(ResKeys.mobile_btn_yes);
            _noText = Settings.GetResource(ResKeys.mobile_btn_no);

            _reportSuccessText = Settings.GetResource(ResKeys.mobile_report_success);
            _reportFailedText = Settings.GetResource(ResKeys.mobile_report_failed);
            _okText = Settings.GetResource(ResKeys.mobile_btn_ok);
        }

        #endregion Constructor

        #region Comments

        public ExtendedObservableCollection<IItemBase> Items
        {
            get => _items;
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public string CommentInput
        {
            get => _commentInput;
            set
            {
                _commentInput = value;
                RaisePropertyChanged(() => CommentInput);
                CommentCommand.RaiseCanExecuteChanged();
                Task.Run(async () => { await CheckMentions(value); });
            }
        }

        public MvxCommand CommentCommand
        {
            get { return _commentCommand ?? (_commentCommand = new MvxCommand(async () => await Comment(), CanComment)); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;
                IsNotLoading = !value;
                RaisePropertyChanged(() => IsLoading);
                CommentCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsNotLoading
        {
            get => _isNotLoading;
            private set { _isNotLoading = value; RaisePropertyChanged(() => IsNotLoading); }
        }

        private async Task Comment()
        {
            if (!IsLoading)
            {
                IsLoading = true;

                try
                {
                    if (!await _feedService.CommentAsync(PostId, CommentInput, MentionUsers.Select(u => u.User.Id).ToList()))
                    {
                        await UserDialogs.AlertAsync("Could not post your comment! Please try again.", "Error");
                    }
                    else
                    {
                        CommentInput = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                    UserDialogs.Alert("Could not post your comment! Please try again.", null, "Error");
                }
                finally
                {
                    IsLoading = false;
                    Mvx.Resolve<IMvxMessenger>().Publish(new NewCommentMessage(this));
                }
            }
        }

        private bool CanComment()
        {
            return !IsLoading && !string.IsNullOrEmpty(CommentInput);
        }

        #endregion Comments

        public void Update()
        {
            InvokeOnMainThread(async () => await Init(PostId));
        }

        #region Properties

        public Guid PostId { get; set; }

        public string SendButtonText => GetResource(ResKeys.mobile_post_btn_post);

        #endregion Properties

        #region ToPostModel

        public List<IFeedPost> BuildItems(FeedResponseModel feedResponseModel)
        {
            var feedPosts = new List<IFeedPost>();

            if (feedResponseModel != null)
            {
                foreach (var apiWhitelabelFeedPostModel in feedResponseModel.Posts)
                {
                    var feedPost = BuildItem(apiWhitelabelFeedPostModel);

                    SetRelatedItemData(feedPost, feedResponseModel.RelatedItems);

                    feedPosts.Add(feedPost);
                }
            }

            return feedPosts;
        }

        private void SetRelatedItemData(IFeedPost feedPost, ApiRelatedItemsModel relatedItems)
        {
            switch (feedPost.PostType)
            {
                case Constants.ProviderNames.WhitelabelEventPostProvider:
                    SetEventData(feedPost, relatedItems.Events.FirstOrDefault(e => e.Id == feedPost.RelatedItemId));
                    break;
            }
        }

        private IFeedPost BuildItem(ApiWhitelabelFeedPostModel feedPostModel)
        {
            var ownId = Settings.UserId;

            // Base properties
            var feedPost = new FeedPost(_numberCommentTextSingle, _numberCommentTextMultiple, _numberLikeTextSingle, _numberLikeTextMultiple, _serverErrorText, _reportMessageText, _reportMessageTitle, _yesText, _noText, _reportSuccessText, _reportFailedText, _okText)
            {
                Id = feedPostModel.Id,
                NumberOfLikes = feedPostModel.Likes?.Count ?? 0,
                HasLiked = feedPostModel.Likes?.Any(l => l.Id == ownId) ?? false,
                NumberOfComments = feedPostModel.Comments?.Count ?? 0,
                DateTime = feedPostModel.PostDateTime,
                HasCommented = feedPostModel.Comments?.Any(c => c.User?.Id == ownId) ?? false,
                UserId = feedPostModel.UserId,
                PostType = feedPostModel.PostType,
                IsFollowing = feedPostModel.Followers.Any(f => f.Id == ownId),
            };

            // Check if has images
            GetAndSetImages(feedPostModel, feedPost);

            // Set the user properties
            GetAndSetUserProperties(feedPost, feedPostModel);

            // Set text if there is any
            SetText(feedPost, feedPostModel);

            return feedPost;
        }

        private void SetEventData(IFeedPost feedPost, ApiWhitelabelEventModel eventModel)
        {
            if (eventModel == null)
                return;

            IImage image;

            if (eventModel.Header == null)
            {
                image = new Image
                {
                    Id = Guid.Empty,
                    Small = Defaults.EventHeaderDefaultString,
                    Medium = Defaults.EventHeaderDefaultString,
                    Large = Defaults.EventHeaderDefaultString,
                    OriginalFileName = Defaults.EventHeaderDefaultString,
                };
            }
            else
            {
                image = eventModel.Header.ToModel();
            }

            // TODO : We have to assign a new list in order to get the feedposte model to assign the correct images to the right properties
            feedPost.Images = new List<IImage> { image };
            feedPost.Title = eventModel.Title;
            feedPost.ItemDateTime = eventModel.DateTime;
            feedPost.Location = eventModel.Location;
            feedPost.RelatedItemId = eventModel.Id;
        }

        private void GetAndSetImages(ApiWhitelabelFeedPostModel feedPostModel, FeedPost feedPost)
        {
            if (feedPostModel.Images.Any())
            {
                feedPost.Images = feedPostModel.Images.Select(i => i.ToModel()).ToList();
            }
        }

        private void GetAndSetUserProperties(FeedPost feedPost, ApiWhitelabelFeedPostModel feedPostModel)
        {
            if (feedPostModel.User == null)
            {
                // TODO : Unknown user
                return;
            }

            feedPost.DisplayName = feedPostModel.User.DisplayName;
            feedPost.UserId = feedPostModel.UserId;
            feedPost.AvatarUrl = feedPostModel.User?.Avatar?.Small;
        }

        private void SetText(IFeedPost post, ApiWhitelabelFeedPostModel feedPostModel)
        {
            var textDataModel = JsonConvert.DeserializeObject<FeedPostDataModel>(feedPostModel.Data);

            post.Text = EmojiHelper.ShortnameToUnicode(textDataModel.Text);
            if (!string.IsNullOrWhiteSpace(post.Text))
                post.Text = BbCode.ConvertToHtml(post.Text);
        }

        #endregion ToPostModel
    }
}