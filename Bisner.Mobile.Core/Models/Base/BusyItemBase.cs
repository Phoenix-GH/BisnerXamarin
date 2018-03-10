namespace Bisner.Mobile.Core.Models.Base
{
    public class BusyItemBase : ItemBase, IBusyItemBase
    {
        private bool _isBusy;
        private bool _isNotBusy;

        public bool IsBusy
        {
            get => _isBusy;
            protected set
            {
                _isBusy = value; RaisePropertyChanged(() => IsBusy);
                IsNotBusy = !value;
            }
        }

        public bool IsNotBusy
        {
            get => _isNotBusy;
            private set { _isNotBusy = value; RaisePropertyChanged(() => IsNotBusy); }
        }
    }
}