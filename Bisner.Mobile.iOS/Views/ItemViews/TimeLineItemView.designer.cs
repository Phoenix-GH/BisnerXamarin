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
    [Register ("TimeLineItemView")]
    partial class TimeLineItemView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vw15 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vw30 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vw45 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblTime != null) {
                lblTime.Dispose ();
                lblTime = null;
            }

            if (vw15 != null) {
                vw15.Dispose ();
                vw15 = null;
            }

            if (vw30 != null) {
                vw30.Dispose ();
                vw30 = null;
            }

            if (vw45 != null) {
                vw45.Dispose ();
                vw45 = null;
            }
        }
    }
}