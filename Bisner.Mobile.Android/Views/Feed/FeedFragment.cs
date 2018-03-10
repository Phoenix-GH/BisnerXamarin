using System.Linq;
using Android.OS;
using Android.Runtime;
using Android.Text.Util;
using Android.Util;
using Android.Views;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Droid.Controls;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Debug = System.Diagnostics.Debug;

namespace Bisner.Mobile.Droid.Views.Feed
{
    [Register("bisner.mobile.droid.views.feed.FeedFragment")]
    public class FeedFragment : BaseToolbarFragment<AndroidMainFeedViewModel>
    {
        #region Constructor

        #endregion Constructor

        #region Fragment

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            HasOptionsMenu = true;
        }

        protected override int FragmentId => ViewModel.FeedType != FeedType.Home ? Resource.Layout.feed_fragment_with_toolbar : Resource.Layout.feed_fragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            if (ViewModel.FeedType != FeedType.Home)
                ParentActivity.SupportActionBar.Title = ViewModel.Title;

            SetupRecyclerView(view);

            return view;
        }

        protected override string ScreenName => "FeedView id = " + ViewModel.FeedId;

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            // Create role
            if (Settings.UserRoles.Any(r => r == Home.Feed.Create.ToLower()))
            {
                inflater.Inflate(Resource.Menu.feed_toolbar, menu);

                // Add post icon
                TintMenuItem(menu.GetItem(0), Resource.Color.bisnerblue);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.create_feed_post)
            {
                ViewModel.CreateCommand.Execute(null);

                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion Fragment

        #region Setup

        private void SetupRecyclerView(View view)
        {
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.feed_recycler_view);

            if (recyclerView != null)
            {
                var displayMetrics = new DisplayMetrics();

                Activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

                recyclerView.Adapter = new FeedAdapter(displayMetrics, (IMvxAndroidBindingContext)BindingContext);
            }
        }

        #endregion Setup
    }
}