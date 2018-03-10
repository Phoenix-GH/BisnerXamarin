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

namespace Bisner.Mobile.iOS.Views.Feed
{
    [Register ("AddPostView")]
    partial class AddPostView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.AvatarImageView Avatar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BottomRuler2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView IconBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView ImageCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Bisner.Mobile.iOS.Controls.PlaceholderTextView Input { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InputBoxHieghtConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MentionUser { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PickImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TakeImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Avatar != null) {
                Avatar.Dispose ();
                Avatar = null;
            }

            if (BottomConstraint != null) {
                BottomConstraint.Dispose ();
                BottomConstraint = null;
            }

            if (BottomRuler2 != null) {
                BottomRuler2.Dispose ();
                BottomRuler2 = null;
            }

            if (IconBox != null) {
                IconBox.Dispose ();
                IconBox = null;
            }

            if (ImageCollection != null) {
                ImageCollection.Dispose ();
                ImageCollection = null;
            }

            if (Input != null) {
                Input.Dispose ();
                Input = null;
            }

            if (InputBoxHieghtConstraint != null) {
                InputBoxHieghtConstraint.Dispose ();
                InputBoxHieghtConstraint = null;
            }

            if (MentionUser != null) {
                MentionUser.Dispose ();
                MentionUser = null;
            }

            if (PickImage != null) {
                PickImage.Dispose ();
                PickImage = null;
            }

            if (TakeImage != null) {
                TakeImage.Dispose ();
                TakeImage = null;
            }
        }
    }
}