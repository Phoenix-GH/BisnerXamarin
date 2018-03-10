using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    public class DashboardHeader : DashboardItemBase
    {
        #region Constructor

        public DashboardHeader()
        {

        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.Header; } }

        public MvxCommand Command { get; set; }

        public Action CommandAction { get; set; }

        public BisnerColor TextColor { get; set; }

        #endregion Properties
    }
}