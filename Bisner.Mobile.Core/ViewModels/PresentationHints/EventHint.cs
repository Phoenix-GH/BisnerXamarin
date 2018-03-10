using System;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.PresentationHints
{
    public class EventHint : MvxPresentationHint
    {
        public Guid EventId { get; set; }
    }
}