using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Events
{
    public class EventCategory : ItemBase, IEventCategory
    {
        private MvxCommand _selectedCommand;

        public string Name { get; set; }

        public EventCategoryType Type { get; set; }

        public bool UsersCanCreateEvent { get; set; }

        public IImage Image { get; set; }

        public MvxCommand SelectedCommand => _selectedCommand ?? (_selectedCommand = new MvxCommand(OnSelected));

        private void OnSelected()
        {
            ShowViewModel<EventsViewModel>(new { categoryId = Id });
        }
    }
}