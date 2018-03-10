using System;
using System.Linq;
using Bisner.Mobile.Core.ViewModels.Dashboard;
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
    public class EventCategoryDoubleCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("EventCategoryDoubleCell");

        private UIView _backpanel, _topBorder;

        private OverlayImageView _background1;
        private UILabel _title1;

        private OverlayImageView _background2;
        private UILabel _title2;


        public EventCategoryDoubleCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;
            SelectionStyle = UITableViewCellSelectionStyle.None;

            _backpanel = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _topBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.BackPanelBorderTop };

            _background1 = new OverlayImageView { ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, Transparency = 0.5f };
            _background1.Layer.CornerRadius = 4f;
            _title1 = new UILabel { Font = iOS.Appearance.Fonts.LatoBlackWithSize(17), TextColor = iOS.Appearance.Colors.White, TextAlignment = UITextAlignment.Center };

            _background2 = new OverlayImageView { ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, Transparency = 0.5f };
            _background2.Layer.CornerRadius = 4f;
            _title2 = new UILabel { Font = iOS.Appearance.Fonts.LatoBlackWithSize(17), TextColor = iOS.Appearance.Colors.White, TextAlignment = UITextAlignment.Center };

            ContentView.AddSubviews(_backpanel, _topBorder, _background1, _title1, _background2, _title2);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _backpanel.AtTopOf(ContentView, 5),
                _backpanel.AtLeftOf(ContentView),
                _backpanel.AtRightOf(ContentView),
                _backpanel.AtBottomOf(ContentView),

                _topBorder.AtTopOf(_backpanel),
                _topBorder.AtLeftOf(_backpanel),
                _topBorder.AtRightOf(_backpanel),
                _topBorder.Height().EqualTo(1),

                _background1.AtTopOf(_backpanel, 7),
                _background1.AtLeftOf(_backpanel, 14),
                _background1.AtBottomOf(_backpanel, 0),

                _title1.WithSameCenterX(_background1),
                _title1.WithSameCenterY(_background1),
                _title1.WithSameRight(_background1),
                _title1.WithSameLeft(_background1),

                _background2.AtTopOf(_backpanel, 7),
                _background2.AtRightOf(_backpanel, 14),
                _background2.AtBottomOf(_backpanel, 0),
                _background2.WithSameWidth(_background1),
                _background2.ToRightOf(_background1, 7),

                _title2.WithSameCenterX(_background2),
                _title2.WithSameRight(_background2),
                _title2.WithSameLeft(_background2),
                _title2.WithSameCenterY(_background2)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<EventCategoryDoubleCell, EventCategoryDouble>();
                set.Bind(_background1).For("ImageUrl").To(vm => vm.Image1).WithConversion("ImageUrl");
                set.Bind(_title1).To(vm => vm.Title1);
                set.Bind(_background1.Tap()).For(t => t.Command).To(vm => vm.Image1Command);
                set.Bind(_background2).For("ImageUrl").To(vm => vm.Image2).WithConversion("ImageUrl");
                set.Bind(_title2).To(vm => vm.Title2);
                set.Bind(_background2.Tap()).For(t => t.Command).To(vm => vm.Image2Command);
                set.Apply();
            });
        }

        #endregion Setup
    }
}