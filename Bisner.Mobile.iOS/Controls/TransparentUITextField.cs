using System;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public partial class TransparentUITextField : UITextField
    {
        public TransparentUITextField(IntPtr handle)
            : base(handle)
        {
            SetupApearance();
        }

        public TransparentUITextField()
        {
            SetupApearance();
        }

        public void SetupApearance()
        {
            BackgroundColor = UIColor.Clear;
            BorderStyle = UITextBorderStyle.None;
        }
    }
}
