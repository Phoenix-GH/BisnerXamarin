using System;
using System.Diagnostics;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.iOS.Controls.Gestures;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Feed.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed
{
    partial class AddPostView : MentionViewBase<AddPostViewModel>
    {
        #region Constructor

        private UIBarButtonItem _postMenuItem, _postIndicatorItem, _closeButton;

        private UIActivityIndicatorView _postingIndicator;

        private MvxCollectionViewSource _source;

        public AddPostView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("ADDPOSTVIEW RECIEVED MEMORY WARNING!!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupNavigationItems();
            SetupImageButton();
            SetupCollectionView();
            SetupBindings();

            ViewModel.AfterPostAction = () =>
            {
                NavigationController.PopViewController(true);
                ViewModel.AfterPostAction = null;
            };

            // Set the navigation bar hidden so we don't get a double nav bar
            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            _originalConstraintConstant = BottomConstraint.Constant;
            
            Input.AutocorrectionType = UITextAutocorrectionType.No;

            // Set screen name for analytics
            ScreenName = "AddPostView feedId = " + ViewModel.FeedId;
        }

        #endregion ViewController

        #region Setup

        private void SetupNavigationItems()
        {
            // Add back button
            using (
                var closeImage =
                    UIImage.FromBundle("Icons/close.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                _closeButton = new UIBarButtonItem(closeImage, UIBarButtonItemStyle.Plain, null, null);
            }

            NavigationController.InteractivePopGestureRecognizer.Delegate = new SwipeGestureDelegate();
            NavigationItem.SetLeftBarButtonItem(_closeButton, true);

            // Add Post button
            _postMenuItem = new UIBarButtonItem
            {
                Title = Settings.GetResource(ResKeys.mobile_post_btn_post),
            };

            var icoFontAttribute = new UITextAttributes { Font = Appearance.Fonts.LatoBoldWithSize(24), TextColor = Appearance.Colors.BisnerBlue };
            _postMenuItem.SetTitleTextAttributes(icoFontAttribute, UIControlState.Application);
            _postMenuItem.Style = UIBarButtonItemStyle.Done;

            // Post indicator
            _postingIndicator = new UIActivityIndicatorView { Color = Appearance.Colors.BisnerBlue };
            _postIndicatorItem = new UIBarButtonItem(_postingIndicator);
            _postingIndicator.StartAnimating();

            NavigationItem.SetRightBarButtonItems(new[] { _postMenuItem }, true);
        }

        private void CloseButtonOnClicked(object sender, EventArgs eventArgs)
        {
            NavigationController.PopViewController(true);
        }

        private void SetupImageButton()
        {
            using (
                var image =
                    UIImage.FromBundle("Icons/add_image.png")
                        .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                PickImage.SetBackgroundImage(image, UIControlState.Normal);
            }

            using (
                var image2 =
                    UIImage.FromBundle("Icons/icon_camera.png")
                        .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                TakeImage.SetBackgroundImage(image2, UIControlState.Normal);
            }

            using (
                var image3 =
                    UIImage.FromBundle("Icons/icon_mention.png")
                        .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                MentionUser.SetBackgroundImage(image3, UIControlState.Normal);
            }
        }

        private void SetupCollectionView()
        {
            _source = new AddPostCollectionSource(ImageCollection, SelectedImageCell.Identifier, new WeakReference<AddPostView>(this));
            ImageCollection.RegisterClassForCell(typeof(SelectedImageCell), SelectedImageCell.Identifier);

            var collectionLayout = new SpacingCollectionFlowLayout { ItemSize = new CGSize(100, 110), };

            ImageCollection.CollectionViewLayout = collectionLayout;
            ImageCollection.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<AddPostView, AddPostViewModel>();
            set.Bind(NavigationItem.LeftBarButtonItem).For(b => b.Enabled).To(vm => vm.IsNotPosting);
            set.Bind(_postMenuItem).To(vm => vm.SendCommand);
            set.Bind(NavigationItem.RightBarButtonItem).For(b => b.Enabled).To(vm => vm.IsNotPosting);
            set.Bind(Avatar).For("AvatarImageUrl").To(vm => vm.AvatarUrl).WithConversion("ImageUrl");
            set.Bind(Input).For(i => i.Placeholder).To(vm => vm.PlaceholderText);
            set.Bind(Input).To(vm => vm.Input);
            set.Bind(Input).For(i => i.Editable).To(vm => vm.IsNotPosting);
            set.Bind(PickImage).To(vm => vm.ChoosePictureCommand);
            set.Bind(PickImage).For(b => b.Enabled).To(vm => vm.IsNotPosting);
            set.Bind(TakeImage).To(vm => vm.TakePictureCommand);
            set.Bind(TakeImage).For(b => b.Enabled).To(vm => vm.IsNotPosting);
            set.Bind(MentionUser).To(vm => vm.MentionCommand);
            set.Bind(MentionUser).For(b => b.Enabled).To(vm => vm.IsNotPosting);
            set.Bind(_source).To(vm => vm.SelectedImages);
            set.Apply();
        }

        #endregion Setup

        #region Appear

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ViewModel.StartPosting += OnStartPosting;
            ViewModel.StopPosting += OnStopPosting;

            if (_closeButton != null)
            {
                _closeButton.Clicked += CloseButtonOnClicked;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ViewModel.StartPosting -= OnStartPosting;
            ViewModel.StopPosting -= OnStopPosting;

            if (_closeButton != null)
            {
                _closeButton.Clicked -= CloseButtonOnClicked;
            }
        }

        private void OnStartPosting()
        {
            InvokeOnMainThread(() =>
            {
                NavigationItem.SetHidesBackButton(true, true);
                NavigationItem.SetRightBarButtonItems(new[] { _postIndicatorItem }, true);
            });
        }

        private void OnStopPosting()
        {
            InvokeOnMainThread(() =>
            {
                NavigationItem.SetHidesBackButton(false, true);
                NavigationItem.SetRightBarButtonItems(new[] { _postMenuItem }, true);
            });
        }

        #endregion Appear

        #region Keyboard

        public override bool HandlesKeyboardNotifications=>true;
        
        private nfloat _originalConstraintConstant;

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            View.SetNeedsUpdateConstraints();

            base.OnKeyboardChanged(visible, keyboardHeight);

            if (visible)
            {
                BottomConstraint.Constant = keyboardHeight + _originalConstraintConstant - TabBarController.TabBar.Frame.Height;
            }
            else
            {
                BottomConstraint.Constant = _originalConstraintConstant - TabBarController.TabBar.Frame.Height;
            }

            View.LayoutIfNeeded();
        }

        #endregion Keyboard

        #region Tabbar

        protected override bool SupportsHideTabBar => true;

        protected override NSLayoutConstraint TabBarTopConstraint => BottomConstraint;

        #endregion Tabbar

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _postMenuItem.Dispose();
                _postIndicatorItem.Dispose();
                _closeButton.Dispose();
                Avatar.Dispose();
                _postingIndicator.Dispose();
                _source.Dispose();
            }
        }

        #endregion Dispose
    }

    internal class AddPostCollectionSource : MvxCollectionViewSource
    {
        private readonly WeakReference<AddPostView> _parent;

        public AddPostCollectionSource(UICollectionView collectionView, WeakReference<AddPostView> parent) : base(collectionView)
        {
            _parent = parent;
        }

        public AddPostCollectionSource(UICollectionView collectionView, NSString defaultCellIdentifier, WeakReference<AddPostView> parent) : base(collectionView, defaultCellIdentifier)
        {
            _parent = parent;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            base.ItemSelected(collectionView, indexPath);

            var item = GetItemAt(indexPath);

            AddPostView parent;

            if (_parent.TryGetTarget(out parent))
            {
                parent.ViewModel.RemoveImage(item as SelectedImage);
            }
        }
    }

    public class SpacingCollectionFlowLayout : UICollectionViewFlowLayout
    {
        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var attributes = base.LayoutAttributesForElementsInRect(rect);

            for (var i = 1; i < attributes.Length; i++)
            {
                var currentLayoutAttributes = attributes[i];
                var prevLayoutAttributes = attributes[i - 1];
                var maximumSpacing = 4;
                var origin = prevLayoutAttributes.Frame.GetMaxX();

                if (origin + maximumSpacing + currentLayoutAttributes.Frame.Size.Width < CollectionViewContentSize.Width)
                {
                    var frame = currentLayoutAttributes.Frame;
                    frame.X = origin + maximumSpacing;
                    currentLayoutAttributes.Frame = frame;
                }
            }

            return attributes;
        }
    }
}
