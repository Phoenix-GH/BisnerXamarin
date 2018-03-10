using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Bisner.Mobile.Droid.Views.Base
{
    public abstract class BaseToolbarFragment<TViewModel> : BaseFragment<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Properties

        public MvxCachingFragmentCompatActivity ParentActivity => (MvxCachingFragmentCompatActivity)Activity;

        #endregion Properties

        #region Constructor

        private Toolbar _toolbar;
        //private MvxActionBarDrawerToggle _drawerToggle;

        #endregion Constructor

        #region Fragment

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            TrySetToolbar(view);

            return view;
        }

        //public override void OnConfigurationChanged(Configuration newConfig)
        //{
        //    base.OnConfigurationChanged(newConfig);

        //    if (_toolbar != null)
        //        _drawerToggle.OnConfigurationChanged(newConfig);
        //}

        //public override void OnActivityCreated(Bundle savedInstanceState)
        //{
        //    base.OnActivityCreated(savedInstanceState);

        //    if (_toolbar != null)
        //        _drawerToggle.SyncState();
        //}

        #endregion Fragment

        #region Toolbar

        private void TrySetToolbar(View view)
        {
            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);

            if (_toolbar != null)
            {
                ParentActivity.SetSupportActionBar(_toolbar);
                //ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                ParentActivity.SupportActionBar.SetDisplayShowTitleEnabled(false);

                //_drawerToggle = new MvxActionBarDrawerToggle(Activity, (ParentActivity as INavigationActivity).DrawerLayout, _toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
                //_drawerToggle.DrawerOpened += (object sender, ActionBarDrawerEventArgs e) => ((MainActivity)Activity).HideSoftKeyboard();
                //(ParentActivity as INavigationActivity).DrawerLayout.AddDrawerListener(_drawerToggle);
            }
        }

        protected void TintMenuItem(IMenuItem menuItem, int colorId)
        {
            var drawable = menuItem.Icon;

            var color = new Color(ContextCompat.GetColor(Activity, colorId));

            drawable.SetColorFilter(color, PorterDuff.Mode.SrcAtop);
        }

        #endregion Toolbar
    }
}