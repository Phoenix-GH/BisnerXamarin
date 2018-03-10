using System;
using Bisner.Mobile.Core.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items
{
    public class DashboardSearch : DashboardItemBase
    {
        #region Constructor

        public DashboardSearch()
        {

        }

        #endregion Constructor

        #region Properties

        public override DashboardItemType Type { get { return DashboardItemType.Search; } }
        public MvxCommand Command { get; set; }
        public Action CommandAction { get; set; }
        public BisnerColor TextColor { get; set; }
        public string PlaceholderText { get; set; }

        #endregion Properties
    }
}