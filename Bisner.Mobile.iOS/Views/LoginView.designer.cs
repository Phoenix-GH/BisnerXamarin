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

namespace Bisner.Mobile.iOS.Views
{
    [Register ("LoginView")]
    partial class LoginView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ScrollViewBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (ScrollViewBottomConstraint != null) {
                ScrollViewBottomConstraint.Dispose ();
                ScrollViewBottomConstraint = null;
            }
        }
    }
}