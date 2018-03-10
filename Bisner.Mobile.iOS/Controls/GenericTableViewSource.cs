using System;
using System.Windows.Input;
using Foundation;
using UIKit;
using MvvmCross.Binding.iOS.Views;

namespace Bisner.Mobile.iOS.Controls
{
    public class GenericTableviewSourceWithHeight<TItem> : GenericTableViewSource<TItem> where TItem : class
    {
        #region Constructor

        public GenericTableviewSourceWithHeight(UITableView tableView) : base(tableView)
        {
        }

        public GenericTableviewSourceWithHeight(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region Height

        public Func<UITableView, NSIndexPath, TItem, nfloat> GetCellHeightFunc { get; set; }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            // Check if bound objects are of correct type
            var objectOfType = GetItemAt(indexPath) as TItem;
            if (objectOfType == null)
            {
                throw new Exception("Unable to cast item to type");
            }

            if (GetCellHeightFunc != null)
            {
                return GetCellHeightFunc(tableView, indexPath, objectOfType);
            }

            return base.GetHeightForRow(tableView, indexPath);
        }

        public Func<UITableView, NSIndexPath, TItem, nfloat> GetEstimatedHeightFunc { get; set; }

        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        {
            // Check if bound objects are of correct type
            var item = GetItemAt(indexPath);

            var objectOfType = item as TItem;
            if (objectOfType == null)
            {
                throw new Exception("Unable to cast item to type");
            }

            return GetEstimatedHeightFunc?.Invoke(tableView, indexPath, objectOfType) ?? 0;
        }

        #endregion Height
    }

    public class GenericTableViewSource<TItem> : MvxTableViewSource where TItem : class
    {
        #region Constructor

        public GenericTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        public GenericTableViewSource(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region Overrides

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            // Check if bound objects are of correct type
            var objectOfType = GetItemAt(indexPath) as TItem;
            if (objectOfType == null)
            {
                throw new Exception("Unable to cast item to type");
            }

            // Create cell
            if (CreateCellFunc == null && GetIdentifierFunc == null)
                throw new NullReferenceException("Please specify either a create cell function or a get cell identifier function");

            var cell = GetIdentifierFunc != null ? tableView.DequeueReusableCell(GetIdentifierFunc(item as TItem, indexPath)) : CreateCellFunc(tableView, indexPath);

            // Layout to give subviews sizes

            cell.SetNeedsLayout();
            cell.LayoutIfNeeded();

            // If set modify cell
            ModifyCellFunc?.Invoke(cell, indexPath, objectOfType);

            if (indexPath.Section + 1 == NumberOfSections(tableView) && RowsInSection(tableView, indexPath.Section) == indexPath.Row + 1)
            {
                // this is the last row in the last section
                OnReachedBottom();
            }

            return cell;
        }

        //public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        //{
        //    if (WillDisplayFunc != null)
        //    {
        //        // Check if bound objects are of correct type
        //        var objectOfType = GetItemAt(indexPath) as TItem;
        //        if (objectOfType == null)
        //        {
        //            throw new Exception("Unable to cast item to type");
        //        }

        //        WillDisplayFunc(tableView, cell, indexPath, objectOfType);
        //        return;
        //    }

        //    base.WillDisplay(tableView, cell, indexPath);
        //}

        #region Events

        public ICommand LoadMoreCommand { get; set; }

        private void OnReachedBottom()
        {
            LoadMoreCommand?.Execute(null);
        }

        #endregion Events

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);

            RowSelectionFunc?.Invoke(tableView, indexPath, GetItemAt(indexPath) as TItem);

            if (AutoDeselect)
            {
                tableView.DeselectRow(indexPath, true);
            }
        }

        #endregion Overrides

        #region Delegates

        public Func<UITableView, NSIndexPath, MvxTableViewCell> CreateCellFunc { get; set; }

        public Action<UITableViewCell, NSIndexPath, TItem> ModifyCellFunc { get; set; }

        public Func<TItem, NSIndexPath, NSString> GetIdentifierFunc { get; set; }

        /// <summary>
        /// Set this function to handle row clicks, if left null no action is taken when a row is selected
        /// </summary>
        public Action<UITableView, NSIndexPath, TItem> RowSelectionFunc { get; set; }

        //public Action<UITableView, UITableViewCell, NSIndexPath, TItem> WillDisplayFunc { get; set; }

        #endregion Delegates

        #region Properties

        /// <summary>
        /// Set to true to make the tableview auto deselect the cell after a row click
        /// </summary>
        public bool AutoDeselect { get; set; }

        #endregion Properties
    }
}