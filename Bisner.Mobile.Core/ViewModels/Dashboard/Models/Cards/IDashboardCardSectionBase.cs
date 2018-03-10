using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Cards
{
    public interface IDashboardCardSectionBase
    {
        /// <summary>
        /// The title of the card
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The text of the footer
        /// </summary>
        string FooterText { get; }

        /// <summary>
        /// The command the footer will call
        /// </summary>
        MvxCommand FooterCommand { get; }

        int Capacity { get; set; }
        int Count { get; }
    }
}