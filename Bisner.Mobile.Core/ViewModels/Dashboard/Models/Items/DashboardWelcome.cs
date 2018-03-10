using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    public class DashboardWelcome : DashboardItemBase
    {
        #region Constructor

        public DashboardWelcome()
        {
            
        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.Welcome; } }
        public MvxCommand Command { get; private set; }
        public Action CommandAction { get; set; }
        public BisnerColor TextColor { get; set; }
        public int Number { get; set; }

        #endregion Properties
    }
}