using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Bisner.Mobile.Core.ViewModels.Members;
using Bisner.Mobile.Droid.Views.Base;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Activity(NoHistory = false)]
    public class MembersView : BaseToolbarActivity<MembersViewModel>
    {
        #region Constructor

        public MembersView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override int LayoutId => Resource.Layout.members_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            SetupRecyclerView();
        }

        protected override string ScreenName => "MembersView";

        #endregion Activity

        #region Setup

        private void SetupRecyclerView()
        {
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.members_recycler_view);

            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
            }
        }

        #endregion Setup
    }
}