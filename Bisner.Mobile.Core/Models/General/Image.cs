using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.General
{
    public class Image : ItemBase, IImage
    {
        #region Constructor

        private MvxCommand _showCommand;

        #endregion Constructor

        #region Properties

        public string Small { get; set; }

        public string Medium { get; set; }

        public string Large { get; set; }

        public string OriginalFileName { get; set; }

        public string MimeType { get; set; }

        #endregion Properties

        #region Show

        public MvxCommand ShowCommand => _showCommand ?? (_showCommand = new MvxCommand(Show));

        private void Show()
        {
            ShowViewModel<ImageZoomViewModel>(new { url = Large });
        }

        #endregion Show
    }
}