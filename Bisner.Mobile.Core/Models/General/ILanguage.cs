using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.General
{
    public interface ILanguage : IItemBase
    {
        /// <summary>
        /// Localized name
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the language code
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        bool IsDefault { get; set; }
    }

    public class Language : ILanguage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDefault { get; set; }
    }
}
