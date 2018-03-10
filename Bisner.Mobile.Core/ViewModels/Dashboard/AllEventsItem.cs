using Bisner.Mobile.Core.Models.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class AllEventsItem : ItemBase
    {
        private string _text;
        private MvxCommand _showCommand;

        public string Text
        {
            get => _text;
            set { _text = value; RaisePropertyChanged(() => Text); }
        }

        public MvxCommand ShowCommand => _showCommand ?? (_showCommand = new MvxCommand(Show));

        private void Show()
        {
            ShowViewModel<EventsViewModel>();
        }
    }
}