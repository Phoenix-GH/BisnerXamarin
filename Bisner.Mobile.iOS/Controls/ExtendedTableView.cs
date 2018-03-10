using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Service;
using CoreGraphics;
using Foundation;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    partial class ExtendedTableView : UITableView
    {
        #region Constructor

        public ExtendedTableView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region Methods

        public void ScrollToBottomAnimated(bool animated)
        {
            var contentSize = ContentSize;
            var boundsSize = Bounds.Size;
            if (contentSize.Height > boundsSize.Height - ContentInset.Bottom)
            {
                var bottomOffset = new CGPoint(0, contentSize.Height - boundsSize.Height + ContentInset.Bottom);
                SetContentOffset(bottomOffset, animated);
            }
        }

        public void ScrollToBottom(bool animated)
        {
            try
            {
                if (NumberOfSections() == 0)
                    return;

                var lastSectionNumber = NumberOfSections() - 1;

                var number = NumberOfRowsInSection(lastSectionNumber) - 1;

                var items = (int)number;
                if (items == 0 || items == -1)
                    return;

                var finalRow = (int)NMath.Max(0, items);
                var finalIndexPath = NSIndexPath.FromRowSection(finalRow, lastSectionNumber);
                ScrollToRow(finalIndexPath, UITableViewScrollPosition.Bottom, animated);
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        public NSIndexPath IndexPathForLastRow
        {
            get
            {
                //:[self numberOfRowsInSection: self.numberOfSections - 1] - 1 inSection: self.numberOfSections - 1]
                var indexPath = NSIndexPath.FromRowSection(NumberOfRowsInSection(NumberOfSections() - 1) - 1, NumberOfSections() - 1);

                return indexPath;
            }
        }

        public override void InsertRows(NSIndexPath[] atIndexPaths, UITableViewRowAnimation withRowAnimation)
        {
            base.InsertRows(atIndexPaths, withRowAnimation);
        }

        public bool CheckIfLastRow(NSIndexPath path)
        {
            var lastSectionNumber = NumberOfSections() - 1;

            var numberOfRows = NumberOfRowsInSection(lastSectionNumber) - 1;

            var numberInt = (int)numberOfRows;
            if (numberInt == 0)
                return false;

            var finalRow = (int)NMath.Max(0, numberInt);
            var finalIndexPath = NSIndexPath.FromRowSection(finalRow, lastSectionNumber);

            return path.Row == finalIndexPath.Row && path.Section == finalIndexPath.Section;
        }

        #endregion Methods

        #region Modifications

        public void SetDelaysContentTouches(bool enabled)
        {
            foreach (var subview in Subviews)
            {
                if (subview is UIScrollView)
                {
                    ((UIScrollView)subview).DelaysContentTouches = enabled;
                }
            }
        }

        #endregion Modifications
    }
}
