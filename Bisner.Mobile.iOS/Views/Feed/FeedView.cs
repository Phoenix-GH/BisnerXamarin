using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Feed.Cells;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;
using static System.Double;

namespace Bisner.Mobile.iOS.Views.Feed
{
    partial class FeedView : ViewBase<FeedViewModel>, IUITableViewDelegate
    {
        #region Constructor

        private GenericTableViewSource<IFeedItem> _source;
        private MvxUIRefreshControl _refreshControl;

        //private NSObject _didAppearNotification;
        private MvxSubscriptionToken _groupJoinedToken;

        private UIBarButtonItem _createButton;

        public FeedView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("FEEDVIEW RECIEVED MEMORY WARNING!!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupAppearance();
            SetupTable();

            // Create add post button
            if (ViewModel.CanCreate)
            {
                SetCreateIcon();
            }
            SetupBindings();

            // Set screen name for analytics
            ScreenName = "FeedView id = " + ViewModel.FeedId;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _groupJoinedToken = Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<GroupJoinedMessage>(m =>
            {
                if (m.GroupId == ViewModel.FeedId)
                {
                    // message is for this group
                    if (m.HasJoined)
                    {
                        SetCreateIcon();
                    }
                    else
                    {
                        UnSetCreateIcon();
                    }
                }
            });

            View.LayoutIfNeeded();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //_didAppearNotification.Dispose();

            //ScrollToTop(false);
            _groupJoinedToken.Dispose();
            _groupJoinedToken = null;
        }

        #endregion ViewController

        #region Setup

        private void SetupAppearance()
        {
            FeedTable.BackgroundColor = Appearance.Colors.BackgroundColor;

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                FeedTable.AtTopOf(View),
                FeedTable.AtLeftOf(View),
                FeedTable.AtRightOf(View),
                FeedTable.AtBottomOf(View)
                );
        }

        private readonly Dictionary<Guid, nfloat> _textHeights = new Dictionary<Guid, nfloat>();
        private readonly Dictionary<Guid, NSAttributedString> _htmlTexts = new Dictionary<Guid, NSAttributedString>();

        private void SetupTable()
        {
            // Main feed needs top offset
            //if (ViewModel.FeedId == Guid.Empty)
            //{
            //    FeedTable.ContentInset = new UIEdgeInsets(5, 0, 0, 0);
            //}

            _source = new GenericTableviewSourceWithHeight<IFeedItem>(FeedTable)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    if (item is CompanyFeedItem)
                    {
                        return CompanyFeedCell.Identifier;
                    }

                    if (item is GroupFeedItem)
                    {
                        return GroupFeedCell.Identifier;
                    }

                    return FeedPostCell.Identifier;
                },
                GetCellHeightFunc = (view, path, item) =>
                {
                    if (item is CompanyFeedItem)
                    {
                        return GetCompanyCellHeight();
                    }

                    if (item is GroupFeedItem)
                    {
                        return 470;
                    }

                    return GetPostCellHeight(item as IFeedPost);
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    if (item is CompanyFeedItem)
                    {
                        return 547;
                    }

                    if (item is GroupFeedItem)
                    {
                        return 470;
                    }

                    nfloat imageHeight = 0;

                    var post = item as IFeedPost;

                    if (post != null)
                    {
                        if (post.NumberOfImages == 1)
                        {
                            // 8 spacing below image
                            imageHeight += UIScreen.MainScreen.Bounds.Width * 0.6f + 8;
                        }
                        else if (post.NumberOfImages == 2)
                        {
                            // 8 spacing below image
                            imageHeight += (UIScreen.MainScreen.Bounds.Width - 3) / 2 * 0.6f + 8;
                        }
                        else if (post.NumberOfImages > 2)
                        {
                            // 8 spacing below image
                            imageHeight += UIScreen.MainScreen.Bounds.Width * 0.6f + 8;
                            imageHeight += (UIScreen.MainScreen.Bounds.Width - 3) / 2 * 0.6f + 3;
                        }

                        var finalHeight = 133 + imageHeight;

                        if (_textHeights.ContainsKey(post.Id))
                        {
                            finalHeight += _textHeights[post.Id];
                        }

                        return finalHeight;
                    }

                    throw new Exception("FEED ----- UNKOWN ITEM TYPE IN LIST");
                },
                ModifyCellFunc = (cell, path, item) =>
                {
                    if (item is CompanyFeedItem)
                    {
                        CalculateCompanyTextHeightCombined(item as CompanyFeedItem, cell as CompanyFeedCell);
                    }

                    var post = item as IFeedPost;

                    if (post != null)
                    {
                        SetCellText(post, cell);

                        var postCell = cell as FeedPostCell;

                        if (postCell != null)
                        {
                            if (post.NumberOfImages == 1)
                            {
                                postCell.SetMainImageVisible();
                            }
                            else if (post.NumberOfImages == 2)
                            {
                                postCell.SetSubImagesVisible();
                            }
                            else if (post.NumberOfImages > 2)
                            {
                                postCell.SetAllImagesVisible();
                            }
                            else
                            {
                                postCell.SetNoImagesVisible();
                            }

                            postCell.SetInformationVisible(post.Title != null);
                        }
                    }
                },
                AutoDeselect = true,
            };

            _refreshControl = new MvxUIRefreshControl();

            FeedTable.RegisterClassForCellReuse(typeof(CompanyFeedCell), CompanyFeedCell.Identifier);
            FeedTable.RegisterClassForCellReuse(typeof(GroupFeedCell), GroupFeedCell.Identifier);
            FeedTable.RegisterClassForCellReuse(typeof(FeedPostCell), FeedPostCell.Identifier);
            //_bottomActivityIndicatorView = new MvxUiRefreshControl
            //{
            //    Frame = new CGRect(0, 0, 320, 66)
            //};
            //FeedTable.TableFooterView = _bottomActivityIndicatorView;
            FeedTable.Source = _source;

            FeedTable.AddSubview(_refreshControl);
            FeedTable.ReloadData();
        }

        /// <summary>
        /// Scroll the table to the top (when pressing home button)
        /// </summary>
        /// <param name="animated"></param>
        public void ScrollToTop(bool animated)
        {
            FeedTable.SetContentOffset(new CGPoint(0, 0), animated);
        }

        private MvxUIRefreshControl _bottomActivityIndicatorView;

        private void SetCreateIcon()
        {
            if (Settings.UserRoles.All(r => r != Home.Feed.Create.ToLower()))
                return;

            if (_createButton == null)
            {
                using (
                    var createImage =
                        UIImage.FromBundle("Icons/create_post_btn.png")
                            .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
                {
                    _createButton = new UIBarButtonItem(createImage, UIBarButtonItemStyle.Plain, null, null);
                }
            }

            NavigationItem.SetRightBarButtonItem(_createButton, true);
        }

        private void UnSetCreateIcon()
        {
            NavigationItem.SetRightBarButtonItem(null, true);
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<FeedView, FeedViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Bind(_source).For(s => s.LoadMoreCommand).To(vm => vm.LoadMoreCommand);
            set.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Bind(_bottomActivityIndicatorView).For(r => r.IsRefreshing).To(vm => vm.IsLoadingMore);
            set.Bind(_createButton).To(vm => vm.CreateCommand);
            set.Apply();
        }

        #endregion Setup

        #region CellText

        private void SetCellText(IFeedPost post, UITableViewCell cell)
        {
            if (!string.IsNullOrWhiteSpace(post.Text))
            {
                var text = GetText(post);

                var feedCell = cell as FeedPostCell;

                if (feedCell != null)
                {
                    feedCell.PostText.AttributedText = text;

                    CalculateTextHeight(post, feedCell);
                }
            }
        }

        private NSAttributedString GetText(IFeedPost post)
        {
            NSAttributedString text;

            if (!_htmlTexts.ContainsKey(post.Id))
            {
                text = post.Text.ConvertHtml();

                _htmlTexts[post.Id] = text;
            }
            else
            {
                text = _htmlTexts[post.Id];
            }

            return text;
        }

        private void CalculateTextHeight(IFeedPost post, FeedPostCell feedCell)
        {
            var textSize = feedCell.PostText.SizeThatFits(new CGSize(feedCell.PostText.Frame.Width, MaxValue));

            _textHeights[post.Id] = textSize.Height;
        }

        private nfloat _companyPostTextHeight = 0;

        private void CalculateCompanyTextHeightCombined(CompanyFeedItem companyFeedItem, CompanyFeedCell cell)
        {
            var titleHeight = companyFeedItem.ContactTitle
                                  ?.StringSize(Appearance.Fonts.LatoBoldWithSize(15), cell.ContactTitle.Frame.Size)
                                  .Height ?? 0;
            var aboutHeight = companyFeedItem.About
                                  ?.StringSize(Appearance.Fonts.LatoWithSize(15),
                                      new CGSize(cell.AboutText.Frame.Width, MaxValue)).Height ?? 0;
            var telePhoneHeight = companyFeedItem.Telephone
                                      ?.StringSize(Appearance.Fonts.LatoWithSize(15), cell.Telephone.Frame.Size)
                                      .Height ?? 0;
            var locationHeight = companyFeedItem.Location
                                     ?.StringSize(Appearance.Fonts.LatoWithSize(15), cell.SpaceLocation.Frame.Size)
                                     .Height ?? 0;
            var websiteHeight = companyFeedItem.Website
                                    ?.StringSize(Appearance.Fonts.LatoWithSize(15), cell.Website.Frame.Size)
                                    .Height ?? 0;

            // About title
            // About text
            // Contact title
            // 
            //
            _companyPostTextHeight = titleHeight + aboutHeight + titleHeight + telePhoneHeight + locationHeight +
                                     websiteHeight;
        }

        #endregion CellText

        #region TextHeight

        private nfloat GetPostCellHeight(IFeedPost post)
        {
            // Top border height             1px
            // Top border <-> Avatar        14px
            // Avatar height                45px
            // Avatar <-> text               8px

            // Text                         CALCULATE
            // MainImage                    CALCULATE

            // Text <-> ruler                8px
            // Ruler height                  1px
            // Button height                50px
            // Bottom border height          1px
            // BottomSpacing (OPTIONAL)      5px
            //----------------------------------
            // Total                        133px

            nfloat finalHeight = 133;

            // Calculate text height
            finalHeight += _textHeights.ContainsKey(post.Id) ? _textHeights[post.Id] : 0;

            if (post.NumberOfImages == 1)
            {
                // 8 spacing below image
                finalHeight += UIScreen.MainScreen.Bounds.Width * 0.6f + 8;
            }
            else if (post.NumberOfImages == 2)
            {
                // 8 spacing below image
                finalHeight += (UIScreen.MainScreen.Bounds.Width - 3) / 2 * 0.6f + 8;
            }
            else if (post.NumberOfImages > 2)
            {
                // 8 spacing below image
                finalHeight += UIScreen.MainScreen.Bounds.Width * 0.6f + 8;
                finalHeight += (UIScreen.MainScreen.Bounds.Width - 3) / 2 * 0.6f + 3;
            }

            return finalHeight;
        }

        private nfloat GetCompanyCellHeight()
        {
            // 280 Header
            // 140 blank space
            // 1 px ruler
            // 10 px spacing
            // ? about title
            // 10 px spacing
            // ? about text
            // 10 px spacing
            // ? contact title
            // 10 px spacing
            // 15 px tel image
            // 10 px spacing
            // 15 px loc image
            // 10 px spacing
            // 15 px web image
            // ---- Social staat uit
            // 20 px spacing
            // 25 px social images
            // ---------------------
            // 15 px spacing
            // 1 px bottomborder
            // 5 px spacing

            nfloat finalHeight = 547;

            return finalHeight + _companyPostTextHeight;
        }

        #endregion TextHeight

        #region Base modifications

        protected override bool EnableTitleBarLogo => true;

        protected override bool EnableCustomBackButton => ViewModel.FeedId != Guid.Empty;

        #endregion Base modifications
    }
}
