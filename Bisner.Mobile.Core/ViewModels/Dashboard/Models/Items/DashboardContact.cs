using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    /// <summary>
    /// Dashboard contact container
    /// </summary>
    public class DashboardContact : DashboardItemBase
    {
        #region Constructor

        public DashboardContact()
        {
        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.Contact; } }
        public MvxCommand Command { get; set; }
        public Action CommandAction { get; set; }
        public BisnerColor TextColor { get; set; }
        public string AvatarUrl { get; set; }
        public string CompanyName { get; set; }
        public BisnerColor SubTextColor { get; set; }

        #endregion Properties
    }
}