using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Dashboard.Models.Items;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard.Models.Cards
{
    /// <summary>
    /// A section containing all items of a dashboard card
    /// </summary>
    public abstract class DashboardCardSectionBase<T> : List<T>, IDashboardCardSectionBase where T : IDashboardItem
    {
        #region Constructor

        private readonly Action _footerAction;
        private MvxCommand _footerCommand;

        /// <summary>
        /// A section containing all items of a dashboard card
        /// </summary>
        /// <param name="footerAction">The action to be executed when the footer is clicked</param>
        protected DashboardCardSectionBase(Action footerAction = null)
        {
            _footerAction = footerAction;
        }

        #endregion Constructor

        #region Private members

        /// <summary>
        /// Execute the footer command when an action has been set
        /// </summary>
        private void ExecuteCommand()
        {
            if (_footerAction != null)
            {
                _footerAction.Invoke();
            }
        }

        #endregion Private members

        #region Properties

        /// <summary>
        /// The title of the card
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// The text of the footer
        /// </summary>
        public abstract string FooterText { get; }

        /// <summary>
        /// The command the footer will call
        /// </summary>
        public MvxCommand FooterCommand
        {
            get { return _footerCommand ?? (_footerCommand = new MvxCommand(ExecuteCommand)); }
        }

        #endregion Properties
    }
}
