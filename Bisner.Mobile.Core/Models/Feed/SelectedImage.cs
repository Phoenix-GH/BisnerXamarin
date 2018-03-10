using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Feed
{
    public class SelectedImage : MvxViewModel
    {
        private byte[] _bytes;
        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; RaisePropertyChanged(() => Bytes); }
        }
    }
}