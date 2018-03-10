using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public partial class InputTextField : UITextField, IUITextFieldDelegate
    {
        public UIEdgeInsets EdgeInsets { get; set; }

        public InputTextField()
        {

        }

        public InputTextField(IntPtr handle)
            : base(handle)
        {
            EdgeInsets = UIEdgeInsets.Zero;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Layer.CornerRadius = 7.0f;
            Layer.BorderWidth = 1.0f;
            Layer.ShadowRadius = 1.0f;
            Layer.ShadowColor = UIColor.DarkGray.CGColor;
            Layer.ShadowOffset = new CGSize(0f, 0.5f);
            Layer.ShadowOpacity = 0.1f;
            Layer.BorderColor = iOS.Appearance.Colors.SubTextColor.CGColor;
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            return base.EditingRect(InsetRect(forBounds, EdgeInsets));
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            return base.EditingRect(InsetRect(forBounds, EdgeInsets));
        }

        // Workaround until this method is available in Xamarin.iOS
        public static CGRect InsetRect(CGRect rect, UIEdgeInsets insets)
        {
            return new CGRect(
                rect.X + insets.Left,
                rect.Y + insets.Top,
                rect.Width - insets.Left - insets.Right,
                rect.Height - insets.Top - insets.Bottom);
        }
    }

    public class HtmlTextView : UITextView
    {
        #region Constructor

        public HtmlTextView(Func<string, bool> urlFunc = null)
        {
            Setup(urlFunc);
        }

        #endregion Constructor

        #region Setup

        private void Setup(Func<string, bool> urlFunc)
        {
            ScrollEnabled = false;
            Editable = false;
            DataDetectorTypes = UIDataDetectorType.All;
            TextContainerInset = new UIEdgeInsets(-5, 0, -15, 0);
            TintColor = iOS.Appearance.Colors.BisnerBlue;
            Delegate = new HtmlTextViewDelegate(urlFunc);
        }

        #endregion Setup

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                ShouldInteractWithUrl = null;
            }
        }

        #endregion Dispose
    }

    public class HtmlTextViewDelegate : UITextViewDelegate
    {
        public HtmlTextViewDelegate(Func<string, bool> urlFunc)
        {
            InteractUrlFunc = urlFunc;
        }

        public override bool ShouldInteractWithUrl(UITextView textView, NSUrl url, NSRange characterRange)
        {
            if (url.Scheme == "mention")
            {
                Guid userId;
                if (Guid.TryParse(url.Host, out userId))
                {
                    var request = new MvxViewModelRequest(typeof(UserViewModel), new MvxBundle(new Dictionary<string, string>
                    {
                        { "userId", userId.ToString() },
                    }), null, null);

                    var dispatcher = Mvx.Resolve<IMvxViewDispatcher>();

                    dispatcher.ShowViewModel(request);

                    return false;
                }
            }

            if (Settings.ShowExternalUrlWarning && InteractUrlFunc != null)
            {
                InteractUrlFunc.Invoke(url.AbsoluteString);
                
                return false;
            }

            return true;
        }

        public Func<string, bool> InteractUrlFunc { get; set; }
    }
}
