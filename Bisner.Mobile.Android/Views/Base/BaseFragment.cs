using Android.OS;
using Android.Views;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;

namespace Bisner.Mobile.Droid.Views.Base
{
    public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Properties

        protected abstract int FragmentId { get; }

        #endregion Properties

        #region Fragment

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(FragmentId, null);

            SendScreen();

            TryAttachViewModel();

            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            TryUnattachViewModel();
        }

        #endregion Fragment

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