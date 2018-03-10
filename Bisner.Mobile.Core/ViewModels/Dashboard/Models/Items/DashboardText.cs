using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    public class DashboardText : DashboardItemBase
    {
        #region Constructor

        public DashboardText()
        {

        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.Text; } }
        
        public MvxCommand Command { get { return new MvxCommand(CommandAction); } }

        public Action CommandAction { get; set; }

        public BisnerColor TextColor { get; set; }

        #endregion Properties
    }
}