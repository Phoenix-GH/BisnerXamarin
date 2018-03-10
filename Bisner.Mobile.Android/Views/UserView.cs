using Android.App;
using Android.OS;
using Android.Widget;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Droid.Controls.PicassoTransformations;
using Bisner.Mobile.Droid.Views.Base;
using Square.Picasso;

namespace Bisner.Mobile.Droid.Views
{
    [Activity(NoHistory = false)]
    public class UserView : BaseActivity<UserViewModel>
    {
        #region Variables

        #endregion Varaiables

        #region Constructor

        #endregion Constructor

        #region Activity

        protected override int LayoutId => Resource.Layout.user_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetupHeader();
        }

        protected override string ScreenName => "UserView";

        private void SetupHeader()
        {
            //var headerImageView = FindViewById<ImageView>(Resource.Id.user_view_header);

            //if (headerImageView != null)
            //{

            //}
        }

        #endregion Activity
    }
}