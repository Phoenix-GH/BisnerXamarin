using System;
using Bisner.Mobile.Core.ViewModels.Workspace;
using MvvmCross.iOS.Views;

namespace Bisner.Mobile.iOS.Views.Workspace
{
    partial class WorkspacesView : MvxViewController<WorkspacesViewModel>
    {
        #region Constructor

        public WorkspacesView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var viewFrame = View.Frame;
            viewFrame.Width = (View.Frame.Width / 8) * 6;
            View.Frame = viewFrame;
            View.BackgroundColor = Appearance.Colors.MenuBackground;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        #endregion ViewController
    }
}
