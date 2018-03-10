using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.Mobile.Core.Communication;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.AccessControl
{
    public class AccessControlItemViewModel : MvxNotifyPropertyChanged
    {
        private LockState _state;
        private string _title;
        private string _subTitle;

        private AccessControlViewModel _parent;

        public AccessControlItemViewModel(AccessControlViewModel parent)
        {
            OpenCommand = new MvxAsyncCommand(Open);
            _parent = parent;
        }

        public Guid Id { get; set; }

        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        public string SubTitle
        {
            get => _subTitle;
            set { _subTitle = value; RaisePropertyChanged(() => SubTitle); }
        }

        public ICommand OpenCommand { get; }

        public LockState State
        {
            get => _state;
            set { _state = value; RaisePropertyChanged(() => State); }
        }

        private string _lastSubtitle;
        private async Task Open()
        {
            try
            {
                if (State == LockState.Close)
                {
                    State = LockState.Opening;
                    _lastSubtitle = SubTitle;
                    SubTitle = "Opening";

                    var response = await _parent.OpenAccessControlAsync(Id, ApiPriority.UserInitiated);

                    if (response.Data)
                    {
                        // Open
                        State = LockState.Open;
                        SubTitle = _lastSubtitle;

                        await Task.Delay(8000);

                        State = LockState.Close;
                    }
                    else
                    {
                        // Fail
                        State = LockState.Close;
                        SubTitle = response.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                State = LockState.Close;
                SubTitle = "Error trying to open";
            }
        }
    }

    public enum LockState
    {
        Close = 0,
        Opening = 1,
        Open = 2,
    }
}