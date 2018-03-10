using System;
using Bisner.Mobile.Core.Models.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Feed
{
    public interface IComment : IItemBase, IDisplayUser
    {
        string Text { get; set; }
        DateTime DateTime { get; set; }
        MvxCommand UserCommand { get; }
    }
}