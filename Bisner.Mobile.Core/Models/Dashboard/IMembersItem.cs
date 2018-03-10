using Bisner.Mobile.Core.Models.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Dashboard
{
    public interface IMembersItem : IItemBase
    {
        string CompanyText { get; }

        string MembersText { get; }

        MvxCommand CompanyCommand { get; }

        MvxCommand MembersCommand { get; }
    }
}