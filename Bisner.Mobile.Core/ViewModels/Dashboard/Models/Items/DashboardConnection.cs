using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    /// <summary>
    /// Item representing a dashboard contact
    /// </summary>
    public class DashboardConnection : DashboardItemBase
    {
        #region Constructor



        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.Connection; } }


        public MvxCommand Command { get; set; }
        public Action CommandAction { get; set; }
        public BisnerColor TextColor { get; set; }
        public string Location { get; set; }
        public BisnerColor LocationTextColor { get; set; }

        #endregion Properties
    }
}