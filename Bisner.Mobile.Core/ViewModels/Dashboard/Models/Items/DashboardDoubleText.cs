using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    /// <summary>
    /// Dashboard item that has 2 texts (for example : Dayname & month)
    /// </summary>
    public class DashboardDoubleText : DashboardItemBase
    {
        #region Constructor

        public DashboardDoubleText()
        {

        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.DoubleText; } }
        public MvxCommand Command { get; set; }
        public Action CommandAction { get; set; }
        public BisnerColor TextColor { get; set; }
        public string Text2 { get; set; }
        public BisnerColor TextColor2 { get; set; }

        #endregion Properties
    }
}