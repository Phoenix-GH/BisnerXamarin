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
    [Register ("DatePickerView")]
    partial class DatePickerView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnClose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnContinue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNextMonth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPrevMonth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCurrentMonth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ScrollViewBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwCalendar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (btnContinue != null) {
                btnContinue.Dispose ();
                btnContinue = null;
            }

            if (btnNextMonth != null) {
                btnNextMonth.Dispose ();
                btnNextMonth = null;
            }

            if (btnPrevMonth != null) {
                btnPrevMonth.Dispose ();
                btnPrevMonth = null;
            }

            if (lblCurrentMonth != null) {
                lblCurrentMonth.Dispose ();
                lblCurrentMonth = null;
            }

            if (ScrollViewBottomConstraint != null) {
                ScrollViewBottomConstraint.Dispose ();
                ScrollViewBottomConstraint = null;
            }

            if (vwCalendar != null) {
                vwCalendar.Dispose ();
                vwCalendar = null;
            }
        }
    }
}