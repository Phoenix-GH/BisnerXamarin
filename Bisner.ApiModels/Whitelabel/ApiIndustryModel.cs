using System;
using System.Collections.Generic;

namespace Bisner.ApiModels.Whitelabel
{
    public class ApiIndustryModel
    {
        private List<ApiIndustryModel> _subIndustries;

        /// <summary>
        /// Industry id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Industry name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sub industries
        /// </summary>
        public List<ApiIndustryModel> SubIndustries
        {
            get { return _subIndustries ?? (_subIndustries = new List<ApiIndustryModel>()); }
            set { _subIndustries = value; }
        }
    }
}