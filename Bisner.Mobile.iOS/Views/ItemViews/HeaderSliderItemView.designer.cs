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

namespace Bisner.Mobile.iOS.Views.ItemViews
{
    [Register ("HeaderSliderItemView")]
    partial class HeaderSliderItemView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBookRoom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDetail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnShowAll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.OverlayImageView ivContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblShowAll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSubTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwBookingStatus { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBookRoom != null) {
                btnBookRoom.Dispose ();
                btnBookRoom = null;
            }

            if (btnDetail != null) {
                btnDetail.Dispose ();
                btnDetail = null;
            }

            if (btnShowAll != null) {
                btnShowAll.Dispose ();
                btnShowAll = null;
            }

            if (ivContent != null) {
                ivContent.Dispose ();
                ivContent = null;
            }

            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (lblShowAll != null) {
                lblShowAll.Dispose ();
                lblShowAll = null;
            }

            if (lblSubTitle != null) {
                lblSubTitle.Dispose ();
                lblSubTitle = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (vwBookingStatus != null) {
                vwBookingStatus.Dispose ();
                vwBookingStatus = null;
            }
        }
    }
}