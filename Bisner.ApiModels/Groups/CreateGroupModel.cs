using System;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.ApiModels.Groups
{
    public class CreateGroupModel
    {
        /// <summary>
        /// Parent group id or guid.empty for main groups
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// /Group location Id
        /// </summary>
        public Guid? Locationid { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Group description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Group type
        /// </summary>
        public GroupType GroupType { get; set; }

        /// <summary>
        /// User id of the user creating the group
        /// </summary>
        public Guid UserId { get; set; }
    }
}