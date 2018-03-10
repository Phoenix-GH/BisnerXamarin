using Bisner.Mobile.iOS.Controls.SlidingPanels;

namespace Bisner.Mobile.iOS.MvvmcrossApp
{
    /// <summary>
    /// Main view presenter host to get a reference to the main view presenter
    /// </summary>
    public interface IMainViewPresenterHost
    {
        /// <summary>
        /// The main sliding panels navigation controller
        /// </summary>
        SlidingPanelsNavigationViewController NavController { get; }

        /// <summary>
        /// The main view presenter containing the tabs
        /// </summary>
        IMainViewPresenter MainViewPresenter { get; set; }


        //SlidingPanelsNavigationViewController SlidingPanelsController { get; }

        /// <summary>
        /// Indicates if the sliders can be opened
        /// </summary>
        bool EnableSliders { get; set; }
    }
}