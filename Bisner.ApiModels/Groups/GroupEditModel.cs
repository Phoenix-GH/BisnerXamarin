using System;

namespace Bisner.ApiModels.Groups
{
    /// <summary>
    /// Edit group model
    /// </summary>
    public class GroupEditModel
    {
        /// <summary>
        /// Group id
        /// </summary>
        public Guid Id { get; set; }

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
        /// Group introduction
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// Group tags
        /// </summary>
        public string Tags { get; set; }
    }
}