using System;
using System.Windows.Input;
using Bisner.Mobile.Core.Models.Booking;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Booking
{
    public class BodySliderItemViewModel : MvxViewModel
    {
        #region Constructor

        public BodySliderItemViewModel(BodySliderData data, Action selectAction = null)
        {
            ImageUrl = data.ImageUrl;

            SelectCommand = new MvxCommand(() =>
            {
                selectAction?.Invoke();
            });
        }

        #endregion Constructor

        #region property

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
