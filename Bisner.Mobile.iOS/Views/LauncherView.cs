using System;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.iOS.Views.Base;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views
{
    public partial class LauncherView : ViewBase<LauncherViewModel>
    {
        #region Constructor

        public LauncherView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            NavigationController.NavigationBarHidden = false;
        }

        #endregion ViewController
    }
}