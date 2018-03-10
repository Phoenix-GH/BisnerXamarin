using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Notifications;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Droid.Shared.Attributes;

namespace Bisner.Mobile.Droid.Views.Notifications
{
    [Register("bisner.mobile.droid.views.notifications.NotificationsFragment")]
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    public class NotificationsFragment : BaseFragment<NotificationsViewModel>
    {
        #region Constructor

        #endregion Constructor

        #region Fragment

        protected override int FragmentId { get { return Resource.Layout.notifications_fragment; } }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            SetupRecyclerView(view);

            return view;
        }

        protected override string ScreenName => "NotificationsView";

        #endregion Fragment

        #region Setup

        private void SetupRecyclerView(View view)
        {
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.notifications_recycler_view);

            if (recyclerView != null)
            {
                // Notification items have a fixed size
                recyclerView.HasFixedSize = true;
            }
        }

        #endregion Setup
    }
}