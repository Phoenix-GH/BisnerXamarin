// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    [Register ("TimePickerItemView")]
    partial class TimePickerItemView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView txvReserve { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwSelectedBg { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblTime != null) {
                lblTime.Dispose ();
                lblTime = null;
            }

            if (txvReserve != null) {
                txvReserve.Dispose ();
                txvReserve = null;
            }

            if (vwSelectedBg != null) {
                vwSelectedBg.Dispose ();
                vwSelectedBg = null;
            }
        }
    }
}