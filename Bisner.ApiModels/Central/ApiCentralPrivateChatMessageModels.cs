using System;
using System.Collections.Generic;

namespace Bisner.ApiModels.Central
{
    public class ApiCentralPrivateChatMessageModel
    {
        /// <summary>
        /// Message id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Datetime of message
        /// </summary>
        public DateTime DateTime { get; set; }
        
        /// <summary>
        /// Text of message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Is read
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// From user id
        /// </summary>
        public Guid FromId { get; set; }

        /// <summary>
        /// To user id
        /// </summary>
        public Guid ToId { get; set; }
        
        /// <summary>
        /// Id for the sender to identify the message
        /// </summary>
        public string SenderMessageId { get; set; }
    }

    public class ApiCentralGroupChatModel
    {
        /// <summary>
        /// Group chat id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Optional group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All useres that have chatted in this group + lasttime they have read the chat messages
        /// </summary>
        public Dictionary<Guid, DateTime> UserIds { get; set; }
        
        /// <summary>
        /// Last message datetime
        /// </summary>
        public DateTime LastMessageDateTime { get; set; }

        /// <summary>
        /// Last message text
        /// </summary>
        public string LastMessageText { get; set; }

        /// <summary>
        /// First message in the collection
        /// </summary>
        public DateTime FirstMessageDateTime { get; set; }
    }
        

    public class ApiCentralGroupChatMessageModel
    {
        /// <summary>
        /// Message id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Datetime of message
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Text of message
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// From user id
        /// </summary>
        public Guid FromId { get; set; }
        
    }
}