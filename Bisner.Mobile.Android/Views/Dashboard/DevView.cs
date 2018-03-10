using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Droid.Views.Base;
using Java.Net;
using MvvmCross.Platform;
using Debug = System.Diagnostics.Debug;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Activity(NoHistory = false)]
    public class DevView : BaseToolbarActivity<DevViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        public DevView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Fragment

        protected override int LayoutId => Resource.Layout.dev_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RetrieveIp();

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        protected override string ScreenName => "DevView";

        #endregion Fragment

        #region IP

        private void RetrieveIp()
        {
            Task.Run(() =>
            {
                try
                {
                    var configuration = Mvx.Resolve<IConfiguration>();

                    var baseUrl = configuration.BaseUrl.Replace("https://", "").Replace("/", "");

                    var address = InetAddress.GetByName(baseUrl);

                    ViewModel.ServerIp = address.CanonicalHostName;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        #endregion IP
    }
}