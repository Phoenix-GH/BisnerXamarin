using System;
using Bisner.Mobile.iOS.Extensions;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Base
{
    public abstract class KeyboardListenerViewBase<TViewModel> : ViewBase<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Constructor

        private NSObject _willShowToken, _willHideToken;

        protected KeyboardListenerViewBase(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //Only do this if required
            if (HandlesKeyboardNotifications)
            {
                RegisterForKeyboardNotifications();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //Only do this if required
            if (HandlesKeyboardNotifications)
            {
                UnRegisterForKeyboardNotifications();
            }
        }

        #endregion ViewController

        #region Keyboard adjust

        /// <summary>
        /// Set this field to any view inside the scroll view to center this view instead of the current responder
        /// </summary>
        protected UIView ViewToCenterOnKeyboardShown { get; set; }
        protected UITableViewScrollPosition PositionToScrollView { get; set; }

        public virtual bool HandlesKeyboardNotifications => false;

        protected virtual void RegisterForKeyboardNotifications()
        {
            _willHideToken = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            _willShowToken = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
        }

        protected virtual void UnRegisterForKeyboardNotifications()
        {
            if (_willHideToken != null)
            {
                _willHideToken.Dispose();
            }

            if (_willShowToken != null)
            {
                _willShowToken.Dispose();
            }
        }

        /// <summary>
        /// Gets the UIView that represents the "active" user input control (e.g. textfield, or button under a text field)
        /// </summary>
        /// <returns>
        /// A <see cref="UIView"/>
        /// </returns>
        protected virtual UIView KeyboardGetActiveView()
        {
            return View.FindFirstResponder();
        }

        private void OnKeyboardNotification(NSNotification notification)
        {
            if (!IsViewLoaded) return;

            //Check if the keyboard is becoming visible
            var visible = notification.Name == UIKeyboard.WillShowNotification;

            //Start an animation, using values from the keyboard
            UIView.SetAnimationBeginsFromCurrentState(true);
            UIView.SetAnimationDelegate(this);
            UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

            //Pass the notification, calculating keyboard height, etc.
            var landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
            var keyboardFrame = visible
                ? UIKeyboard.FrameEndFromNotification(notification)
                : UIKeyboard.FrameBeginFromNotification(notification);

            UIView.Animate(UIKeyboard.AnimationDurationFromNotification(notification), () =>
            {
                OnKeyboardChanged(visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
            }, () =>
            {
                OnAfterKeyboardAnimationFinished(visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
            });
        }

        /// <summary>
        /// Override this method to apply custom logic when the keyboard is shown/hidden
        /// </summary>
        /// <param name='visible'>
        /// If the keyboard is visible
        /// </param>
        /// <param name='keyboardHeight'>
        /// Calculated height of the keyboard (width not generally needed here)
        /// </param>
        protected virtual void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            var activeView = ViewToCenterOnKeyboardShown ?? KeyboardGetActiveView();
            if (activeView == null)
                return;

            var scrollView = activeView.FindSuperviewOfType(View, typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
                return;

            if (!visible)
                RestoreScrollPosition(scrollView);
            else
                CenterViewInScroll(activeView, scrollView, keyboardHeight);
        }

        /// <summary>
        /// Called after the animations of the keyboard have been committed
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="keyboardHeight"></param>
        protected virtual void OnAfterKeyboardAnimationFinished(bool visible, nfloat keyboardHeight)
        {
            // Do stuff
        }

        protected virtual void CenterViewInScroll(UIView viewToCenter, UIScrollView scrollView, nfloat keyboardHeight)
        {
            var contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardHeight, 0.0f);
            scrollView.ContentInset = contentInsets;
            scrollView.ScrollIndicatorInsets = contentInsets;

            // Position of the active field relative isnside the scroll view
            var relativeFrame = viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

            var landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
            var spaceAboveKeyboard = (landscape ? scrollView.Frame.Width : scrollView.Frame.Height) - keyboardHeight;

            // Move the active field to the center of the available space
            nfloat offset = 0;

            switch (PositionToScrollView)
            {
                case UITableViewScrollPosition.None:
                    // Offset = 0
                    break;
                case UITableViewScrollPosition.Top:
                    offset = relativeFrame.Y - spaceAboveKeyboard;
                    break;
                case UITableViewScrollPosition.Middle:
                    offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2;
                    break;
                case UITableViewScrollPosition.Bottom:
                    offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height - KeyboardScrollExtraOffset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            scrollView.ContentOffset = new CGPoint(0, offset);
        }

        protected virtual nfloat KeyboardScrollExtraOffset => 0;

        protected virtual void RestoreScrollPosition(UIScrollView scrollView)
        {
            scrollView.ContentInset = UIEdgeInsets.Zero;
            scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
        }

        /// <summary>
        /// Call it to force dismiss keyboard when background is tapped
        /// </summary>
        protected void DismissKeyboardOnBackgroundTap()
        {
            // Add gesture recognizer to hide keyboard
            var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tap.AddTarget(() => View.EndEditing(true));
            View.AddGestureRecognizer(tap);
        }

        #endregion
    }
}