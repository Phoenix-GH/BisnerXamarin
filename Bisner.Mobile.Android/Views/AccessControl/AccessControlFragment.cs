using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using Bisner.Mobile.Droid.Views.Base;
using Bisner.Mobile.Droid.Views.Feed;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.AccessControl
{
    [Register("bisner.mobile.droid.views.feed.AccessControlFragment")]
    public class AccessControlFragment : BaseToolbarFragment<AccessControlViewModel>
    {
        #region Fragment

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        protected override int FragmentId => Resource.Layout.accesscontrol_fragment;

        protected override string ScreenName => "AccessControl";

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            SetupRecyclerView(view);

            return view;
        }

        #endregion Fragment

        #region Setup

        private void SetupRecyclerView(View view)
        {
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.accesscontrol_recycler_view);

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