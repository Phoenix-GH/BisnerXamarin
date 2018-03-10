using System;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public class ScrollAwareTableViewSource<TItem> : GenericTableviewSourceWithHeight<TItem> where TItem : class
    {
        #region Constructor

        public ScrollAwareTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        public ScrollAwareTableViewSource(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region Properties

        public NSLayoutConstraint ConstraintToChange { get; set; }

        public UIView ViewToMove { get; set; }

        #endregion Properties

        #region Scroll

        private nfloat _previousOffset;

        /// <summary>
        /// We have scrolled. We may want to hide the tab.
        /// </summary>
        public override void Scrolled(UIScrollView scrollView)
        {
            if (ConstraintToChange == null || ViewToMove == null)
            {
                throw new Exception("NO CONSTRAINT AND VIEW SET!!!");
            }

            var currentOffset = scrollView.ContentOffset.Y;
            var height = scrollView.Frame.Size.Height;
            var distanceFromBottom = scrollView.ContentSize.Height - currentOffset;

            if (_previousOffset < currentOffset && distanceFromBottom > height)
            {
                if (currentOffset > ViewToMove.Frame.Height)
                    currentOffset = ViewToMove.Frame.Height;
                ConstraintToChange.Constant += _previousOffset - currentOffset;
                _previousOffset = currentOffset;
            }
            else {
                if (_previousOffset > currentOffset)
                {
                    if (currentOffset < 0)
                        currentOffset = 0;
                    ConstraintToChange.Constant += _previousOffset - currentOffset;
                    _previousOffset = currentOffset;
                }
            }
        }

        public enum ScrollDirection
        {
            Unknown,
            Up,
            Down
        }

        #endregion Scroll
    }
}