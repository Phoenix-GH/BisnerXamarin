using System;
using System.Linq;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.General.Cells;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class
        EventDetailCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("EventDetailCell");

        private UIImageView _header;

        private UIView _infoContainer, _infoContainerPlaceHolder;
        private UILabel _title;
        private UILabel _time;
        private UILabel _location;

        private UILabel _date;
        private UIView _dateBackPanel;

        private UIButton _attendButton;

        private UIView _infoBackGround;

        private UILabel _attendingLabel;
        private UICollectionView _attendingCollection;
        private MvxCollectionViewSource _attendingCollectionSource;

        private UIView _ruler;

        private UILabel _eventInfoHeader;

        private UILabel _eventDateLabel;
        private UILabel _eventDate;
        private UILabel _eventTimeLabel;
        private UILabel _eventTime;
        private UILabel _eventLocationLabel;
        public UILabel EventLocation;

        private UIView _infoBottomBorder;

        private UIView _imageBackground;
        private UIView _imageTopBorder;
        private UIView _imageBottomBorder;
        private UICollectionView _imageCollection;
        private MvxCollectionViewSource _imageCollectionSource;

        private UIView _aboutBackground;
        private UIView _aboutTopBorder;
        private UIView _aboutRuler;

        private UILabel _aboutHeaderLabel;
        public HtmlTextView AboutText;

        //private FeedButton _commentButton;

        public EventDetailCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Cell

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!_header.Subviews.Any())
            {
                _header.SetOverlay(0.4f);
            }

            foreach (var subview in _header.Subviews)
            {
                var frame = subview.Frame;

                frame.Width = UIScreen.MainScreen.Bounds.Width;
                frame.Height = _header.Frame.Height;

                subview.Frame = frame;
            }
        }

        #endregion Cell

        #region Setup

        private void SetupSubViews()
        {
            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;

            // Header
            _header = new UIImageView
            {
                BackgroundColor = UIColor.Black,
                ContentMode = UIViewContentMode.ScaleAspectFill,
                ClipsToBounds = true
            };

            _date = new UILabel
            {
                TextColor = iOS.Appearance.Colors.DefaultTextColor,
                Font = iOS.Appearance.Fonts.LatoWithSize(13),
                TextAlignment = UITextAlignment.Center
            };
            _dateBackPanel = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _dateBackPanel.Layer.CornerRadius = 4.0f;

            _infoContainer = new UIView();
            _infoContainerPlaceHolder = new UIView();

            _title = new UILabel
            {
                TextColor = iOS.Appearance.Colors.White,
                Font = iOS.Appearance.Fonts.LatoBlackWithSize(24),
                Lines = 2,
                LineBreakMode = UILineBreakMode.TailTruncation
            };
            _time = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoWithSize(15) };
            _location = new UILabel
            {
                TextColor = iOS.Appearance.Colors.White,
                Font = iOS.Appearance.Fonts.LatoWithSize(15),
                Lines = 1,
                LineBreakMode = UILineBreakMode.TailTruncation
            };

            _attendButton = new BlueButton { Font = iOS.Appearance.Fonts.LatoBlackWithSize(18) };
            _attendButton.Layer.ShadowColor = UIColor.Black.CGColor;
            _attendButton.Layer.ShadowRadius = 10f;
            _attendButton.Layer.ShadowOpacity = 0.65f;
            _attendButton.Layer.ShadowOffset = new CGSize(0, 5);

            _infoBackGround = new UIView { BackgroundColor = iOS.Appearance.Colors.White };

            _attendingLabel = new UILabel
            {
                Font = iOS.Appearance.Fonts.LatoWithSize(15),
                TextColor = iOS.Appearance.Colors.SubTextColor
            };
            _attendingCollection = new UICollectionView(ContentView.Frame,
                new UICollectionViewFlowLayout
                {
                    ItemSize = new CGSize(31, 31),
                    ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                    MinimumInteritemSpacing = 0,
                    MinimumLineSpacing = 5
                })
            {
                AllowsSelection = false,
                ScrollEnabled = false,
                BackgroundColor = UIColor.Clear,
            };

            _attendingCollection.RegisterClassForCell(typeof(CollectionUserCell), CollectionUserCell.Identifier);
            _attendingCollection.Source =
                _attendingCollectionSource =
                    new MvxCollectionViewSource(_attendingCollection, CollectionUserCell.Identifier);

            _ruler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _eventInfoHeader = new UILabel
            {
                Font = iOS.Appearance.Fonts.LatoWithSize(15),
                TextColor = iOS.Appearance.Colors.SubTextColor,
            };
            _eventDateLabel = new UILabel
            {
                TextColor = iOS.Appearance.Colors.DefaultTextColor,
                Font = iOS.Appearance.Fonts.LatoBoldWithSize(14),
            };
            _eventDate = new UILabel
            {
                TextColor = iOS.Appearance.Colors.ChatMessageColor,
                Font = iOS.Appearance.Fonts.LatoWithSize(14)
            };
            _eventTimeLabel = new UILabel
            {
                TextColor = iOS.Appearance.Colors.DefaultTextColor,
                Font = iOS.Appearance.Fonts.LatoBoldWithSize(14),
            };
            _eventTime = new UILabel
            {
                TextColor = iOS.Appearance.Colors.ChatMessageColor,
                Font = iOS.Appearance.Fonts.LatoWithSize(14)
            };
            _eventLocationLabel = new UILabel
            {
                TextColor = iOS.Appearance.Colors.DefaultTextColor,
                Font = iOS.Appearance.Fonts.LatoBoldWithSize(14),
            };
            EventLocation = new UILabel
            {
                TextColor = iOS.Appearance.Colors.ChatMessageColor,
                Font = iOS.Appearance.Fonts.LatoWithSize(14),
                Lines = 0,
            };

            _infoBottomBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _imageTopBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _imageBackground = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _imageBottomBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _imageCollection = new UICollectionView(ContentView.Frame,
                new UICollectionViewFlowLayout
                {
                    ItemSize = new CGSize(105, 105),
                    ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                    MinimumInteritemSpacing = 0,
                    MinimumLineSpacing = 14
                })
            {
                AllowsSelection = true,
                ScrollEnabled = true,
                BackgroundColor = UIColor.White,
                ShowsHorizontalScrollIndicator = false,
            };

            _imageCollection.RegisterClassForCell(typeof(ImageCollectionCell), ImageCollectionCell.Identifier);
            _imageCollection.Source = _imageCollectionSource = new GenericCollectionViewSource<IImage>(_imageCollection, ImageCollectionCell.Identifier)
            {
                ModifyCellFunc = (view, path, cell, item) =>
                {
                    var imageCollectionCell = cell as ImageCollectionCell;

                    if (imageCollectionCell != null)
                    {
                        imageCollectionCell.CornerRadius = 8f;
                    }
                }
            };

            _aboutBackground = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _aboutTopBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _aboutRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _aboutHeaderLabel = new UILabel
            {
                Font = iOS.Appearance.Fonts.LatoBoldWithSize(14),
                TextColor = iOS.Appearance.Colors.DefaultTextColor,
            };
            AboutText = new HtmlTextView();

            //_commentButton = new FeedButton { ImageTopSpacing = 3 };

            ContentView.AddSubviews(_header, _infoContainer, _infoContainerPlaceHolder, _title, _time, _location,
                _dateBackPanel, _date, _attendButton, _infoBackGround, _attendingLabel, _attendingCollection, _ruler,
                _eventInfoHeader, _eventDate, _eventDateLabel, _eventTime, _eventTimeLabel, _eventLocationLabel,
                EventLocation, _infoBottomBorder, _imageBackground, _imageCollection, _imageTopBorder,
                _imageBottomBorder,
                _aboutBackground, _aboutTopBorder, AboutText, _aboutHeaderLabel, _aboutRuler);//, _commentButton);
        }

        private NSLayoutConstraint _imageBackgroundHeightConstraint, _imageBackgroundTopSpacingConstraint;

        private NSLayoutConstraint _attendingHeight;

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            _imageBackgroundTopSpacingConstraint =
                _imageBackground.Below(_infoBackGround, 5).ToLayoutConstraints().First();
            _imageBackgroundHeightConstraint = _imageBackground.Height().EqualTo(134).ToLayoutConstraints().First();

            _attendingHeight = _ruler.Below(_header, 125).ToLayoutConstraints().First();

            ContentView.AddConstraint(_imageBackgroundHeightConstraint);
            ContentView.AddConstraint(_imageBackgroundTopSpacingConstraint);
            ContentView.AddConstraint(_attendingHeight);

            ContentView.AddConstraints(
                _header.AtTopOf(ContentView),
                _header.AtLeftOf(ContentView),
                _header.AtRightOf(ContentView),
                _header.Height().EqualTo(345),

                _dateBackPanel.AtTopOf(ContentView, 20),
                _dateBackPanel.AtRightOf(ContentView, 20),
                _dateBackPanel.Width().EqualTo(90),
                _dateBackPanel.Height().EqualTo(25),

                _date.WithSameCenterY(_dateBackPanel),
                _date.WithSameCenterX(_dateBackPanel),

                _infoContainerPlaceHolder.Below(_dateBackPanel),
                _infoContainerPlaceHolder.Above(_attendButton),
                _infoContainerPlaceHolder.AtLeftOf(_header),
                _infoContainerPlaceHolder.AtRightOf(_header),

                _infoContainer.AtLeftOf(_header),
                _infoContainer.AtRightOf(_header),
                _infoContainer.WithSameCenterY(_infoContainerPlaceHolder),

                _title.AtTopOf(_infoContainer),
                _title.AtRightOf(_infoContainer, 20),
                _title.AtLeftOf(_infoContainer, 20),

                _time.Below(_title, 5),
                _time.AtLeftOf(_infoContainer, 20),
                _time.AtRightOf(_infoContainer, 20),

                _location.Below(_time, 5),
                _location.AtLeftOf(_infoContainer, 20),
                _location.AtRightOf(_infoContainer, 20),
                _location.Below(_infoContainer),

                _attendButton.Height().EqualTo(45),
                _attendButton.AtBottomOf(_header, 39),
                _attendButton.AtLeftOf(_header, 39),
                _attendButton.AtRightOf(_header, 39),

                _infoBackGround.Below(_header),
                _infoBackGround.AtLeftOf(ContentView),
                _infoBackGround.AtRightOf(ContentView),

                _attendingLabel.Below(_header, 30),
                _attendingLabel.AtLeftOf(ContentView, 14),
                _attendingLabel.AtRightOf(ContentView, 14),

                _attendingCollection.Below(_attendingLabel, 13),
                _attendingCollection.AtLeftOf(ContentView, 14),
                _attendingCollection.AtRightOf(ContentView, 14),
                _attendingCollection.Height().EqualTo(31),

                _ruler.AtLeftOf(ContentView, 10),
                _ruler.AtRightOf(ContentView, 10),
                _ruler.Height().EqualTo(1),

                _eventInfoHeader.Below(_ruler, 13),
                _eventInfoHeader.AtLeftOf(ContentView, 14),
                _eventInfoHeader.Height().EqualTo(20),

                _eventDateLabel.Below(_eventInfoHeader, 14),
                _eventDateLabel.AtLeftOf(ContentView, 14),
                _eventDateLabel.Height().EqualTo(20),

                _eventDate.AtLeftOf(ContentView, 100),
                _eventDate.WithSameCenterY(_eventDateLabel),

                _eventTimeLabel.Below(_eventDateLabel, 7),
                _eventTimeLabel.AtLeftOf(ContentView, 14),

                _eventTime.WithSameCenterY(_eventTimeLabel),
                _eventTime.WithSameLeft(_eventDate),
                _eventTime.AtRightOf(ContentView, 14),

                _eventLocationLabel.Below(_eventTimeLabel, 7),
                _eventLocationLabel.AtLeftOf(ContentView, 14),

                EventLocation.WithSameTop(_eventLocationLabel),
                EventLocation.WithSameLeft(_eventDate),
                EventLocation.AtRightOf(ContentView, 14),

                _infoBottomBorder.Below(EventLocation, 30),
                _infoBottomBorder.Height().EqualTo(1),
                _infoBottomBorder.AtLeftOf(ContentView),
                _infoBottomBorder.AtRightOf(ContentView),
                _infoBottomBorder.AtBottomOf(_infoBackGround),

                _imageBackground.AtLeftOf(ContentView),
                _imageBackground.AtRightOf(ContentView),

                _imageTopBorder.AtTopOf(_imageBackground),
                _imageTopBorder.AtLeftOf(ContentView),
                _imageTopBorder.AtRightOf(ContentView),
                _imageTopBorder.Height().EqualTo(1),

                _imageBottomBorder.AtBottomOf(_imageBackground),
                _imageBottomBorder.AtLeftOf(ContentView),
                _imageBottomBorder.AtRightOf(ContentView),
                _imageBottomBorder.Height().EqualTo(1),

                _imageCollection.AtTopOf(_imageBackground),
                _imageCollection.AtLeftOf(_imageBackground, 14),
                _imageCollection.AtRightOf(_imageBackground, 14),
                _imageCollection.AtBottomOf(_imageBackground),

                _aboutBackground.Below(_imageBackground, 5),
                _aboutBackground.AtLeftOf(ContentView),
                _aboutBackground.AtRightOf(ContentView),

                _aboutHeaderLabel.AtTopOf(_aboutBackground, 14),
                _aboutHeaderLabel.AtLeftOf(_aboutBackground, 14),
                _aboutHeaderLabel.AtRightOf(_aboutBackground, 14),
                _aboutHeaderLabel.Height().EqualTo(20),

                AboutText.Below(_aboutHeaderLabel, 13),
                AboutText.AtLeftOf(_aboutBackground, 12),
                AboutText.AtRightOf(_aboutBackground, 12),

                _aboutTopBorder.AtTopOf(_aboutBackground),
                _aboutTopBorder.Height().EqualTo(1),
                _aboutTopBorder.AtLeftOf(_aboutBackground),
                _aboutTopBorder.AtRightOf(_aboutBackground),

                _aboutRuler.Below(AboutText, 10),
                _aboutRuler.AtLeftOf(_aboutBackground),
                _aboutRuler.AtRightOf(_aboutBackground),
                _aboutRuler.Height().EqualTo(1),
                _aboutRuler.AtBottomOf(_aboutBackground)

                //_commentButton.Below(_aboutRuler),
                //_commentButton.AtLeftOf(ContentView, 14),
                //_commentButton.Height().EqualTo(50),
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<EventDetailCell, IEvent>();
                set.Bind(_header).For("ImageUrl").To(vm => vm.Header.Medium).WithConversion("ImageUrl");
                set.Bind(_date).To(vm => vm.DateTime).WithConversion("ShortDate");
                set.Bind(_title).To(vm => vm.Title);
                set.Bind(_time).To(vm => vm.DateTimeText);
                set.Bind(_location).To(vm => vm.Location);
                set.Bind(_attendButton).For("Title").To(vm => vm.IsAttendingString);
                set.Bind(_attendButton).To(vm => vm.AttendCommand);
                set.Bind(_attendingLabel).To(vm => vm.NumberAttendingString);
                set.Bind(_attendingCollectionSource).To(vm => vm.Attendees);
                set.Bind(_eventDate).To(vm => vm.DateTime).WithConversion("LongDate");
                set.Bind(_eventTime).To(vm => vm.DateTimeText);
                set.Bind(EventLocation).To(vm => vm.Location);
                set.Bind(_imageCollectionSource).To(vm => vm.Images);
                set.Bind(_eventInfoHeader).To(vm => vm.EventInfoHeaderText);
                set.Bind(_eventDateLabel).To(vm => vm.EventDateLabel);
                set.Bind(_eventTimeLabel).To(vm => vm.EventTimeLabel);
                set.Bind(_eventLocationLabel).To(vm => vm.EventLocationLabel);
                set.Bind(_aboutHeaderLabel).To(vm => vm.AboutHeaderLabel);

                //set.Bind(_commentButton).For("Comment").To(vm => vm.HasCommented);
                //set.Bind(_commentButton).For("Title").To(item => item.CommentButtonText);
                set.Apply();
            });
        }

        #endregion Setup

        #region Modifications

        public void SetImagesVisible(bool visible)
        {
            if (visible)
            {
                _imageBackgroundHeightConstraint.Constant = 134;
                _imageBackgroundTopSpacingConstraint.Constant = 5;
                _imageBackground.Hidden = false;
                _imageBottomBorder.Hidden = false;
                _imageTopBorder.Hidden = false;
            }
            else
            {
                _imageBackgroundHeightConstraint.Constant = 0;
                _imageBackgroundTopSpacingConstraint.Constant = 0;
                _imageBackground.Hidden = true;
                _imageBottomBorder.Hidden = true;
                _imageTopBorder.Hidden = true;
            }
        }

        public void SetAttendeesVisible(bool visible)
        {
            if (visible)
            {
                _attendingHeight.Constant = 125;
                _attendingCollection.Hidden = false;
                _attendingLabel.Hidden = false;
                _ruler.Hidden = false;
            }
            else
            {
                _attendingHeight.Constant = 0;
                _attendingCollection.Hidden = true;
                _attendingLabel.Hidden = true;
                _ruler.Hidden = true;
            }
        }

        #endregion Modifications
    }
}