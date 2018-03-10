using System.Collections.Generic;
using Bisner.ApiModels.Central;

namespace Bisner.Mobile.Core.Communication.Models
{
    public class EventCategoriesAndTypesModel
    {
        public List<ApiCentralEventCategoryModel> Categories { get; set; }

        public List<ApiCentralEventCategoryModel> Types { get; set; }
    }
}