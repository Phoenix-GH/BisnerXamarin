using System;
using Bisner.Mobile.Core.Extensions;

namespace Bisner.Mobile.Core.Models.Chat
{
    public class ChatLabel : IChatLabel
    {
        private string _text;

        /// <summary>
        /// The date of this label (set time to 00:00 before using this to get the correct sorting in the list)
        /// </summary>
        public DateTime DateTime { get; set; }

        private string _dateTimeText;
        public string DateTimeText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_dateTimeText))
                {
                    _dateTimeText = DateTime.ToChatTime();
                }

                return _dateTimeText;
            }
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Sender message id for message identification
        /// </summary>
        public string SenderMessageId { get; set; }

        /// <summary>
        /// If you set this to null it will show the date in a string format
        /// </summary>
        public string Text
        {
            get => _text ?? DateTime.ToLocalTime().ToDateString();
            set => _text = value;
        }
    }
}