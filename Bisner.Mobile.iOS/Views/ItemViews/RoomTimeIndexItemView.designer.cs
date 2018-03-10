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
    [Register ("RoomTimeIndexItemView")]
    partial class RoomTimeIndexItemView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView clvTimeLine { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.OverlayImageView ivContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwStatus { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (clvTimeLine != null) {
                clvTimeLine.Dispose ();
                clvTimeLine = null;
            }

            if (ivContent != null) {
                ivContent.Dispose ();
                ivContent = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (vwStatus != null) {
                vwStatus.Dispose ();
                vwStatus = null;
            }
        }
    }
}