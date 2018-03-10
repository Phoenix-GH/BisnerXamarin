// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Bisner.Mobile.iOS.Controls;
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed
{
    [Register ("FeedView")]
    partial class FeedView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView FeedTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FeedTable != null) {
                FeedTable.Dispose ();
                FeedTable = null;
            }
        }
    }
}