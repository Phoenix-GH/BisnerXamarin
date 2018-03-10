using System;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers
{
    public class BottomPanelContainer : PanelContainer
    {
        #region Data Members

        /// <summary>
        /// starting Y Coordinate of the top view
        /// </summary>
        private nfloat _topViewStartYPosition;

        /// <summary>
        /// Y coordinate where the user touched when starting a slide operation
        /// </summary>
        private nfloat _touchPositionStartYPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the panel position.
        /// </summary>
        /// <value>The panel position.</value>
        public CGRect PanelPosition
        {
            get
            {
                return new CGRect
                {
                    X = 0,
                    Y = View.Frame.Height - View.Frame.Y - Size.Height,
                    Height = Size.Height,
                    Width = View.Bounds.Width
                };
            }
        }

        #endregion

        #region Construction / Destruction

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidingPanels.PanelContainers.BottomPanelContainer"/> class.
        /// </summary>
        /// <param name="panel">Panel.</param>
        public BottomPanelContainer(UIViewController panel)
            : base(panel, PanelType.BottomPanel)
        {
        }

        #endregion

        #region ViewController

        /// <summary>
        /// Called after the Panel is loaded for the first time
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            PanelVc.View.Frame = PanelPosition;
        }

        /// <summary>
        /// Called whenever the Panel is about to become visible
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            PanelVc.View.Frame = PanelPosition;
        }

        #endregion ViewController

        #region Position Methods

        /// <summary>
        /// Returns a rectangle representing the location and size of the top view 
        /// when this Panel is showing
        /// </summary>
        /// <returns>The top view position when slider is visible.</returns>
        /// <param name="topViewCurrentFrame">Top view current frame.</param>
        public override CGRect GetTopViewPositionWhenSliderIsVisible(CGRect topViewCurrentFrame)
        {
            topViewCurrentFrame.Y = View.Frame.Height - topViewCurrentFrame.Height - Size.Height;
            return topViewCurrentFrame;
        }

        /// <summary>
        /// Returns a rectangle representing the location and size of the top view 
        /// when this Panel is hidden
        /// </summary>
        /// <returns>The top view position when slider is visible.</returns>
        /// <param name="topViewCurrentFrame">Top view current frame.</param>
        public override CGRect GetTopViewPositionWhenSliderIsHidden(CGRect topViewCurrentFrame)
        {
            topViewCurrentFrame.Y = 0;
            return topViewCurrentFrame;
        }

        #endregion

        #region Sliding Methods

        /// <summary>
        /// Determines whether this instance can start sliding given the touch position and the 
        /// current location/size of the top view. 
        /// Note that touchPosition is in Screen coordinate.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="touchPosition">Touch position.</param>
        /// <param name="topViewCurrentFrame">Top view's current frame.</param>
        public override bool CanStartSliding(CGPoint touchPosition, CGRect topViewCurrentFrame)
        {
            // touchPosition is in Screen coordinate.

            nfloat offset = 0;
            touchPosition.Y += offset;

            if (!IsVisible)
            {
                return (touchPosition.Y >= (View.Bounds.Height - EdgeTolerance) && touchPosition.Y <= View.Bounds.Height);
            }

            return topViewCurrentFrame.Contains(touchPosition);
        }

        /// <summary>
        /// Called when sliding has started on this Panel
        /// </summary>
        /// <param name="touchPosition">Touch position.</param>
        /// <param name="topViewCurrentFrame">Top view current frame.</param>
        public override void SlidingStarted(CGPoint touchPosition, CGRect topViewCurrentFrame)
        {
            _touchPositionStartYPosition = touchPosition.Y;
            _topViewStartYPosition = topViewCurrentFrame.Y;
        }

        /// <summary>
        /// Called while the user is sliding this Panel
        /// </summary>
        /// <param name="touchPosition">Touch position.</param>
        /// <param name="topViewCurrentFrame">Top view current frame.</param>
        public override CGRect Sliding(CGPoint touchPosition, CGRect topViewCurrentFrame)
        {
            var translation = touchPosition.Y - _touchPositionStartYPosition;

            var frame = topViewCurrentFrame;
            frame.Y = _topViewStartYPosition + translation;

            if (frame.Y >= 0)
            {
                frame.Y = 0;
            }
            else if (frame.Y <= (View.Bounds.Height - topViewCurrentFrame.Height - Size.Height))
            {
                frame.Y = View.Bounds.Height - topViewCurrentFrame.Height - Size.Height;
            }
            return frame;
        }

        /// <summary>
        /// Determines if a slide is complete
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="touchPosition">Touch position.</param>
        /// <param name="topViewCurrentFrame">Top view current frame.</param>
        public override bool SlidingEnded(CGPoint touchPosition, CGRect topViewCurrentFrame)
        {
            // touchPosition will be in View coordinate, and will be adjusted to account
            // for the nav bar if visible.

            var screenHeight = topViewCurrentFrame.Height;
            var panelHeight = Size.Height;

            var y = topViewCurrentFrame.Y + topViewCurrentFrame.Height;
            return (y < (screenHeight - (panelHeight / 2)));
        }

        #endregion
    }
}