using System;
using Android.App;
using Android.OS;
using Android.Views;
using Bisner.Mobile.Core.ViewModels.Manage.User;
using Bisner.Mobile.Droid.Views.Base;

namespace Bisner.Mobile.Droid.Views.Manage
{
    [Activity(NoHistory = false)]
    public class SecurityView : BaseToolbarActivity<SecurityViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        public SecurityView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override int LayoutId => Resource.Layout.security_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ViewModel.AfterChangedAction = AfterChangedAction;
        }

        private void AfterChangedAction()
        {
            Finish();
        }

        protected override string ScreenName => "SecurityView";

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.security_toolbar, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.update_password_btn)
            {
                ViewModel.UpdateCommand.Execute(null);

                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion Activity
    }
}