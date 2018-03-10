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

namespace Bisner.Mobile.iOS.Views.Feed
{
    [Register ("DetailsView")]
    partial class DetailsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint CommentButtonRightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.InputTextField Input { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputBoxBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputBoxHieghtConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InputBoxTopRuler { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InputContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputRightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.ExtendedTableView ItemTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Send { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TableBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CommentButtonRightConstraint != null) {
                CommentButtonRightConstraint.Dispose ();
                CommentButtonRightConstraint = null;
            }

            if (Input != null) {
                Input.Dispose ();
                Input = null;
            }

            if (InputBoxBottomConstraint != null) {
                InputBoxBottomConstraint.Dispose ();
                InputBoxBottomConstraint = null;
            }

            if (InputBoxHieghtConstraint != null) {
                InputBoxHieghtConstraint.Dispose ();
                InputBoxHieghtConstraint = null;
            }

            if (InputBoxTopRuler != null) {
                InputBoxTopRuler.Dispose ();
                InputBoxTopRuler = null;
            }

            if (InputContainer != null) {
                InputContainer.Dispose ();
                InputContainer = null;
            }

            if (InputRightConstraint != null) {
                InputRightConstraint.Dispose ();
                InputRightConstraint = null;
            }

            if (ItemTable != null) {
                ItemTable.Dispose ();
                ItemTable = null;
            }

            if (Send != null) {
                Send.Dispose ();
                Send = null;
            }

            if (TableBottomConstraint != null) {
                TableBottomConstraint.Dispose ();
                TableBottomConstraint = null;
            }
        }
    }
}