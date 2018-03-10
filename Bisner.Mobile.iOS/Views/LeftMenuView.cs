using System;
using Bisner.Mobile.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;

namespace Bisner.Mobile.iOS.Views
{
    partial class LeftMenuView : MvxViewController<LeftMenuViewModel>
    {
        #region Constructor 

        public LeftMenuView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region Lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var viewFrame = View.Frame;
            viewFrame.Width = (View.Frame.Width / 8) * 6;
            View.Frame = viewFrame;
            View.BackgroundColor = Appearance.Colors.MenuBackground;
        }

        #endregion Lifecycle
    }
}
