using System;
using System.Collections.Generic;
using System.Linq;
using Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.SlidingPanels
{
    /// <summary>
    /// Sliding Panels gesture recogniser.
    /// </summary>
    public class SlidingGestureRecogniser : UIPanGestureRecognizer
    {
        #region Data Members

        /// <summary>
        /// The list of panels that need to be monitored for gestures
        /// </summary>
        private readonly List<PanelContainer> _panelContainers;

        #endregion

        #region Properties

        /// <summary>
        /// The currently showing panel
        /// </summary>
        /// <value>The current active panel container.</value>
        protected PanelContainer CurrentActivePanelContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the sliding controller.
        /// </summary>
        /// <value>The sliding controller.</value>
        public UIViewController SlidingController
        {
            get;
            private set;
        }

        /// <summary>
        /// Occurs when a sliding panel should be shown
        /// </summary>
        public event EventHandler ShowPanel;

        /// <summary>
        /// Occurs when a sliding panel should be hidden
        /// </summary>
        public event EventHandler HidePanel;

        #endregion

        #region Construction / Destruction

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidingPanels.SlidingGestureRecogniser"/> class.
        /// </summary>
        /// <param name="panelContainers">List of Panel Containers to monitor for gestures</param>
        /// <param name="shouldReceiveTouch">Indicates that touch events should be monitored</param>
        /// <param name="slidingController">The Sliding Panels controller</param>
        /// <param name="contentView"></param>
        public SlidingGestureRecogniser(List<PanelContainer> panelContainers, UITouchEventArgs shouldReceiveTouch, UIViewController slidingController, UIView contentView)
        {
            SlidingController = slidingController;
            _panelContainers = panelContainers;

            ShouldReceiveTouch += (sender, touch) =>
            {
                if (SlidingController == null)
                {
                    return false;
                }

                if (touch.View is UIButton)
                {
                    return false;
                }

                var validTouch = false;
                var touchView = touch.View;
                while (touchView != null)
                {
                    if (Equals(touchView, contentView))
                    {
                        validTouch = true;
                        break;
                    }
                    touchView = touchView.Superview;
                }

                return validTouch && shouldReceiveTouch(sender, touch);
            };
        }

        #endregion

        #region Touch Methods

        /// <summary>
        /// We want to prevent any other gesture to be recognized on the window!
        /// </summary>
        /// <param name="preventingGestureRecognizer">Preventing gesture recognizer.</param>
        public override bool CanBePreventedByGestureRecognizer(UIGestureRecognizer preventingGestureRecognizer)
        {
            return State != UIGestureRecognizerState.Began;
        }

        /// <summary>
        /// Manages what happens when the user begins a possible slide 
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            CGPoint touchPt;
            var touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                touchPt = touch.LocationInView(View);
            }
            else
            {
                State = UIGestureRecognizerState.Failed;
                return;
            }

            CurrentActivePanelContainer = _panelContainers.FirstOrDefault(p => p.IsVisible);
            if (CurrentActivePanelContainer == null)
            {
                CurrentActivePanelContainer = _panelContainers.FirstOrDefault(p => p.CanStartSliding(touchPt, SlidingController.View.Frame));
                if (CurrentActivePanelContainer != null)
                {
                    CurrentActivePanelContainer.Show();
                    CurrentActivePanelContainer.SlidingStarted(touchPt, SlidingController.View.Frame);
                }
                else
                {
                    State = UIGestureRecognizerState.Failed;
                }
            }
            else
            {
                CurrentActivePanelContainer.SlidingStarted(touchPt, SlidingController.View.Frame);
            }
        }

        /// <summary>
        /// Manages what happens while the user is mid-slide
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (CurrentActivePanelContainer == null)
            {
                return;
            }

            CGPoint touchPt;
            var touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                touchPt = touch.LocationInView(View);
            }
            else
            {
                return;
            }

            var newFrame = CurrentActivePanelContainer.Sliding(touchPt, SlidingController.View.Frame);
            SlidingController.View.Frame = newFrame;
        }

        /// <summary>
        /// Manages what happens when the user completes a slide
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (CurrentActivePanelContainer == null)
            {
                return;
            }

            CGPoint touchPt;
            var touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                touchPt = touch.LocationInView(View);
            }
            else
            {
                return;
            }

            if (CurrentActivePanelContainer.SlidingEnded(touchPt, SlidingController.View.Frame))
            {
                if (ShowPanel != null)
                {
                    ShowPanel(this, new SlidingGestureEventArgs(CurrentActivePanelContainer));
                }
            }
            else
            {
                if (HidePanel != null)
                {
                    HidePanel(this, new SlidingGestureEventArgs(CurrentActivePanelContainer));
                }
            }
        }

        /// <summary>
        /// Manages what happens when a slide is interrupted
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
        }

        #endregion
    }
}