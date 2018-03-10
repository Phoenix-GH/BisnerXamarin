using Android.OS;
using Android.Views;
using Bisner.Mobile.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Bisner.Mobile.Droid.Views.Base
{
    public abstract class BaseToolbarActivity<TViewModel> : BaseActivity<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Constructor

        #endregion Constructor

        #region Create

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetupActionbar();
        }

        #endregion Create

        #region TitleBar

        private void SetupActionbar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            SetSupportActionBar(toolbar);

            // Always set title to null, set title in derived activity
            SupportActionBar.Title = null;

            // Check if we need the back button
            if (SupportBackButton)
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        protected bool SupportBackButton { get; set; }

        #endregion TitleBar

        #region OptionsMenu

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                if (SupportBackButton)
                {
                    if (!MainApplication.MainViewIsRunning)
                    {
                        // Show main viewmodel 
                        Mvx.Resolve<IMvxViewDispatcher>()
                            .ShowViewModel(new MvxViewModelRequest<MainViewModel>(null, null, null));
                    }

                    OnBackPressed();

                    return true;
                }
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion OptionsMenu
    }
}