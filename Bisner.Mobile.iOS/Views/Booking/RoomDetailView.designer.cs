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
    [Register ("RoomDetailView")]
    partial class RoomDetailView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCheck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLocationName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPersons { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView clvBodySlider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ImageContainerHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.OverlayImageView ivContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAboutDesc1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAboutDesc2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAboutTitle1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAboutTitle2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwStatus { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnCheck != null) {
                btnCheck.Dispose ();
                btnCheck = null;
            }

            if (btnLocationName != null) {
                btnLocationName.Dispose ();
                btnLocationName = null;
            }

            if (btnPersons != null) {
                btnPersons.Dispose ();
                btnPersons = null;
            }

            if (clvBodySlider != null) {
                clvBodySlider.Dispose ();
                clvBodySlider = null;
            }

            if (ImageContainerHeight != null) {
                ImageContainerHeight.Dispose ();
                ImageContainerHeight = null;
            }

            if (ivContent != null) {
                ivContent.Dispose ();
                ivContent = null;
            }

            if (lblAboutDesc1 != null) {
                lblAboutDesc1.Dispose ();
                lblAboutDesc1 = null;
            }

            if (lblAboutDesc2 != null) {
                lblAboutDesc2.Dispose ();
                lblAboutDesc2 = null;
            }

            if (lblAboutTitle1 != null) {
                lblAboutTitle1.Dispose ();
                lblAboutTitle1 = null;
            }

            if (lblAboutTitle2 != null) {
                lblAboutTitle2.Dispose ();
                lblAboutTitle2 = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (vwStatus != null) {
                vwStatus.Dispose ();
                vwStatus = null;
            }
        }
    }
}