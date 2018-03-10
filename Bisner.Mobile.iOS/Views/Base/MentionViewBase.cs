using System;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Base
{
    public class MentionViewBase<TViewModel> : HideTabBarViewBase<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Constructor

        private UITableView _mentionsTable;
        private GenericTableViewSource<MentionUser> _source;
        private NSLayoutConstraint _mentionsTableTopConstraint;

        public MentionViewBase(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (ViewModel is MentionViewModelBase)
            {
                // Add mentions table
                SetupTable();
                SetupConstraints();
                SetupBindings();

                _originalMentionsTopConstant = _mentionsTableTopConstraint.Constant;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MentionViewModel.OnShowMentions = ShowMentions;
            MentionViewModel.OnHideMentions = HideMentions;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            MentionViewModel.OnShowMentions = null;
            MentionViewModel.OnHideMentions = null;
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _mentionsTable = new UITableView();

            _mentionsTable.RegisterClassForCellReuse(typeof(MentionCell), MentionCell.Identifier);
            _mentionsTable.EstimatedRowHeight = 58;
            _mentionsTable.RowHeight = UITableView.AutomaticDimension;
            _mentionsTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _mentionsTable.Bounces = false;

            _source = new GenericTableViewSource<MentionUser>(_mentionsTable)
            {
                GetIdentifierFunc = (user, path) => MentionCell.Identifier,
                RowSelectionFunc = (view, path, arg3) =>
                {
                    MentionViewModel.MentionSelected(arg3);
                },
            };
            _mentionsTable.Source = _source;

            _mentionsTable.Frame = new CGRect(0, 0, 100, 100);

            View.AddSubviews(_mentionsTable);
        }

        private void SetupConstraints()
        {
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            var viewToPlaceMentions = ViewToPlaceMentions() ?? View;

            _mentionsTableTopConstraint = NSLayoutConstraint.Create(_mentionsTable, NSLayoutAttribute.Top, NSLayoutRelation.Equal, viewToPlaceMentions, NSLayoutAttribute.Bottom, 1, 0);

            View.AddConstraints(
                _mentionsTable.AtLeftOf(View),
                _mentionsTable.AtRightOf(View),
                // Table offset is 3 cells with a height of 58 = 174
                _mentionsTable.Height().EqualTo(174)
                );

            View.AddConstraint(_mentionsTableTopConstraint);
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<MentionViewBase<TViewModel>, MentionViewModelBase>();
            set.Bind(_source).To(vm => vm.MentionUsers);
            set.Apply();

            _mentionsTable.ReloadData();
        }

        private MentionViewModelBase MentionViewModel => ViewModel as MentionViewModelBase;

        #endregion Setup

        #region Mentions

        protected virtual UIView ViewToPlaceMentions()
        {
            return null;
        }

        private void ShowMentions()
        {
            InvokeOnMainThread(() =>
            {
                //Start an animation
                UIView.Animate(0.25, () =>
                {

                    View.SetNeedsUpdateConstraints();

                    _mentionsTableTopConstraint.Constant -= 172;

                    View.LayoutIfNeeded();
                });
            });
        }

        private void HideMentions()
        {
            InvokeOnMainThread(() =>
            {
                //Start an animation
                UIView.Animate(0.25, () =>
                {
                    View.SetNeedsUpdateConstraints();

                    _mentionsTableTopConstraint.Constant += 172;

                    View.LayoutIfNeeded();
                });
            });
        }

        #endregion Mentions

        #region Keyboard

        private nfloat _originalMentionsTopConstant;

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            InvokeOnMainThread(() =>
            {
                View.SetNeedsLayout();

                if (visible)
                {
                    _mentionsTableTopConstraint.Constant = _originalMentionsTopConstant - keyboardHeight;
                }
                else
                {
                    _mentionsTableTopConstraint.Constant = _originalMentionsTopConstant;
                }

                View.LayoutIfNeeded();
            });
        }

        #endregion Keyboard

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _mentionsTable.Dispose();
                _mentionsTable = null;
                _source.Dispose();
                _source = null;
                _mentionsTableTopConstraint.Dispose();
                _mentionsTable = null;
            }
        }

        #endregion Dispose
    }

    public class MentionCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("MentionCell");

        private UIView _topRuler;
        private AvatarImageView _avatar;
        private UILabel _displayLabel;
        
        public MentionCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            _topRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _avatar = new AvatarImageView();
            _displayLabel = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            
            ContentView.AddSubviews(_topRuler, _avatar, _displayLabel);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _topRuler.AtTopOf(ContentView),
                _topRuler.AtLeftOf(ContentView),
                _topRuler.AtRightOf(ContentView),
                _topRuler.Height().EqualTo(1),

                _avatar.WithSameCenterY(ContentView),
                _avatar.AtLeftOf(ContentView, 14),
                _avatar.Height().EqualTo(30),
                _avatar.Width().EqualTo(30),

                _displayLabel.WithSameCenterY(_avatar),
                _displayLabel.ToRightOf(_avatar, 10)
            );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<MentionCell, MentionUser>();
            set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.User.Avatar.Small).WithConversion("ImageUrl");
            set.Bind(_displayLabel).To(vm => vm.User.DisplayName);
            set.Apply();
        }

        #endregion Setup
    }
}