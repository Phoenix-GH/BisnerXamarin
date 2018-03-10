using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.MvvmcrossApp
{
    /// <summary>
    /// Interface for tab bar controller to be able to show views in tabs and set badge numbers
    /// </summary>
    public interface IMainViewPresenter
    {
        /// <summary>
        /// Show a view within the main view presenter
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        bool Show(IMvxIosView view);

        /// <summary>
        /// Feed tab bar item to be able to set badge number
        /// </summary>
        UITabBarItem FeedTabBarItem { get; }

        /// <summary>
        /// Chat tab bar item to be able to set badge number
        /// </summary>
        UITabBarItem ChatTabBarItem { get; }

        /// <summary>
        /// Notification tab bar item to be able to set badge number
        /// </summary>
        UITabBarItem NotificationTabBarItem { get; }

        /// <summary>
        /// If true views will be pushed to the master navigation controller instead of being shown in the tabs
        /// </summary>
        bool ShowInTab { get; set; }

        bool Close(IMvxViewModel viewModel);

        /// <summary>
        /// Set the active tab
        /// </summary>
        void SetActiveTab(int index);
    }
}