using Bisner.ApiModels.Central;

namespace Bisner.Mobile.Core.Communication.Models
{
    public class EventCategoryAndTypeModel
    {
        public ApiCentralEventCategoryModel Category { get; set; }

        public ApiCentralEventCategoryModel Type { get; set; }
    }
}