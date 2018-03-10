// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    [Register ("EventCategoriesView")]
    partial class EventCategoriesView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ItemsTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TableBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ItemsTable != null) {
                ItemsTable.Dispose ();
                ItemsTable = null;
            }

            if (TableBottomConstraint != null) {
                TableBottomConstraint.Dispose ();
                TableBottomConstraint = null;
            }
        }
    }
}