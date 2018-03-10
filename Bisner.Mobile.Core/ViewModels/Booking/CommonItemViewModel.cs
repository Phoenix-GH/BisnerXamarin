using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class CommonItemViewModel : MvxViewModel
    {
        public CommonItemViewModel(ICommand selectCommand)
        {
            SelectCommand = selectCommand;
        }

        #region property

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        public ICommand SelectCommand { get; }

        #endregion
    }
}
