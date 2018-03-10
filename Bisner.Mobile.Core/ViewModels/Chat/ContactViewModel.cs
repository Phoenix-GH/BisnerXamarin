using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Chat
{
    public class ContactViewModel : MvxViewModel
    {
        #region Constructor

        private string _displayName;

        #endregion Constructor

        #region Properties

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        #endregion Properties
    }
}