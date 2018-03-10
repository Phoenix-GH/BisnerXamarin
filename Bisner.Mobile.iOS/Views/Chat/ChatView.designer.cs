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

namespace Bisner.Mobile.iOS.Views.Chat
{
    [Register ("ChatView")]
    partial class ChatView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ConversationTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ConversationTable != null) {
                ConversationTable.Dispose ();
                ConversationTable = null;
            }
        }
    }
}