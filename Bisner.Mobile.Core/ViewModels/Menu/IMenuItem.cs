namespace Bisner.Mobile.Core.ViewModels.Menu
{
    public interface IMenuItem
    {
        MenuItemType Type { get; }

        string Title { get; set; }
    }
}