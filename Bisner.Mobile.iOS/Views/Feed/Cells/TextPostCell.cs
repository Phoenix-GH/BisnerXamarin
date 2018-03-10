using System;
using Bisner.Mobile.Core.ViewModels.Feed.Items;
using Foundation;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    partial class TextPostCell : TextPostCellBase<FeedTextPost>
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("TextPostCell");

        public TextPostCell(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor
    }
}
