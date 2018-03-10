using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Debug = System.Diagnostics.Debug;

namespace Bisner.Mobile.Droid.Views.Feed
{
    [Activity(NoHistory = false)]
    // FeeView is used for group and company feeds
    public class FeedView : BaseToolbarActivity<FeedViewModel>
    {
        #region Variables

        private MvxSubscriptionToken _groupJoinedToken;

        private IMenuItem _addMenuItem;

        #endregion Variables

        #region Constructor

        public FeedView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetupGroupJoinedMessage();

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            SupportActionBar.Title = null;
        }

        protected override int LayoutId => Resource.Layout.feed_view;

        protected override string ScreenName => "FeedView id = " + ViewModel.FeedId;

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            // Base class adds the toolbar here so we can only set the title after that is done
            base.OnPostCreate(savedInstanceState);

            SetupRecyclerView();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.feed_toolbar, menu);

            _addMenuItem = menu.GetItem(0);

            // Initial set
            ChangePostButton(ViewModel.CanCreate);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.create_feed_post)
            {
                ViewModel.CreateCommand.Execute(null);

                return true;
            }

            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_groupJoinedToken == null) return;

            _groupJoinedToken.Dispose();
            _groupJoinedToken = null;
        }

        #endregion Activity

        #region Setup

        private void SetupRecyclerView()
        {
            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.feed_recycler_view);

            if (recyclerView != null)
            {
                var displayMetrics = new DisplayMetrics();

                WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

                recyclerView.Adapter = new FeedAdapter(displayMetrics, (IMvxAndroidBindingContext)BindingContext);
            }
        }

        private void SetupGroupJoinedMessage()
        {
            _groupJoinedToken = Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<GroupJoinedMessage>(m =>
            {
                if (m.GroupId != ViewModel.FeedId) return;

                // Message is for this group
                ChangePostButton(m.HasJoined);
            });
        }

        private void ChangePostButton(bool enabled)
        {
            if (enabled)
            {
                _addMenuItem.SetEnabled(true);
                _addMenuItem.Icon.SetAlpha(255);
            }
            else
            {
                _addMenuItem.SetEnabled(false);
                _addMenuItem.Icon.SetAlpha(0);
            }
        }

        #endregion Setup
    }
}