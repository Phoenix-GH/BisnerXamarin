using System.Threading.Tasks;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels
{
    public class LauncherViewModel : BaseViewModel
    {
        #region Constructor

        public LauncherViewModel(IPlatformService platformService) : base(platformService)
        {
        }
        
        #endregion Constructor

        #region Init

        public async Task Init()
        {
            await Task.Delay(2000).ContinueWith(task =>
            {
                ShowViewModel<MainViewModel>();
            });
        }

        #endregion Init
    }
}
