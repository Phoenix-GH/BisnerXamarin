using Bisner.Mobile.Core.Models.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.General
{
    public interface IImage : IItemBase
    {
        string Small { get; set; }

        string Medium { get; set; }

        string Large { get; set; }

        string OriginalFileName { get; set; }

        string MimeType { get; set; }

        MvxCommand ShowCommand { get; }
    }
}