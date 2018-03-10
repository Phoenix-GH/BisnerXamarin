using System.Collections.Generic;

namespace Bisner.Mobile.Core.ViewModels.Menu
{
    /// <summary>
    /// Left sliding menu view model
    /// </summary>
    public class IosBaseMenuViewModel : BaseMenuViewModel<IMenuItem>
    {
        #region Constructor

        #endregion Constructor

        #region Items

        protected override List<IMenuItem> BuildMenuItems()
        {
            return new List<IMenuItem>
            {
                new MenuItem(MenuItemType.Home) { Title = "Home" },
                new MenuItem(MenuItemType.Members) { Title = "Members" },
                new MenuItem(MenuItemType.Events) { Title = "Events" },
                new MenuItem(MenuItemType.Groups) { Title = "Groups" },
                new MenuItem(MenuItemType.Chat) { Title = "Chat" },
                new MenuItem(MenuItemType.Collaborate) { Title = "Collaborate" },
            };
        }

        protected override void ItemSelected(IMenuItem item)
        {
            // TODO: Navigation
        }

        #endregion Items
    }
}