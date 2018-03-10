using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.Chat
{
    public interface IChatMessage : IChatItem, IDisplayUser
    {
        Guid ConversationId { get; set; }
        bool IsRead { get; set; }
        bool SendFail { get; set; }
        void Update(IChatMessage message);
    }
}