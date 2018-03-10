using System;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    public class EventCategoriesTemplateSelector : MvxTemplateSelector<IItemBase>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(IItemBase forItemObject)
        {
            if (forItemObject is Event)
                return Resource.Layout.event_item;

            if (forItemObject is AllEventsItem)
                return Resource.Layout.eventcategories_item_all;

            if (forItemObject is HeaderImage)
                return Resource.Layout.headerimage_item;

            if (forItemObject is EventCategoryDouble)
                return Resource.Layout.eventcategories_item_double;

            if (forItemObject is EventCategory)
                return Resource.Layout.eventcategories_item_single;

            throw new ArgumentException($"No layout defined for EventCategoryViewModel item of type {forItemObject.GetType().FullName}");
        }
    }
}