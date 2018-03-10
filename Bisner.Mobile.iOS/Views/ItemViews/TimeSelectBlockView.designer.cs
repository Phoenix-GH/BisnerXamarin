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
    [Register ("TimeSelectBlockView")]
    partial class TimeSelectBlockView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView txvDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwBlock { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwIndicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwResize { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (txvDescription != null) {
                txvDescription.Dispose ();
                txvDescription = null;
            }

            if (vwBlock != null) {
                vwBlock.Dispose ();
                vwBlock = null;
            }

            if (vwIndicator != null) {
                vwIndicator.Dispose ();
                vwIndicator = null;
            }

            if (vwResize != null) {
                vwResize.Dispose ();
                vwResize = null;
            }
        }
    }
}