using System;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Menu
{
    /// <summary>
    /// Left sliding menu view model
    /// </summary>
    public class AndroidMenuViewModel : MvxViewModel
    {
        #region Constructor

        #endregion Constructor

        #region Navigate

        public void ShowViewModelAndroid(Type viewModel)
        {
            ShowViewModel(viewModel);
        }

        #endregion Navigate
    }
}