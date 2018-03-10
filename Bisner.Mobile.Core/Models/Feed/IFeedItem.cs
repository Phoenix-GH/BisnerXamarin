using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.Feed
{
    public interface IFeedItem : IItemBase
    {
        DateTime DateTime { get; set; }
    }
}