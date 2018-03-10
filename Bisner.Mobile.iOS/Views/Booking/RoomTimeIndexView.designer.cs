// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [Register ("RoomTimeIndexView")]
    partial class RoomTimeIndexView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TableBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tvRoomTimeIndex { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TableBottomConstraint != null) {
                TableBottomConstraint.Dispose ();
                TableBottomConstraint = null;
            }

            if (tvRoomTimeIndex != null) {
                tvRoomTimeIndex.Dispose ();
                tvRoomTimeIndex = null;
            }
        }
    }
}