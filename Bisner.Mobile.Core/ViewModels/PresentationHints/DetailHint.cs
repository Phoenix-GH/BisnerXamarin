using System;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.PresentationHints
{
    public class DetailHint : MvxPresentationHint
    {
        public Guid PostId { get; set; }
    }
}