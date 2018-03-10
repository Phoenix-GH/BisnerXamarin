using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels
{
    public class WebBrowserViewModel : BaseViewModel
    {
        #region Constructor

        public WebBrowserViewModel(IPlatformService platformService) : base(platformService)
        {
            SwapCommand = new MvxCommand(SwapUrl);
        }

        #endregion Constructor

        #region Init

        private string _reloadUrlNexudus = null;

        public void Init(string url)
        {
            Url = url;
        }

        #endregion Init

        #region Properties

        private string _url;
        public string Url
        {
            get => _url;
            set { _url = value; RaisePropertyChanged(() => Url); }
        }

        #endregion Properties

        public ICommand SwapCommand { get; }

        private bool _swap = false;

        public void SwapUrl()
        {
            Url = _swap ? "https://www.google.nl/" : "https://thebridge.spaces.nexudus.com/en/invoices/";
            _swap = !_swap;
        }
    }
}
