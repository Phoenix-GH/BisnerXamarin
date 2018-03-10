using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Events
{
    public interface IEventCategory : IItemBase
    {
        /// <summary>
        /// Category name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Event type
        /// </summary>
        EventCategoryType Type { get; set; }

        /// <summary>
        /// If true all users can create this type of event
        /// </summary>
        bool UsersCanCreateEvent { get; set; }

        /// <summary>
        /// Category image
        /// </summary>
        IImage Image { get; set; }

        MvxCommand SelectedCommand { get; }
    }
}