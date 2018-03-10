using System;
using Bisner.Mobile.Core.Models.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class HeaderImage : ItemBase
    {
        private string _image;
        private string _titleText;
        private MvxCommand _selectedCommand;

        public string Image
        {
            get => _image;
            set { _image = value; RaisePropertyChanged(() => Image); }
        }

        public string TitleText
        {
            get => _titleText;
            set { _titleText = value; RaisePropertyChanged(() => TitleText); }
        }

        public MvxCommand SelectedCommand => _selectedCommand ?? (_selectedCommand = new MvxCommand(OnSelected));

        public Action SelectedAction { get; set; }

        private void OnSelected()
        {
            SelectedAction?.Invoke();
        }
    }
}