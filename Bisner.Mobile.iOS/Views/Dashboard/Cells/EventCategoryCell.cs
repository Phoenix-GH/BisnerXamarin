using System;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class EventCategoryCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("EventCategoryCell");

        private UIImageView _arrow;
        private UILabel _title;
        private UIView _bottomRuler;

        public EventCategoryCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            using (
                var arrow =
                    UIImage.FromBundle("Icons/arrow_right_light.png").ImageWithColor(UIColor.FromRGB(197, 197, 197)))
            {
                _arrow = new UIImageView(arrow) { ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true };
            }
            _title = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(14), TextColor = iOS.Appearance.Colors.ChatMessageColor };
            _bottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            ContentView.AddSubviews(_arrow, _title, _bottomRuler);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _arrow.WithSameCenterY(ContentView),
                _arrow.AtRightOf(ContentView, 14),
                _arrow.Height().EqualTo(15),
                _arrow.Width().EqualTo(9),

                _title.WithSameCenterY(ContentView),
                _title.AtLeftOf(ContentView, 14),

                _bottomRuler.AtBottomOf(ContentView),
                _bottomRuler.AtLeftOf(ContentView),
                _bottomRuler.AtRightOf(ContentView),
                _bottomRuler.Height().EqualTo(1)
            );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<EventCategoryCell, IEventCategory>();
                set.Bind(_title).To(vm => vm.Name);
                set.Bind(_arrow.Tap()).For(tap => tap.Command).To(vm => vm.SelectedCommand);
                set.Bind(ContentView.Tap()).For(tap => tap.Command).To(vm => vm.SelectedCommand);
                set.Apply();
            });
        }

        #endregion Setup
    }
}