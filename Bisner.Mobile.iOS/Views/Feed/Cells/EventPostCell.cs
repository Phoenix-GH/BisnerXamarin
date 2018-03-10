//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Bisner.Mobile.Core.Models.Feed;
//using Bisner.Mobile.iOS.Extensions;
//using Cirrious.FluentLayouts.Touch;
//using Foundation;
//using MvvmCross.Binding.BindingContext;
//using MvvmCross.Binding.iOS.Views;
//using MvvmCross.Binding.iOS.Views.Gestures;
//using UIKit;

//namespace Bisner.Mobile.iOS.Views.Feed.Cells
//{
//    partial class EventPostCell : FeedPostCellBase<FeedEventPost>
//    {
//        #region Constructor

//        public static readonly NSString Identifier = new NSString("EventPostCell");

//        private UIImageView _eventImage;
//        private MvxImageViewLoader _imageViewLoader;
//        private UIView _titleContainer;
//        private UILabel _title, _subtitle, _attending;

//        public EventPostCell(IntPtr handle)
//            : base(handle)
//        {
//        }

//        #endregion Constructor

//        #region Cell

//        public override void LayoutSubviews()
//        {
//            base.LayoutSubviews();

//            if (_eventImage.Frame.Size.Width > 0 && !_eventImage.Subviews.Any())
//            {
//                _eventImage.SetOverlay(0.4f);
//            }
//        }

//        #endregion Cell

//        #region Overrides.

//        protected override List<UIView> ControllsToAdd()
//        {
//            _imageViewLoader = new MvxImageViewLoader(() => _eventImage) { DefaultImagePath = NSBundle.MainBundle.PathForResource("Images/background_login", "png") };
//            _eventImage = new UIImageView
//            {
//                ContentMode = UIViewContentMode.ScaleToFill,
//            };

//            _titleContainer = new UIView();

//            _title = new UILabel { Font = iOS.Appearance.Fonts.LatoBlackWithSize(30.0f), TextColor = iOS.Appearance.Colors.White, TextAlignment = UITextAlignment.Center, Lines = 2 };
//            _subtitle = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(14.0f), TextColor = iOS.Appearance.Colors.White, TextAlignment = UITextAlignment.Center };

//            _attending = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(14.0f), TextColor = iOS.Appearance.Colors.SubTextColor };

//            return new List<UIView> { _eventImage, _titleContainer, _title, _subtitle, _attending };
//        }

//        protected override List<FluentLayout> AddContentConstraints(UIView contentContainer)
//        {
//            return new List<FluentLayout>
//            {
//                _eventImage.AtTopOf(contentContainer),
//                _eventImage.WithSameLeft(contentContainer),
//                _eventImage.WithSameRight(contentContainer),
//                _eventImage.Height().EqualTo().WidthOf(_eventImage).WithMultiplier(0.5f),

//                _titleContainer.WithSameCenterX(_eventImage),
//                _titleContainer.WithSameCenterY(_eventImage),

//                _title.AtTopOf(_titleContainer),
//                _title.AtLeftOf(_titleContainer),
//                _title.AtRightOf(_titleContainer),

//                _subtitle.AtBottomOf(_titleContainer),
//                _subtitle.AtLeftOf(_titleContainer),
//                _subtitle.AtRightOf(_titleContainer),
//                _subtitle.Below(_title, 5),

//                _attending.Below(_eventImage, 10),
//                _attending.AtLeftOf(contentContainer, 14),
//                _attending.AtBottomOf(contentContainer, 10)
//            };
//        }

//        protected override void AddBindingsToSet(MvxFluentBindingDescriptionSet<FeedPostCellBase<FeedEventPost>, FeedEventPost> set)
//        {
//            set.Bind(_imageViewLoader).To(vm => vm.EventImageUrl).WithConversion("ImageUrl");
//            set.Bind(_title).To(vm => vm.EventTitle);
//            set.Bind(_subtitle).To(vm => vm.EventDate);
//            set.Bind(_attending).To(vm => vm.AttendeesString);
//            set.Bind(_eventImage.Tap()).For(tap => tap.Command).To(vm => vm.CommentCommand);
//        }

//        #endregion Overrides
//    }
//}
