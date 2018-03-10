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
    [Register ("DashboardView")]
    partial class DashboardView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnShowMember { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView clvBodySlider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView clvHeaderSlider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cntSearchbarHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint HeaderSliderHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivShowAllMembers { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblShowAllMembers { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MembersArrowImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MembersBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MembersContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MembersHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MembersMiddleBorder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MembersTopBorder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scvMain { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint tableHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tvRouter { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txfSearch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnShowMember != null) {
                btnShowMember.Dispose ();
                btnShowMember = null;
            }

            if (clvBodySlider != null) {
                clvBodySlider.Dispose ();
                clvBodySlider = null;
            }

            if (clvHeaderSlider != null) {
                clvHeaderSlider.Dispose ();
                clvHeaderSlider = null;
            }

            if (cntSearchbarHeight != null) {
                cntSearchbarHeight.Dispose ();
                cntSearchbarHeight = null;
            }

            if (HeaderSliderHeight != null) {
                HeaderSliderHeight.Dispose ();
                HeaderSliderHeight = null;
            }

            if (ivShowAllMembers != null) {
                ivShowAllMembers.Dispose ();
                ivShowAllMembers = null;
            }

            if (lblShowAllMembers != null) {
                lblShowAllMembers.Dispose ();
                lblShowAllMembers = null;
            }

            if (MembersArrowImage != null) {
                MembersArrowImage.Dispose ();
                MembersArrowImage = null;
            }

            if (MembersBottomConstraint != null) {
                MembersBottomConstraint.Dispose ();
                MembersBottomConstraint = null;
            }

            if (MembersContainer != null) {
                MembersContainer.Dispose ();
                MembersContainer = null;
            }

            if (MembersHeightConstraint != null) {
                MembersHeightConstraint.Dispose ();
                MembersHeightConstraint = null;
            }

            if (MembersMiddleBorder != null) {
                MembersMiddleBorder.Dispose ();
                MembersMiddleBorder = null;
            }

            if (MembersTopBorder != null) {
                MembersTopBorder.Dispose ();
                MembersTopBorder = null;
            }

            if (scvMain != null) {
                scvMain.Dispose ();
                scvMain = null;
            }

            if (tableHeight != null) {
                tableHeight.Dispose ();
                tableHeight = null;
            }

            if (tvRouter != null) {
                tvRouter.Dispose ();
                tvRouter = null;
            }

            if (txfSearch != null) {
                txfSearch.Dispose ();
                txfSearch = null;
            }
        }
    }
}