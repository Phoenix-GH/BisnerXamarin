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
    [Register ("BodySliderItemView")]
    partial class BodySliderItemView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.OverlayImageView ivContent { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ivContent != null) {
                ivContent.Dispose ();
                ivContent = null;
            }
        }
    }
}