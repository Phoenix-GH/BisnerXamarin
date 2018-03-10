using Bisner.Mobile.iOS.Controls.Gestures;
using Bisner.Mobile.iOS.Controls.SlidingPanels;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public class NonTransluscentUINavigationController : UINavigationController
    {
        public NonTransluscentUINavigationController()
        {

        }

        public NonTransluscentUINavigationController(UIViewController viewController)
            : base(viewController)
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            
            NavigationBar.Translucent = false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }
    }

    public class NonTranslucentSlidingPanelsNavigationController : SlidingPanelsNavigationViewController
    {
        public NonTranslucentSlidingPanelsNavigationController(UIViewController viewController)
            : base(viewController)
        {
        }

        public override void LoadView()
        {
            base.LoadView();

            NavigationBar.Translucent = false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }
    }
}
