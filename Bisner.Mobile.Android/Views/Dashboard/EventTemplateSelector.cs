using System;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.Models.Feed;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    public class EventTemplateSelector : MvxTemplateSelector<IItemBase>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(IItemBase forItemObject)
        {
            if (forItemObject is IComment)
                return Resource.Layout.comment_item;

            if (forItemObject is IEvent)
                return Resource.Layout.event_detail_item;

            throw new ArgumentException($"No layout defined for EventVieModel item of type {forItemObject.GetType().FullName}");
        }
    }
}