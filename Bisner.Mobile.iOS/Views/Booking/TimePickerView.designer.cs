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
    [Register ("TimePickerView")]
    partial class TimePickerView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBook { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnClose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tvTimePicker { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnBook != null) {
                btnBook.Dispose ();
                btnBook = null;
            }

            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (tvTimePicker != null) {
                tvTimePicker.Dispose ();
                tvTimePicker = null;
            }
        }
    }
}