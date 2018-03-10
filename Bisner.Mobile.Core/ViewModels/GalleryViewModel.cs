using System;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels
{
    public class GalleryViewModel : BaseViewModel, IMvxPageViewModel
    {
        #region Constructor

        public GalleryViewModel(IPlatformService platformService) : base(platformService)
        {

        }

        #endregion Constructor

        #region Init

        public void Init()
        { }

        #endregion Init

        #region Paging

        public IMvxPagedViewModel GetDefaultViewModel()
        {
            throw new NotImplementedException();
        }

        public IMvxPagedViewModel GetNextViewModel(IMvxPagedViewModel currentViewModel)
        {
            throw new NotImplementedException();
        }

        public IMvxPagedViewModel GetPreviousViewModel(IMvxPagedViewModel currentViewModel)
        {
            throw new NotImplementedException();
        }

        #endregion Paging
    }
}