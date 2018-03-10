using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    /// <summary>
    /// File dashboard item
    /// </summary>
    public class DashboardFile : DashboardItemBase
    {
        #region Constructor

        public DashboardFile()
        {

        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.File; } }
        public MvxCommand Command { get; set; }
        public Action CommandAction { get; set; }
        public BisnerColor TextColor { get; set; }
        public string Extension { get; set; }

        #endregion Properties
    }
}