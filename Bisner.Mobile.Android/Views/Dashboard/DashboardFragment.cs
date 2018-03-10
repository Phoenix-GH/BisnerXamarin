using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Register("bisner.mobile.droid.views.dashboard.DashboardFragment")]
    public class DashboardFragment : BaseFragment<DashboardViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Fragment

        protected override int FragmentId => Resource.Layout.dashboard_fragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            SetupRecyclerView(view);

            return view;
        }

        protected override string ScreenName => "DashboardView";

        #endregion Fragment

        #region Setup

        private void SetupRecyclerView(View view)
        {
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.dashboard_recycler_view);

            if (recyclerView != null)
            {
                var displayMetrics = new DisplayMetrics();

                Activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

                recyclerView.Adapter = new DashboardAdapter(displayMetrics, (IMvxAndroidBindingContext)BindingContext);
            }
        }

        private class DashboardAdapter : MvxRecyclerAdapter
        {
            #region Variables

            private readonly DisplayMetrics _displayMetrics;
            private readonly float _pixels;

            #endregion Variables

            #region Constructor

            public DashboardAdapter(DisplayMetrics displayMetrics, IMvxAndroidBindingContext bindingContext) : base(bindingContext)
            {
                _displayMetrics = displayMetrics;

                // Convert 1 dp to number of pixels for reference
                _pixels = TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, _displayMetrics);
            }

            #endregion Constructor

            #region Adapter

            protected override View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
            {
                var view = base.InflateViewForHolder(parent, viewType, bindingContext);

                //TrySetupButtonCell(view);

                return view;
            }

            #endregion Adapter

            #region Setup
            
            private void TrySetupButtonCell(View view)
            {
                // Card height
                var cardView = view.FindViewById<CardView>(Resource.Id.dashboard_item_button_layout);

                if (cardView != null)
                {
                    // Make it square
                    cardView.LayoutParameters.Height = (int)Math.Round(_displayMetrics.WidthPixels / 1.5f);

                    var memberButton = view.FindViewById<Button>(Resource.Id.dashboard_members_button);
                    memberButton.SetCompoundDrawablesWithIntrinsicBounds(null, ContextCompat.GetDrawable(Application.Context, Resource.Drawable.icon_members), null, null);
                    var eventsButton = view.FindViewById<Button>(Resource.Id.dashboard_events_button);
                    eventsButton.SetCompoundDrawablesWithIntrinsicBounds(null, ContextCompat.GetDrawable(Application.Context, Resource.Drawable.icon_calendar), null, null);
                    var groupsButton = view.FindViewById<Button>(Resource.Id.dashboard_groups_button);
                    groupsButton.SetCompoundDrawablesWithIntrinsicBounds(null, ContextCompat.GetDrawable(Application.Context, Resource.Drawable.icon_groups), null, null);
                    var infoButton = view.FindViewById<Button>(Resource.Id.dashboard_info_button);
                    infoButton.SetCompoundDrawablesWithIntrinsicBounds(null, ContextCompat.GetDrawable(Application.Context, Resource.Drawable.icon_info), null, null);
                }
            }

            #endregion Setup
        }

        #endregion Setup
    }
}