using System;
using Foundation;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    partial class UnknownPostCell : UITableViewCell
    {
        public readonly static NSString Identifier = new NSString("UnknownPostCell");

        public UnknownPostCell(IntPtr handle)
            : base(handle)
        {
        }
    }
}
