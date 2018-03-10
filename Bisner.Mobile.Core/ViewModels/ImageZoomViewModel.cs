using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;

namespace Bisner.Mobile.Core.ViewModels
{
    public class ImageZoomViewModel : BaseViewModel
    {
        #region Constructor

        private string _imageUrl;

        public ImageZoomViewModel(IPlatformService platformService) : base(platformService)
        {
        }

        #endregion Constructor

        #region Init

        public void Init(string url)
        {
            ImageUrl = url;
        }

        #endregion Init

        #region Image

        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        #endregion Image
    }
}
