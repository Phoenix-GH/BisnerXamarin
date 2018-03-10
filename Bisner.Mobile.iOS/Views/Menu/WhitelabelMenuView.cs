using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.Menu;
using Bisner.Mobile.iOS.Views.Base;
using Cirrious.FluentLayouts.Touch;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Menu
{
    partial class WhitelabelMenuView : ViewBase<IosBaseMenuViewModel>
    {
        #region Constructor

        public WhitelabelMenuView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            Debug.WriteLine("WHITELABELMENUVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var viewFrame = View.Frame;
            viewFrame.Width = (View.Frame.Width / 8) * 6;
            View.Frame = viewFrame;
            View.BackgroundColor = Appearance.Colors.MenuBackground;

            var resetButton = new UIButton();
            resetButton.SetTitle("RESET PASSWORD", UIControlState.Normal);
            resetButton.SetTitleColor(Appearance.Colors.DefaultTextColor, UIControlState.Normal);

            View.Add(resetButton);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                resetButton.WithSameCenterX(View),
                resetButton.WithSameCenterY(View)
                );

            resetButton.TouchUpInside += resetButton_TouchUpInside;

            // Set screen name for analytics
            ScreenName = "GroupsView";
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        void resetButton_TouchUpInside(object sender, EventArgs e)
        {
            Settings.RefreshToken = null;
            Settings.Token = null;
            //Mvx.Resolve<INotificationAlertWindow>().ShowNotification(Mvx.Resolve<IChatMessageRepository>().GetAll().First().ConversationId, "Hey pannekoek dit is echt een fucking lange zin dat is niet normaal joh moet je kijken helemaal buiten het scherm echt bizar man wat de neuk!!!!");
        }

        #endregion ViewController
    }
}
