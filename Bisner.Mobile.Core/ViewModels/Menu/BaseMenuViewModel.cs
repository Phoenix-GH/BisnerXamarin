using System.Collections.Generic;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Menu
{
    public abstract class BaseMenuViewModel<TItem> : MvxViewModel
    {
        #region Constrcutor

        private List<TItem> _items;

        protected BaseMenuViewModel()
        {
            Setup();
        }

        private void Setup()
        {
            Items = BuildMenuItems();

            ItemSelectedCommand = new MvxCommand<TItem>(ItemSelected);
        }

        #endregion Constrcutor

        #region Items

        public List<TItem> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        protected abstract List<TItem> BuildMenuItems();

        protected MvxCommand<TItem> ItemSelectedCommand { get; private set; }

        protected virtual void ItemSelected(TItem item)
        {
            // Should be overridden for functionality
        }

        #endregion Items
    }
}