using System;
using System.Linq;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class EventCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("EventCell");

        private OverlayImageView _background;
        private UILabel _name, _time, _location, _date;
        private UIView _dateBackPanel, _infoContainer, _infoContainerPlaceholder;

        public EventCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            _background = new OverlayImageView { BackgroundColor = UIColor.Black, ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, Transparency = 0.5f };

            _infoContainer = new UIView();
            _infoContainerPlaceholder = new UIView();

            _name = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoBlackWithSize(22), Lines = 1, LineBreakMode = UILineBreakMode.TailTruncation };
            _time = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoWithSize(13) };
            _location = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoWithSize(13), Lines = 1, LineBreakMode = UILineBreakMode.TailTruncation };
            _date = new UILabel { TextColor = iOS.Appearance.Colors.DefaultTextColor, Font = iOS.Appearance.Fonts.LatoWithSize(13), TextAlignment = UITextAlignment.Center };
            _dateBackPanel = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _dateBackPanel.Layer.CornerRadius = 4.0f;

            ContentView.AddSubviews(_background, _infoContainerPlaceholder, _infoContainer, _name, _time, _location, _dateBackPanel, _date);

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _background.AtLeftOf(ContentView).WithIdentifier("BackgroundAtLeftOfContentView"),
                _background.AtRightOf(ContentView).WithIdentifier("BackgroundAtRightOfContentView"),
                _background.AtTopOf(ContentView).WithIdentifier("BackgroundAtTopOfContentView"),
                _background.AtBottomOf(ContentView, 2).WithIdentifier("BackgroundAtBottomOfContentView"),

                _dateBackPanel.AtTopOf(ContentView, 20).WithIdentifier("DateBackPanelAtTopOfContentView"),
                _dateBackPanel.AtRightOf(ContentView, 20).WithIdentifier("DateBackPanelAtRightOfContentView"),
                _dateBackPanel.Width().EqualTo(90).WithIdentifier("DateBackPanelWidth"),
                _dateBackPanel.Height().EqualTo(25).WithIdentifier("DateBackPanelHeight"),

                _date.WithSameCenterY(_dateBackPanel).WithIdentifier("DateAtRightOfContentView"),
                _date.WithSameCenterX(_dateBackPanel).WithIdentifier("DateAtBottomOfContentView"),

                _infoContainerPlaceholder.AtTopOf(_background),
                _infoContainerPlaceholder.AtLeftOf(_background),
                _infoContainerPlaceholder.AtRightOf(_background),
                _infoContainerPlaceholder.AtBottomOf(_background),

                _infoContainer.AtLeftOf(_infoContainerPlaceholder).WithIdentifier("InfoContainerAtLeftOfBackground"),
                _infoContainer.AtRightOf(_infoContainerPlaceholder).WithIdentifier("InforContainerAtRightOfBackground"),
                _infoContainer.WithSameCenterY(_infoContainerPlaceholder),

                _name.AtLeftOf(_infoContainer, 20).WithIdentifier("NameAtLeftOfBackgorund"),
                _name.AtRightOf(_infoContainer, 20).WithIdentifier("NameAtRightBackground"),
                _name.AtTopOf(_infoContainer, 4),

                _time.Below(_name, 5).WithIdentifier("TimeAboveLocation"),
                _time.AtLeftOf(_infoContainer, 20).WithIdentifier("TimeAtLeftOfBackground"),
                _time.AtRightOf(_infoContainer, 20).WithIdentifier("TimeAtRightOfBackground"),

                _location.Below(_time, 3),
                _location.AtBottomOf(_infoContainer),
                _location.AtLeftOf(_infoContainer, 20),
                _location.AtRightOf(_infoContainer, 20)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<EventCell, IEvent>();
                set.Bind(_name).To(vm => vm.Title);
                set.Bind(_time).To(vm => vm.DateTimeText);
                set.Bind(_date).To(vm => vm.DateTime).WithConversion("ShortDate");
                set.Bind(_background).For("ImageUrl").To(vm => vm.Header.Medium).WithConversion("ImageUrl");
                set.Bind(_location).To(vm => vm.Location);
                set.Bind(_infoContainerPlaceholder.Tap()).For(tap => tap.Command).To(vm => vm.ShowEventCommand);
                set.Bind(_name.Tap()).For(tap => tap.Command).To(vm => vm.ShowEventCommand);
                set.Bind(_time.Tap()).For(tap => tap.Command).To(vm => vm.ShowEventCommand);
                set.Bind(_date.Tap()).For(tap => tap.Command).To(vm => vm.ShowEventCommand);
                set.Bind(_location.Tap()).For(tap => tap.Command).To(vm => vm.ShowEventCommand);
                set.Bind(_infoContainer.Tap()).For(tap => tap.Command).To(vm => vm.ShowEventCommand);
                set.Apply();
            });
        }

        #endregion Setup
    }
}