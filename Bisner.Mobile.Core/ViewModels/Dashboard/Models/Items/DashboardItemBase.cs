namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    /// <summary>
    /// Dashboard item base class
    /// </summary>
    public abstract class DashboardItemBase : IDashboardItem
    {
        /// <summary>
        /// The type of the item
        /// </summary>
        public abstract DashboardItemType Type { get; }

        /// <summary>
        /// The text of the item
        /// </summary>
        public string Text { get; set; }
    }
}
