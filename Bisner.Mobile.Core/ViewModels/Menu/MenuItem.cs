namespace Bisner.Mobile.Core.ViewModels.Menu
{
    public class MenuItem : IMenuItem
    {
        #region Constructor

        public MenuItem(MenuItemType type)
        {
            Type = type;
        }

        #endregion Constructor

        #region Properties

        public MenuItemType Type { get; private set; }

        public string Title { get; set; }

        #endregion Properties
    }
}