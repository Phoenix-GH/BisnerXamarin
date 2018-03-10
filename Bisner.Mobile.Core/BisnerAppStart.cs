using Bisner.Constants;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core
{
    public class BisnerAppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            if (!string.IsNullOrWhiteSpace(Settings.RefreshToken))
            {
                if (App.AppPlatform == AppPlatform.iOS && Settings.CustomLogin)
                {
                    ShowViewModel<LauncherViewModel>();
                }
                else
                {
                    ShowViewModel<MainViewModel>();
                }
            }
            else
            {
                // User has not logged in before
                ShowViewModel<LoginViewModel>();
            }

            CheckNotifications();
        }

        /// <summary>
        /// Checks on the App instance if any of the id's from a push notification have been set
        /// </summary>
        /// <returns></returns>
        private void CheckNotifications()
        {
            if (App.ConversationId != null)
            {
                ShowViewModel<ChatConversationViewModel>(new { id = App.ConversationId.Value });

                App.ConversationId = null;
            }

            if (App.EventId != null)
            {
                ShowViewModel<EventViewModel>(new { id = App.EventId.Value });

                App.EventId = null;
            }

            if (App.PostId != null)
            {
                ShowViewModel<DetailsViewModel>(new { postId = App.PostId.Value });

                App.PostId = null;
            }

            if (App.GroupId != null)
            {
                ShowViewModel<FeedViewModel>(new { id = App.GroupId.Value, feedType = FeedType.Group });

                App.GroupId = null;
            }
        }
    }
}