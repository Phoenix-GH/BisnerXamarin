using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.Chat
{
    public interface IChatItem : IItemBase
    {
        DateTime DateTime { get; set; }

        string DateTimeText { get; }

        string Text { get; set; }

        string SenderMessageId { get; set; }
    }
}