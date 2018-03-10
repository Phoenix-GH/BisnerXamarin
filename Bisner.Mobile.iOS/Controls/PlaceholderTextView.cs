using System;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public partial class PlaceholderTextView : UITextView
    {
        private string _placeholder;

        /// <summary>
        /// Gets or sets the placeholder to show prior to editing - doesn't exist on UITextView by default
        /// </summary>
        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                if (string.IsNullOrEmpty(Text) || Text == _placeholder)
                {
                    Text = value;
                }
                _placeholder = value;
            }
        }

        public PlaceholderTextView()
        {
            Initialize();
        }

        public PlaceholderTextView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }

        public PlaceholderTextView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        void Initialize()
        {
            Placeholder = "Please enter text";

            ShouldBeginEditing = t =>
            {
                if (Text == Placeholder)
                    Text = string.Empty;

                return true;
            };
            ShouldEndEditing = t =>
            {
                if (string.IsNullOrEmpty(Text))
                    Text = Placeholder;

                return true;
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ShouldBeginEditing = null;
                ShouldEndEditing = null;
            }

            base.Dispose(disposing);
        }
    }
}
