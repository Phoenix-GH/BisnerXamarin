using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.Core.ViewModels.Members;
using Bisner.Mobile.Core.ViewModels.Menu;
using Bisner.Mobile.Droid.Views.Base;
using static Android.Support.Design.Widget.NavigationView;
using IMenuItem = Android.Views.IMenuItem;

namespace Bisner.Mobile.Droid.Views.Menu
{
    [Register("bisner.mobile.droid.views.menu.MenuFragment")]
    public class MenuFragment : BaseFragment<AndroidMenuViewModel>, IOnNavigationItemSelectedListener
    {
        #region Constructor

        private NavigationView _navigationView;
        private IMenuItem _previousMenuItem;

        #endregion Constructor

        #region Fragment

        protected override int FragmentId => Resource.Layout.menu_fragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // BaseFragment instantiates the correct layout
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            SetupNavigationView(view);

            return view;
        }

        protected override string ScreenName => "MenuView";

        #endregion Fragment

        #region Setup

        private void SetupNavigationView(View view)
        {
            _navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);

            if (_navigationView != null)
            {
                _navigationView.SetNavigationItemSelectedListener(this);

                // Resources
                var navProfile = _navigationView.Menu.FindItem(Resource.Id.nav_profile);
                navProfile.SetTitle("Settings");
                var navDashboard = _navigationView.Menu.FindItem(Resource.Id.nav_dashboard);
                navDashboard.SetTitle("Dashboard");
                var navMembers = _navigationView.Menu.FindItem(Resource.Id.nav_members);
                navMembers.SetTitle("Members");
                var navEvents = _navigationView.Menu.FindItem(Resource.Id.nav_events);
                navEvents.SetTitle("Events");
                var navGroups = _navigationView.Menu.FindItem(Resource.Id.nav_groups);
                navGroups.SetTitle("Groups");
                var navMore = _navigationView.Menu.FindItem(Resource.Id.nav_more);
                navMore.SetTitle("More");
            }
        }

        #endregion Setup

        #region Navigation

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            // No action on same item pressed
            if (_previousMenuItem != null && item.ItemId == _previousMenuItem.ItemId)
                return false;

            _previousMenuItem = item;

            Navigate(item.ItemId);

            return true;
        }

        private void Navigate(int itemId)
        {
            ((INavigationActivity)Activity).DrawerLayout.CloseDrawers();

            switch (itemId)
            {
                case Resource.Id.nav_profile:
                    ViewModel.ShowViewModelAndroid(typeof(ManageViewModel));
                    break;
                case Resource.Id.nav_members:
                    ViewModel.ShowViewModelAndroid(typeof(MembersViewModel));
                    break;
                case Resource.Id.nav_events:
                    ViewModel.ShowViewModelAndroid(typeof(EventCategoriesViewModel));
                    break;
                case Resource.Id.nav_groups:
                    ViewModel.ShowViewModelAndroid(typeof(GroupsViewModel));
                    break;
                case Resource.Id.nav_more:
                    ViewModel.ShowViewModelAndroid(typeof(ManageViewModel));
                    break;
            }
        }

        #endregion Navigation
    }
}