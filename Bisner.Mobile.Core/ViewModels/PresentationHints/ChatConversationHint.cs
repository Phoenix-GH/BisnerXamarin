using System;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.PresentationHints
{
    public class ChatConversationHint : MvxPresentationHint
    {
        public Guid SelectedUser { get; set; }
    }
}
