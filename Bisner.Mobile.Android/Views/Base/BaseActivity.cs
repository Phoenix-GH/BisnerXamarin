using Android.Content;
using Android.OS;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Calligraphy;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;

namespace Bisner.Mobile.Droid.Views.Base
{
    public abstract class BaseActivity<TViewModel> : MvxCachingFragmentCompatActivity<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Constructor

        #endregion Constructor

        #region Activity

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(LayoutId);

            SendScreen();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            TryAttachViewModel();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            TryUnattachViewModel();
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        #endregion Activity

        #region Properties

        protected abstract int LayoutId { get; }

        #endregion Properties

        #region Events

        private void TryAttachViewModel()
        {
            var eventViewModel = ViewModel as IEventViewModel;

            eventViewModel?.Attach();
        }

        private void TryUnattachViewModel()
        {
            var eventViewModel = ViewModel as IEventViewModel;

            eventViewModel?.Unattach();
        }

        #endregion Events

        #region Analytics

        protected abstract string ScreenName { get; }

        private void SendScreen()
        {
            Mvx.Resolve<IAnalyticsService>().SendScreen(ScreenName);
        }

        #endregion Analytics
    }
}