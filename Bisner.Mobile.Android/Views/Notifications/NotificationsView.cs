using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Notifications;
using Bisner.Mobile.Droid.Extensions;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform;
using MvvmCross.Plugins.DownloadCache;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Droid.Views.Notifications
{
    [Activity(NoHistory = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class NotificationsView : BaseToolbarActivity<AndroidNotificationsViewModel>
    {
        #region Variables

        private MvxRecyclerView _recyclerView;
        private MvxSubscriptionToken _newMessageToken;

        #endregion Variables

        #region Constructor

        public NotificationsView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Fragment

        protected override int LayoutId => Resource.Layout.notifications_view;

        protected override string ScreenName => "NotificationsView";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Intent.PrintExtras();

            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = ViewModel.Title;

            SetupRecyclerView();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Dispose and remove references for GC
            _recyclerView = null;
        }

        #endregion Fragment

        #region Setup

        private void SetupRecyclerView()
        {
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.notifications_recycler_view);

            if (recyclerView != null)
            {
                // Notification items have a fixed size
                recyclerView.HasFixedSize = true;
            }
        }

        #endregion Setup
    }
}