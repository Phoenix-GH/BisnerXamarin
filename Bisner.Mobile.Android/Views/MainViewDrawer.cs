//using System.Collections.Generic;
//using Android.App;
//using Android.OS;
//using Android.Support.V4.View;
//using Android.Support.V4.Widget;
//using Android.Support.V7.App;
//using Bisner.Mobile.Android.Views.Base;
//using Bisner.Mobile.Android.Views.Menu;
//using Bisner.Mobile.Core.ViewModels;
//using Bisner.Mobile.Core.ViewModels.Menu;
//using MvvmCross.Core.ViewModels;
//using MvvmCross.Droid.Shared.Caching;
//using MvvmCross.Platform;
//using Fragment = Android.Support.V4.App.Fragment;
//using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
//using IMenuItem = Android.Views.IMenuItem;

//namespace Bisner.Mobile.Android.Views
//{
//    [Activity(NoHistory = false)]
//    //[IntentFilter("")]
//    public class MainViewDrawer : BaseToolbarActivity<AndroidHomeViewModel>, INavigationActivity
//    {
//        #region Constructor

//        private ActionBarDrawerToggle _drawerToggle;

//        #endregion Constructor

//        #region Activity

//        /// <summary>
//        /// Called when activity is created.
//        /// </summary>
//        /// <param name="bundle">The bundle containing activity parameters</param>
//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);

//            SetContentView(Resource.Layout.main_view_drawer);

//            // Setup everything
//            SetupMenu();
//            SetupDrawer();
//        }

//        public override void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction)
//        {
//            transaction.SetCustomAnimations(
//                // Your entrance animation xml reference
//                Resource.Animation.abc_slide_in_bottom,
//                // Your exit animation xml reference
//                Resource.Animation.abc_slide_out_top);

//            base.OnBeforeFragmentChanging(fragmentInfo, transaction);
//        }

//        protected override void OnPostCreate(Bundle savedInstanceState)
//        {
//            base.OnPostCreate(savedInstanceState);

//            SupportActionBar.Title = null;
//        }

//        #endregion Activity

//        #region FragmentCache

//        protected override IEnumerable<Fragment> GetCurrentCacheableFragments()
//        {
//            var fragments = base.GetCurrentCacheableFragments();

//            return fragments;
//        }

//        protected override IMvxCachedFragmentInfo GetFragmentInfoByTag(string tag)
//        {
//            var fragmentInfo = base.GetFragmentInfoByTag(tag);

//            return fragmentInfo;
//        }

//        #endregion FragmentCache

//        #region Setup

//        private void SetupMenu()
//        {
//            var menuFragment = SupportFragmentManager.FindFragmentById(Resource.Id.menu_fragment) as MenuFragment;

//            if (menuFragment != null)
//            {
//                var menuViewModel =
//                    Mvx.Resolve<IMvxViewModelLoader>()
//                        .LoadViewModel(new MvxViewModelRequest(typeof(AndroidMenuViewModel), null, null, null), null) as AndroidMenuViewModel;

//                if (menuViewModel != null)
//                    menuFragment.ViewModel = menuViewModel;
//            }
//        }

//        private void SetupDrawer()
//        {
//            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.main_drawer_layout);
//            _drawerToggle = new ActionBarDrawerToggle(this, DrawerLayout, Resource.String.drawer_open,
//                Resource.String.drawer_close);
//            DrawerLayout.AddDrawerListener(_drawerToggle);
//        }

//        #endregion Setup

//        #region Toolbar

//        public override bool OnOptionsItemSelected(IMenuItem item)
//        {
//            if (_drawerToggle.OnOptionsItemSelected(item))
//            {
//                return true;
//            }

//            return base.OnOptionsItemSelected(item);
//        }

//        #endregion Toolbar

//        #region Drawer

//        public DrawerLayout DrawerLayout { get; private set; }

//        public override void OnBackPressed()
//        {
//            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
//                DrawerLayout.CloseDrawers();
//            else
//                base.OnBackPressed();
//        }

//        #endregion Drawer
//    }
//}