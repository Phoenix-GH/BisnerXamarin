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
using UIKit;

namespace Bisner.Mobile.iOS.Views.Chat
{
    [Register ("ChatConversationView")]
    partial class ChatConversationView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.InputTextField ChatInput { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InputBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputBoxBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InputBoxTopRuler { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputRightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.ExtendedTableView MessageTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Send { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SendRightConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ChatInput != null) {
                ChatInput.Dispose ();
                ChatInput = null;
            }

            if (InputBox != null) {
                InputBox.Dispose ();
                InputBox = null;
            }

            if (InputBoxBottomConstraint != null) {
                InputBoxBottomConstraint.Dispose ();
                InputBoxBottomConstraint = null;
            }

            if (InputBoxTopRuler != null) {
                InputBoxTopRuler.Dispose ();
                InputBoxTopRuler = null;
            }

            if (InputRightConstraint != null) {
                InputRightConstraint.Dispose ();
                InputRightConstraint = null;
            }

            if (MessageTable != null) {
                MessageTable.Dispose ();
                MessageTable = null;
            }

            if (Send != null) {
                Send.Dispose ();
                Send = null;
            }

            if (SendRightConstraint != null) {
                SendRightConstraint.Dispose ();
                SendRightConstraint = null;
            }
        }
    }
}