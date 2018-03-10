using System;

namespace Bisner.ApiModels.Whitelabel
{

    public class ApiWhitelabelNotificationModel
    {
        /// <summary>
        /// Notification id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Related to item id
        /// </summary>
        public Guid RelatedItemId { get; set; }

        /// <summary>
        /// Json data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Notification type
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Created on this datetime
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Read on this datetime
        /// </summary>
        public DateTime? IsReadOnDateTime { get; set; }

        /// <summary>
        /// Seen before yes /no
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Notification triggered by user
        /// </summary>
        public ApiWhitelabelPublicUserModel ByUser { get; set; }

        /// <summary>
        /// User id that recieves this notification
        /// </summary>
        public Guid UserId { get; set; }
    }
}