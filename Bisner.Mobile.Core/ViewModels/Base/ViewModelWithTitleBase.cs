using Bisner.Mobile.Core.Service;

namespace Bisner.Mobile.Core.ViewModels.Base
{
    public abstract class ViewModelWithTitleBase : BaseViewModel
    {
        #region Constructor

        private string _title;

        protected ViewModelWithTitleBase(IPlatformService platformService) : base(platformService)
        {
        }

        #endregion Constructor

        #region View properties

        public string Title
        {
            get { return _title; }
            protected set { _title = value; RaisePropertyChanged(() => Title); }
        }

        #endregion View properties
    }
}