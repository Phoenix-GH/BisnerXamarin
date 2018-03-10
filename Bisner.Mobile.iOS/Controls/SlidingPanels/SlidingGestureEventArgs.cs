using System;
using Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers;

namespace Bisner.Mobile.iOS.Controls.SlidingPanels
{
    /// <summary>
    /// Argument class for Sliding Gestures
    /// </summary>
    public class SlidingGestureEventArgs : EventArgs
    {
        /// <summary>
        /// The Panel Container being affected
        /// </summary>
        /// <value>The panel container.</value>
        public PanelContainer PanelContainer
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidingPanels.SlidingGestureEventArgs"/> class.
        /// </summary>
        /// <param name="panelContainer">Panel container.</param>
        public SlidingGestureEventArgs(PanelContainer panelContainer)
        {
            PanelContainer = panelContainer;
        }
    }
}