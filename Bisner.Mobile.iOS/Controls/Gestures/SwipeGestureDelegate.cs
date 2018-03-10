using UIKit;

namespace Bisner.Mobile.iOS.Controls.Gestures
{
    public class SwipeGestureDelegate : UIGestureRecognizerDelegate
    {
        public override bool ShouldBeRequiredToFailBy(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            return gestureRecognizer is UIScreenEdgePanGestureRecognizer;
        }
    }
}