using Bisner.Mobile.Core.Models.Chat;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using static Bisner.Mobile.iOS.Appearance;

namespace Bisner.Mobile.iOS.Views.Chat.Cells
{
    public class ChatLabelCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("ChatLabelCell");

        private UIView _background;
        private UILabel _text;

        public ChatLabelCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            BackgroundColor = UIColor.Clear;

            _background = new UIView { BackgroundColor = Colors.HeaderGreen };
            _background.Layer.CornerRadius = 8.0f;

            _text = new UILabel { TextColor = Colors.White, Font = Fonts.LatoBoldWithSize(10) };

            ContentView.AddSubviews(_background, _text);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _background.WithSameCenterX(ContentView),
                _background.AtTopOf(ContentView),
                _background.AtBottomOf(ContentView),

                _text.AtTopOf(_background, 2),
                _text.AtBottomOf(_background, 2),
                _text.WithSameCenterX(_background),
                _text.AtLeftOf(_background, 10),
                _text.AtRightOf(_background, 10)
                );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ChatLabelCell, ChatLabel>();
            set.Bind(_text).To(vm => vm.Text);
            set.Apply();
        }

        #endregion Setup
    }
}
