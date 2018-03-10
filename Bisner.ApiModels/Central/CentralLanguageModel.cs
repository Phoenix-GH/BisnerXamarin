using System;
using System.Collections.Generic;
using Bisner.ApiModels.General;

namespace Bisner.ApiModels.Central
{
    public class LanguageModel
    {
        /// <summary>
        /// Language id
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Localized name
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language code
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is published; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublished { get; set; }

        /// <summary>
        /// All translations for this language
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        public List<Translation> Translations { get; set; }
    }


    public class Translation
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Default bisner value
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Whitelabel custom value
        /// </summary>
        /// <value>
        /// The override value.
        /// </value>
        public string OverrideValue { get; set; }

        /// <summary>
        /// Gets or sets the main category.
        /// </summary>
        /// <value>
        /// The main category.
        /// </value>
        public string MainCategory { get; set; }

        /// <summary>
        /// Gets or sets the sub category.
        /// </summary>
        /// <value>
        /// The sub category.
        /// </value>
        public string SubCategory { get; set; }
    }


    public class CreateCentralLanguageModel
    {
        public string Code { get; set; }
        
        public string Name { get; set; }

        public bool IsDefaultLanguage { get; set; }

        public ApiImageModel Flag { get; set; }
    }

    public class CentralLanguageModel : CreateCentralLanguageModel
    {
        private List<TranslationResourceModel> _translations;

        public Guid Id { get; set; }

        public bool IsPublished { get; set; }

        public string UACode { get; set; }

        public List<TranslationResourceModel> Translations
        {
            get { return _translations ?? (_translations = new List<TranslationResourceModel>()); }
            set { _translations = value; }
        } 
    }

    public class ResourceCategoryModel
    {
        private List<ResourceSubcategoryModel> _subcategories;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ResourceSubcategoryModel> Subcategories
        {
            get { return _subcategories ?? (_subcategories = new List<ResourceSubcategoryModel>()); }
            set { _subcategories = value; }
        }
    }

    public class ResourceSubcategoryModel
    {
        private List<string> _keys;

        public string Name { get; set; }

        public List<string> Keys
        {
            get { return _keys ?? (_keys = new List<string>()); }
            set { _keys = value; }
        }

        public Guid CategoryId { get; set; }
    }

    public class TranslationResourceModel
    {
        public string Key { get; set; }

        public string Translation { get; set; }

        public string Override { get; set; }
    }
}
