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
    [Register ("JobboardView")]
    partial class JobboardView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBookingNew { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tvBookings { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBookingNew != null) {
                btnBookingNew.Dispose ();
                btnBookingNew = null;
            }

            if (tvBookings != null) {
                tvBookings.Dispose ();
                tvBookings = null;
            }
        }
    }
}