using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    public class DashboardTask : DashboardItemBase
    {
        #region Constructor

        public DashboardTask()
        {

        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.Task; } }
        public MvxCommand Command { get; set; }
        public Action CommandAction { get; set; }
        public BisnerColor TextColor { get; set; }

        #endregion Properties
    }
}