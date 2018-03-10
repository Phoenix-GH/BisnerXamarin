// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    [Register ("EventDetailView")]
    partial class EventDetailView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.InputTextField Input { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputBoxHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InputBoxTopBorder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InputContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputContainerBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputRightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Send { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SendRightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.ExtendedTableView TableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Input != null) {
                Input.Dispose ();
                Input = null;
            }

            if (InputBoxHeightConstraint != null) {
                InputBoxHeightConstraint.Dispose ();
                InputBoxHeightConstraint = null;
            }

            if (InputBoxTopBorder != null) {
                InputBoxTopBorder.Dispose ();
                InputBoxTopBorder = null;
            }

            if (InputContainer != null) {
                InputContainer.Dispose ();
                InputContainer = null;
            }

            if (InputContainerBottomConstraint != null) {
                InputContainerBottomConstraint.Dispose ();
                InputContainerBottomConstraint = null;
            }

            if (InputRightConstraint != null) {
                InputRightConstraint.Dispose ();
                InputRightConstraint = null;
            }

            if (Send != null) {
                Send.Dispose ();
                Send = null;
            }

            if (SendRightConstraint != null) {
                SendRightConstraint.Dispose ();
                SendRightConstraint = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}