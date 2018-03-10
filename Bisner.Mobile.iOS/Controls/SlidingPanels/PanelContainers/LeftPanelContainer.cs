using System;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers
{
    /// <summary>
    /// Container class for Sliding Panels located on the left edge of the device screen
    /// </summary>
    public class LeftPanelContainer : PanelContainer
    {
        #region Data Members

        /// <summary>
        /// starting X Coordinate of the top view
        /// </summary>
        private nfloat _topViewStartXPosition = 0.0f;

        /// <summary>
        /// X coordinate where the user touched when starting a slide operation
        /// </summary>
        private nfloat _touchPositionStartXPosition = 0.0f;

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
                    X = View.Frame.X,
                    Y = -View.Frame.Y,
                    Width = Size.Width,
                    Height = View.Bounds.Height
                };
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidingPanels.PanelContainers.LeftPanelContainer"/> class.
        /// </summary>
        /// <param name="panel">Panel.</param>
        public LeftPanelContainer(UIViewController panel)
            : base(panel, PanelType.LeftPanel)
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
            topViewCurrentFrame.X = Size.Width;
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
            topViewCurrentFrame.X = 0;
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
            if (!IsVisible)
            {
                return (touchPosition.X >= 0.0f && touchPosition.X <= EdgeTolerance);
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
            _touchPositionStartXPosition = touchPosition.X;
            _topViewStartXPosition = topViewCurrentFrame.X;
        }

        /// <summary>
        /// Called while the user is sliding this Panel
        /// </summary>
        /// <param name="touchPosition">Touch position.</param>
        /// <param name="topViewCurrentFrame">Top view current frame.</param>
        public override CGRect Sliding(CGPoint touchPosition, CGRect topViewCurrentFrame)
        {
            var panelWidth = Size.Width;
            var translation = touchPosition.X - _touchPositionStartXPosition;

            var frame = topViewCurrentFrame;

            frame.X = _topViewStartXPosition + translation;
            if (frame.X <= 0)
            {
                frame.X = 0;
            }

            if (frame.X >= panelWidth)
            {
                frame.X = panelWidth;
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
            return (topViewCurrentFrame.X > (Size.Width / 2));
        }

        #endregion
    }
}