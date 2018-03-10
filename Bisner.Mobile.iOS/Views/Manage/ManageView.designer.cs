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

namespace Bisner.Mobile.iOS.Views.Manage
{
    [Register ("ManageView")]
    partial class ManageView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.ExtendedTableView ItemTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ItemTable != null) {
                ItemTable.Dispose ();
                ItemTable = null;
            }
        }
    }
}