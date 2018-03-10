using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Akavache;
using Bisner.ApiModels.Whitelabel;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.Models.Feed.DataModels;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace Bisner.Mobile.Core.ViewModels.Feed
{
    public enum FeedType
    {
        Home = 0,
        Company = 1,
        Group = 2,
    }

    /// <summary>
    /// Viewmodel just for usage in the android main fragment tabs
    /// </summary>
    /// <seealso cref="Bisner.Mobile.Core.ViewModels.Feed.FeedViewModel" />
    public class AndroidMainFeedViewModel : FeedViewModel
    {
        #region Constructor

        public AndroidMainFeedViewModel(IPlatformService platformService, ICompanyService companyService, IUserService userService, IFeedService feedService, IGroupService groupService, INotificationService notificationService, IChatService chatService) : base(platformService, companyService, userService, feedService, groupService, notificationService, chatService)
        {
        }

        #endregion Constructor
    }

    /// <summary>
    /// ViewModel for the feed
    /// </summary>
    public class FeedViewModel : BaseViewModel
    {
        #region Constructor

        private DateTime? _olderThen;

        private CompanyFeedItem _company;
        private GroupFeedItem _group;

        private ExtendedObservableCollection<IFeedItem> _items;
        private bool _isRefreshing, _isNotRefresing;

        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IFeedService _feedService;
        private readonly IGroupService _groupService;
        private readonly INotificationService _notificationService;
        private readonly IChatService _chatService;

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

        public FeedViewModel(IPlatformService platformService, ICompanyService companyService, IUserService userService, IFeedService feedService, IGroupService groupService, INotificationService notificationService, IChatService chatService) : base(platformService)
        {
            _companyService = companyService;
            _userService = userService;
            _feedService = feedService;
            _groupService = groupService;
            _notificationService = notificationService;
            _chatService = chatService;

            Items = new ExtendedObservableCollection<IFeedItem>(new List<IFeedItem>());
            CanCreate = true;
        }

        #endregion Constructor

        #region Init

        public async Task Init(Guid id, FeedType feedType)
        {
            Debug.WriteLine("FeedViewModel INIT with id = {0}", id);

            SetResources();

            //IsRefreshing = true;

            FeedId = id;
            FeedType = feedType;

            // For the postbox
            //await SetUserInfo();

            // Check company if this is company feed
            if (feedType == FeedType.Company)
                await GetCompany();

            // Check group if this is a group feed
            if (feedType == FeedType.Group)
                await GetGroup();

            try
            {
                var items = await GetFeedItemsAsync(null);

                _olderThen = items.Any() ? items.Min(p => p.DateTime) : DateTime.MaxValue;

                AddHeader(items);

                Items = new ExtendedObservableCollection<IFeedItem>(items);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                IsRefreshing = false;
            }
        }

        private async Task SetUserInfo()
        {
            var platform = await PlatformService.GetPublicPlatformAsync(ApiPriority.UserInitiated);
            var user = await _userService.GetPersonalModelAsync(ApiPriority.UserInitiated);

            UserAvatarUrl = platform?.CdnBasePath + user?.Avatar?.Small;
            PostboxText = $"Post an update {user?.FirstName}";
        }

        private async Task GetCompany()
        {
            var company = await _companyService.GetAsync(ApiPriority.UserInitiated, FeedId);

            var companyIndustry = await PlatformService.GetIndustryAsync(ApiPriority.UserInitiated, company.IndustryId);

            var companyFeedItem = new CompanyFeedItem(company, companyIndustry) { DateTime = DateTime.MaxValue };

            var location = await PlatformService.GetLocationAsync(ApiPriority.UserInitiated, company.LocationId);

            if (location != null)
            {
                companyFeedItem.Location = location.Name;
            }

            _company = companyFeedItem;

            CanCreate = company.UserIds.Contains(Settings.UserId);

            Title = company.Name;
        }

        private async Task GetGroup()
        {
            var group = await _groupService.GetAsync(ApiPriority.UserInitiated, FeedId);

            var groupFeedItem = new GroupFeedItem(group) { DateTime = DateTime.MaxValue };

            _group = groupFeedItem;

            CanCreate = group.UserIds.Contains(Settings.UserId);

            Title = group.Name;
        }

        private void SetResources()
        {
            try
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
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Init 

        #region IEventViewModel

        public override void Attach()
        {
            base.Attach();

            var bisnerClient = Mvx.Resolve<ISignalRClient>();

            bisnerClient.NewFeedPost += AddPost;
            bisnerClient.DeleteFeedPost += RemovePost;
        }

        public override void Unattach()
        {
            base.Unattach();

            var bisnerClient = Mvx.Resolve<ISignalRClient>();

            bisnerClient.NewFeedPost -= AddPost;
            bisnerClient.DeleteFeedPost -= RemovePost;
        }

        #endregion IEventViewModel

        #region Update

        private void RemovePost(Guid guid)
        {
            InvokeOnMainThread(() =>
            {
                var post = Items.FirstOrDefault(p => p.Id == guid);

                if (post != null)
                {
                    Items.Remove(post);
                }
            });
        }

        private void AddPost(ApiWhitelabelFeedPostModel model)
        {
            BlobCache.LocalMachine.Invalidate("homefeed");

            var oldPost = Items.FirstOrDefault(p => p.Id == model.Id && p is IFeedPost);

            InvokeOnMainThread(() =>
            {
                var feedPost = BuildItem(model);

                if (oldPost != null)
                {
                    // Update existing
                    ((IFeedPost)oldPost).Update(feedPost);
                }
                else
                {
                    // Insert new one
                    Items.Insert(FeedId == Guid.Empty ? 0 : 1, feedPost);
                }
            });
        }

        #endregion Update


        #region Create

        private ICommand _createCommand;
        private string _userAvatarUrl;

        public ICommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new MvxCommand(CreatePost, () => CanCreate)); }
        }

        public string UserAvatarUrl
        {
            get => _userAvatarUrl;
            set { _userAvatarUrl = value; RaisePropertyChanged(() => UserAvatarUrl); }
        }

        public string PostboxText
        {
            get => _postboxText;
            private set { _postboxText = value; RaisePropertyChanged(() => PostboxText); }
        }

        private void CreatePost()
        {
            ShowViewModel<AddPostViewModel>(new { feedId = FeedId });
        }

        public bool CanCreate
        {
            get => _canCreate;
            private set { _canCreate = value; RaisePropertyChanged(() => CanCreate); }
        }

        #endregion Create

        #region Refresh

        private MvxCommand _refreshCommand;
        private string _postboxText;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                IsNotRefresing = !value;
                RaisePropertyChanged(() => IsRefreshing);
            }
        }

        public bool IsNotRefresing
        {
            get => _isNotRefresing;
            private set { _isNotRefresing = value; RaisePropertyChanged(() => IsNotRefresing); }
        }

        public MvxCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => await RefreshAsync()));
            }
        }

        private async Task RefreshAsync()
        {
            if (!IsRefreshing)
            {
                IsRefreshing = true;

                try
                {
                    var items = await GetFeedItemsAsync(null);

                    _olderThen = items.Any() ? items.Min(p => p.DateTime) : DateTime.MaxValue;

                    AddHeader(items);

                    Items = new ExtendedObservableCollection<IFeedItem>(items);
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }
                finally
                {
                    IsRefreshing = false;
                }
            }
        }

        #endregion Refresh

        #region LoadMore

        private MvxCommand _loadMoreCommand;
        private bool _isLoadingMore, _isAtEnd;
        private bool _canCreate;

        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            set { _isLoadingMore = value; RaisePropertyChanged(() => IsLoadingMore); }
        }

        public MvxCommand LoadMoreCommand
        {
            get
            {
                return _loadMoreCommand ?? (_loadMoreCommand = new MvxCommand(async () => await LoadMoreAsync()));
            }
        }

        private async Task LoadMoreAsync()
        {
            if (!IsLoadingMore && !_isAtEnd)
            {
                IsLoadingMore = true;
                Debug.WriteLine("LOADING MORE!");

                try
                {
                    var posts = await GetFeedItemsAsync(_olderThen);

                    if (posts.Any())
                    {
                        _olderThen = posts.Min(p => p.DateTime);
                    }
                    else
                    {
                        _isAtEnd = true;
                    }

                    InvokeOnMainThread(() =>
                    {
                        UpdateList(posts);
                    });
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }
                finally
                {
                    IsLoadingMore = false;
                    Debug.WriteLine("DONE LOADING MORE!");
                }
            }
        }

        #endregion LoadMore

        #region Items

        public Guid FeedId { get; private set; }

        public FeedType FeedType { get; private set; }

        public ExtendedObservableCollection<IFeedItem> Items
        {
            get => _items;
            private set { _items = value; RaisePropertyChanged(() => Items); }
        }

        private async Task<List<IFeedItem>> GetFeedItemsAsync(DateTime? olderThen)
        {
            FeedResponseModel feedResponseModel;

            switch (FeedType)
            {
                case FeedType.Home:
                    feedResponseModel = await _feedService.GetHomeFeedAsync(ApiPriority.UserInitiated, olderThen);
                    break;
                case FeedType.Company:
                    feedResponseModel = await _feedService.GetCompanyFeedAsync(ApiPriority.UserInitiated, FeedId, olderThen);
                    break;
                case FeedType.Group:
                    feedResponseModel = await _feedService.GetGroupFeedAsync(ApiPriority.UserInitiated, FeedId, olderThen);
                    break;
                default:
                    feedResponseModel = null;
                    break;
            }

            var feedPosts = BuildItems(feedResponseModel);

            return new List<IFeedItem>(feedPosts);
        }

        private void AddHeader(List<IFeedItem> items)
        {
            if (FeedType == FeedType.Company)
                AddCompany(items);

            if (FeedType == FeedType.Group)
                AddGroup(items);
        }

        private void AddCompany(List<IFeedItem> items)
        {
            if (FeedId != Guid.Empty && !items.Any(i => i is CompanyFeedItem))
            {
                // Add company header
                items.Insert(0, _company);
            }
        }

        private void AddGroup(List<IFeedItem> items)
        {
            if (FeedId != Guid.Empty && !items.Any(i => i is GroupFeedItem))
            {
                // Add company header
                items.Insert(0, _group);
            }
        }

        private void UpdateList(IEnumerable<IFeedItem> posts)
        {
            BlobCache.LocalMachine.Invalidate("homefeed");

            foreach (var feedPost in posts)
            {
                var oldPost = Items.FirstOrDefault(p => p.Id == feedPost.Id && p is IFeedPost);

                if (oldPost != null && feedPost is IFeedPost)
                {
                    InvokeOnMainThread(() =>
                    {
                        ((IFeedPost)oldPost).Update((IFeedPost)feedPost);
                    });
                }
                else
                {
                    // Get index in list
                    var closestPost = Items.Where(i => i is IFeedPost).Cast<IFeedPost>().OrderBy(t => Math.Abs((t.DateTime - feedPost.DateTime).Ticks)).FirstOrDefault();

                    if (closestPost == null)
                    {
                        Items.Add(feedPost);
                        continue;
                    }

                    var indexOf = Items.IndexOf(closestPost);

                    InvokeOnMainThread(() =>
                    {
                        if (closestPost.DateTime > feedPost.DateTime)
                        {
                            // Older then
                            Items.Insert(indexOf + 1, feedPost);
                        }
                        else
                        {
                            // Younger then
                            Items.Insert(indexOf, feedPost);
                        }
                    });
                }
            }
        }

        #endregion Items

        #region ToPostModel

        public List<IFeedPost> BuildItems(FeedResponseModel feedResponseModel)
        {
            var feedPosts = new List<IFeedPost>();

            if (feedResponseModel != null)
            {
                foreach (var apiWhitelabelFeedPostModel in feedResponseModel.Posts)
                {
                    var feedPost = BuildItem(apiWhitelabelFeedPostModel);

                    SetRelatedItemData(feedPost, apiWhitelabelFeedPostModel, feedResponseModel.RelatedItems);

                    feedPosts.Add(feedPost);
                }
            }

            return feedPosts;
        }

        private void SetRelatedItemData(IFeedPost feedPost, ApiWhitelabelFeedPostModel feedPostModel, ApiRelatedItemsModel relatedItems)
        {
            var feedPostDataModel = JsonConvert.DeserializeObject<FeedPostDataModel>(feedPostModel.Data);

            switch (feedPost.PostType)
            {
                case ProviderNames.WhitelabelEventPostProvider:
                    SetEventData(feedPost, relatedItems.Events.FirstOrDefault(e => e.Id == feedPostDataModel.EventId));
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

    public class ExtendedObservableCollection<T> : ObservableCollection<T>
    {
        public ExtendedObservableCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        public void AddRange(IEnumerable<T> dataToAdd)
        {
            CheckReentrancy();

            //
            // We need the starting index later
            //
            var startingIndex = Count;

            //
            // Add the items directly to the inner collection

            //
            foreach (var data in dataToAdd)
            {
                Items.Add(data);
            }

            //
            // Now raise the changed events
            //
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            //
            // We have to change our input of new items into an IList since that is what the
            // event args require.
            //
            var changedItems = new List<T>(dataToAdd);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startingIndex));
        }

        public void RemoveRange(IEnumerable<T> dataToRemove)
        {
            CheckReentrancy();

            //
            // We need the starting index later
            //
            var startingIndex = Count;

            //
            // Add the items directly to the inner collection

            //
            foreach (var data in dataToRemove)
            {
                Items.Remove(data);
            }

            //
            // Now raise the changed events
            //
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            //
            // We have to change our input of new items into an IList since that is what the
            // event args require.
            //
            var changedItems = new List<T>(dataToRemove);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startingIndex));
        }
    }
}
