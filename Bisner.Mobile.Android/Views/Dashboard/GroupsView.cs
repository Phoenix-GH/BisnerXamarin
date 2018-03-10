using Android.App;
using Android.OS;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Activity(NoHistory = false)]
    public class GroupsView : BaseToolbarActivity<GroupsViewModel>
    {
        #region Constructor

        public GroupsView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override int LayoutId => Resource.Layout.groups_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetupRecyclerView();
            
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        protected override string ScreenName => "GroupsFragment";

        #endregion Activity

        #region Recycler view

        public void SetupRecyclerView()
        {
            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.groups_recycler_view);

            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;

                recyclerView.ItemTemplateSelector = new GroupsTemplateSelector(ViewModel);
            }
        }

        #endregion Recycler view
    }
}