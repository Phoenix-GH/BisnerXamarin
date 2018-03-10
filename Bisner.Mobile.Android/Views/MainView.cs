using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Text.Util;
using Android.Views;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.Core.ViewModels.Notifications;
using Bisner.Mobile.Droid.Controls;
using Bisner.Mobile.Droid.Views.AccessControl;
using Bisner.Mobile.Droid.Views.Base;
using Bisner.Mobile.Droid.Views.Chat;
using Bisner.Mobile.Droid.Views.Dashboard;
using Bisner.Mobile.Droid.Views.Feed;
using Bisner.Mobile.Droid.Views.Manage;
using Bisner.Mobile.Droid.Views.Notifications;
using Java.Lang;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Support.V4;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace Bisner.Mobile.Droid.Views
{
    [Activity(NoHistory = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainView : BaseToolbarActivity<MainViewModel>, FragmentManager.IOnBackStackChangedListener, TabLayout.IOnTabSelectedListener
    {
        #region Variables

        public ViewPager ViewPager { get; private set; }

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Activity

        /// <summary>
        /// Called when activity is created.
        /// </summary>
        /// <param name="bundle">The bundle containing activity parameters</param>
        protected override void OnCreate(Bundle bundle)
        {
            // Call before setcontentview, which is in the base class
            RequestWindowFeature(WindowFeatures.IndeterminateProgress);

            base.OnCreate(bundle);

            CheckIntent();

            //Listen for changes in the back stack
            SupportFragmentManager.AddOnBackStackChangedListener(this);

            //Handle when activity is recreated like on orientation Change
            ShouldDisplayHomeUp();

            SetupTabs();

            MainApplication.MainViewIsRunning = true;

            BetterLinkMovementMethod.Linkify(MatchOptions.All, this).SetOnLinkClickListener(new LinkCaptureClickListener());
        }

        private void CheckIntent()
        {
            //if(Intent.Action == Intent.ActionSend && Intent.Extras.ContainsKey(Android.Content.Intent.ExtraStream) )
        }

        protected override int LayoutId => Resource.Layout.main_view;
        protected override string ScreenName => "MainView";

        public override void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction)
        {
            transaction.SetCustomAnimations(
                // Your entrance animation xml reference
                Android.Resource.Animation.SlideInLeft,
                // Your exit animation xml reference
                Android.Resource.Animation.SlideOutRight);

            base.OnBeforeFragmentChanging(fragmentInfo, transaction);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            SupportActionBar.Title = null;
        }

        protected override void ShowFragment(string tag, int contentId, Bundle bundle, bool forceAddToBackStack = false,
            bool forceReplaceFragment = false)
        {
            // Force recreation for add post and chat conversations
            if (tag == typeof(AddPostViewModel).FullName || tag == typeof(ChatConversationViewModel).FullName)
            {
                base.ShowFragment(tag, contentId, bundle, forceAddToBackStack, true);
            }

            base.ShowFragment(tag, contentId, bundle, forceAddToBackStack, forceReplaceFragment);
        }

        public override bool OnSupportNavigateUp()
        {
            //This method is called when the up button is pressed. Just the pop back stack.
            SupportFragmentManager.PopBackStack();

            return base.OnSupportNavigateUp();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            MainApplication.MainViewIsRunning = false;
        }

        #endregion Activity

        #region BackStackChangedListener

        public void OnBackStackChanged()
        {
            ShouldDisplayHomeUp();
        }

        public void ShouldDisplayHomeUp()
        {
            //Enable Up button only  if there are entries in the back stack
            var canback = SupportFragmentManager.BackStackEntryCount > 0;
            SupportActionBar?.SetDisplayHomeAsUpEnabled(canback);
        }

        #endregion BackStackChangedListener

        #region Tabs & fragments

        private List<MvxCachingFragmentStatePagerAdapter.FragmentInfo> _fragments;

        private void SetupTabs()
        {
            _fragments = new List<MvxCachingFragmentStatePagerAdapter.FragmentInfo>();

            var userRoles = Settings.UserRoles;

            if (userRoles.Any(r => r == Home.View.ToLower()))
            {
                _fragments.Add(new MvxCachingFragmentStatePagerAdapter.FragmentInfo("Feed", typeof(FeedFragment), typeof(AndroidMainFeedViewModel)));
            }

            // If access control is enabled add tab
            if (Settings.AccessControlEnabled)
            {
                _fragments.Add(new MvxCachingFragmentStatePagerAdapter.FragmentInfo("Access Control", typeof(AccessControlFragment), typeof(AccessControlViewModel)));
            }

            _fragments.Add(new MvxCachingFragmentStatePagerAdapter.FragmentInfo("Dashboard", typeof(DashboardFragment), typeof(DashboardViewModel)));

            if (userRoles.Any(r => r == ApiModels.Security.Roles.Chat.View.ToLower()))
            {
                _fragments.Add(new MvxCachingFragmentStatePagerAdapter.FragmentInfo("Chat", typeof(ChatFragment), typeof(ChatViewModel)));
            }

            // If access control is enabled move notifications to the toolbar
            if (!Settings.AccessControlEnabled)
            {
                _fragments.Add(new MvxCachingFragmentStatePagerAdapter.FragmentInfo("Notifications",
                    typeof(NotificationsFragment), typeof(NotificationsViewModel)));
            }
            _fragments.Add(new MvxCachingFragmentStatePagerAdapter.FragmentInfo("More", typeof(ManageFragment), typeof(AndroidManageViewModel)));

            // Set the adapter on the viewpager
            ViewPager = FindViewById<ViewPager>(Resource.Id.main_view_pager);
            // Set this to number of fragments to prefent the tabs from being unloaded from memory
            ViewPager.OffscreenPageLimit = 5;
            var fragmentPagerAdapter = new MainFragmentAdapter(this, SupportFragmentManager, _fragments);
            ViewPager.Adapter = fragmentPagerAdapter;

            // Set pager on tablayout
            var tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetBackgroundColor(new Color(ContextCompat.GetColor(this, Resource.Color.tabbarcolor)));
            tabLayout.SetSelectedTabIndicatorColor(ContextCompat.GetColor(this, Resource.Color.selectedtabbarcolor));
            tabLayout.SetupWithViewPager(ViewPager);

            // Set icons
            var tabcount = 0;
            if (userRoles.Any(r => r == Home.View.ToLower()))
            {
                tabLayout.GetTabAt(tabcount).SetIcon(Resource.Drawable.feed_normal);
                tabcount++;
            }

            // Access control enabled so we have an extra tab
            if (Settings.AccessControlEnabled)
            {
                tabLayout.GetTabAt(tabcount).SetIcon(Resource.Drawable.lock_normal);
                tabcount++;
            }

            tabLayout.GetTabAt(tabcount).SetIcon(Resource.Drawable.dashboard_normal);
            tabcount++;

            if (userRoles.Any(r => r == ApiModels.Security.Roles.Chat.View.ToLower()))
            {
                tabLayout.GetTabAt(tabcount).SetIcon(Resource.Drawable.chat_normal);
                tabcount++;
            }

            // Access control enabled we move notifications to the toolbar
            if (!Settings.AccessControlEnabled)
            {
                tabLayout.GetTabAt(tabcount).SetIcon(Resource.Drawable.notifications_normal);
                tabcount++;
            }

            tabLayout.GetTabAt(tabcount).SetIcon(Resource.Drawable.more_normal);

            tabLayout.AddOnTabSelectedListener(this);
        }

        private class MainFragmentAdapter : MvxCachingFragmentStatePagerAdapter
        {
            #region Constructor

            public MainFragmentAdapter(Context context, FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments) : base(context, fragmentManager, fragments)
            {
            }

            #endregion Constructor

            #region Adapter

            public override ICharSequence GetPageTitleFormatted(int position)
            {
                return null;
            }

            #endregion Adapter
        }

        #endregion Tabs & fragments

        #region Tablayout

        public void OnTabReselected(TabLayout.Tab tab)
        {

        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            // Set current tab item because when setting IOntabSelectedListener on the tablayout the tabs won't switch when clicking on the tab icons
            // see http://stackoverflow.com/questions/32305734/android-in-tablayout-viewpager-fragments-not-updating-consistantly
            ViewPager.SetCurrentItem(tab.Position, true);
            int tabIconColor = ContextCompat.GetColor(this, Resource.Color.selectedtabbarcolor);
            tab.Icon.SetColorFilter(new Color(tabIconColor), PorterDuff.Mode.SrcIn);

        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
            int tabIconColor = ContextCompat.GetColor(this, Resource.Color.unselectedtabbarcolor);
            tab.Icon.SetColorFilter(new Color(tabIconColor), PorterDuff.Mode.SrcIn);
        }

        #endregion TabLayout
    }
}