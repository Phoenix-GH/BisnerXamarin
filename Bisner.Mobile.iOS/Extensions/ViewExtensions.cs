using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Extensions
{
    public static class ViewExtensions
    {
        /// <summary>
        /// Find the first responder in the <paramref name="view"/>'s subview hierarchy
        /// </summary>
        /// <param name="view">
        /// A <see cref="UIView"/>
        /// </param>
        /// <returns>
        /// A <see cref="UIView"/> that is the first responder or null if there is no first responder
        /// </returns>
        public static UIView FindFirstResponder(this UIView view)
        {
            if (view.IsFirstResponder)
            {
                return view;
            }

            foreach (UIView subView in view.Subviews)
            {
                var firstResponder = subView.FindFirstResponder();
                if (firstResponder != null)
                    return firstResponder;
            }

            return null;
        }

        /// <summary>
        /// Find the first Superview of the specified type (or descendant of)
        /// </summary>
        /// <param name="view">
        /// A <see cref="UIView"/>
        /// </param>
        /// <param name="stopAt">
        /// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
        /// </param>
        /// <param name="type">
        /// A <see cref="Type"/> to look for, this should be a UIView or descendant type
        /// </param>
        /// <returns>
        /// A <see cref="UIView"/> if it is found, otherwise null
        /// </returns>
        public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
        {
            if (view.Superview != null)
            {
                if (type.IsAssignableFrom(view.Superview.GetType()))
                {
                    return view.Superview;
                }

                if (view.Superview != stopAt)
                    return view.Superview.FindSuperviewOfType(stopAt, type);
            }

            return null;
        }

        public static void CenterImageAndTitle(this UIButton button)
        {
            var frame = button.ImageView.Frame;

            frame = new CGRect(NMath.Round((button.Bounds.Size.Width - frame.Size.Width) / 2), 10.0f, frame.Size.Width, frame.Size.Height);

            button.ImageView.Frame = frame;

            frame = button.TitleLabel.Frame;

            frame = new CGRect(NMath.Round((button.Bounds.Size.Width - frame.Size.Width) / 2), button.Bounds.Size.Height - frame.Size.Height - 5.0, frame.Size.Width, frame.Size.Height);

            button.TitleLabel.Frame = frame;
        }

        public static void CenterVerticallyWithPadding(this UIButton button, nfloat padding)
        {
            var imageSize = button.ImageView.Frame.Size;
            var titleSize = button.TitleLabel.Frame.Size;

            var totalHeight = (imageSize.Height + titleSize.Height + padding);

            button.ImageEdgeInsets = new UIEdgeInsets(-(totalHeight - imageSize.Height),
                                                0.0f,
                                                0.0f,
                                                -titleSize.Width);

            button.TitleEdgeInsets = new UIEdgeInsets(0.0f, -imageSize.Width, -(totalHeight - titleSize.Height), 0.0f);
        }

        public static void DisposeEx(this UIView view)
        {
            const bool enableLogging = true;
            try
            {
                if (view.IsDisposedOrNull())
                    return;

                var viewDescription = string.Empty;

                if (enableLogging)
                {
                    viewDescription = view.Description;
                    Debug.WriteLine("Destroying " + viewDescription);
                }

                var disposeView = true;
                var disconnectFromSuperView = true;
                var disposeSubviews = true;
                var removeGestureRecognizers = false; // WARNING: enable at your own risk, may causes crashes
                var removeConstraints = true;
                var removeLayerAnimations = true;
                var associatedViewsToDispose = new List<UIView>();
                var otherDisposables = new List<IDisposable>();

                if (view is UIActivityIndicatorView)
                {
                    var aiv = (UIActivityIndicatorView)view;
                    if (aiv.IsAnimating)
                    {
                        aiv.StopAnimating();
                    }
                }
                else if (view is UITableView)
                {
                    var tableView = (UITableView)view;

                    if (tableView.DataSource != null)
                    {
                        otherDisposables.Add(tableView.DataSource);
                    }
                    if (tableView.BackgroundView != null)
                    {
                        associatedViewsToDispose.Add(tableView.BackgroundView);
                    }

                    tableView.Source = null;
                    tableView.Delegate = null;
                    tableView.DataSource = null;
                    tableView.WeakDelegate = null;
                    tableView.WeakDataSource = null;
                    associatedViewsToDispose.AddRange(tableView.VisibleCells ?? new UITableViewCell[0]);
                }
                else if (view is UITableViewCell)
                {
                    var tableViewCell = (UITableViewCell)view;
                    disposeView = false;
                    disconnectFromSuperView = false;
                    if (tableViewCell.ImageView != null)
                    {
                        associatedViewsToDispose.Add(tableViewCell.ImageView);
                    }
                }
                else if (view is UICollectionView)
                {
                    var collectionView = (UICollectionView)view;
                    disposeView = false;
                    if (collectionView.DataSource != null)
                    {
                        otherDisposables.Add(collectionView.DataSource);
                    }
                    if (!collectionView.BackgroundView.IsDisposedOrNull())
                    {
                        associatedViewsToDispose.Add(collectionView.BackgroundView);
                    }
                    //associatedViewsToDispose.AddRange(collectionView.VisibleCells ?? new UICollectionViewCell[0]);
                    collectionView.Source = null;
                    collectionView.Delegate = null;
                    collectionView.DataSource = null;
                    collectionView.WeakDelegate = null;
                    collectionView.WeakDataSource = null;
                }
                else if (view is UICollectionViewCell)
                {
                    var collectionViewCell = (UICollectionViewCell)view;
                    disposeView = false;
                    disconnectFromSuperView = false;
                    if (collectionViewCell.BackgroundView != null)
                    {
                        associatedViewsToDispose.Add(collectionViewCell.BackgroundView);
                    }
                }
                else if (view is UIWebView)
                {
                    var webView = (UIWebView)view;
                    if (webView.IsLoading)
                        webView.StopLoading();
                    webView.LoadHtmlString(string.Empty, null); // clear display
                    webView.Delegate = null;
                    webView.WeakDelegate = null;
                }
                else if (view is UIImageView)
                {
                    var imageView = (UIImageView)view;
                    if (imageView.Image != null)
                    {
                        otherDisposables.Add(imageView.Image);
                        imageView.Image = null;
                    }
                }
                else if (view is UIScrollView)
                {
                    var scrollView = (UIScrollView)view;
                    //scrollView.UnsetZoomableContentView();
                }

                var gestures = view.GestureRecognizers;
                if (removeGestureRecognizers && gestures != null)
                {
                    foreach (var gr in gestures)
                    {
                        view.RemoveGestureRecognizer(gr);
                        gr.Dispose();
                    }
                }

                if (removeLayerAnimations && view.Layer != null)
                {
                    view.Layer.RemoveAllAnimations();
                }

                if (disconnectFromSuperView && view.Superview != null)
                {
                    view.RemoveFromSuperview();
                }

                var constraints = view.Constraints;
                if (constraints != null && constraints.Any() && constraints.All(c => c.Handle != IntPtr.Zero))
                {
                    view.RemoveConstraints(constraints);
                    foreach (var constraint in constraints)
                    {
                        constraint.Dispose();
                    }
                }

                foreach (var otherDisposable in otherDisposables)
                {
                    otherDisposable.Dispose();
                }

                foreach (var otherView in associatedViewsToDispose)
                {
                    otherView.DisposeEx();
                }

                var subViews = view.Subviews;
                if (disposeSubviews && subViews != null)
                {
                    foreach (var subView in subViews)
                    {
                        subView.DisposeEx();
                    }
                }

                if (view is ISpecialDisposable)
                {
                    ((ISpecialDisposable)view).SpecialDispose();
                }
                else if (disposeView)
                {
                    if (view.Handle != IntPtr.Zero)
                        view.Dispose();
                }

                if (enableLogging)
                {
                    Debug.WriteLine("Destroyed {0}", viewDescription);
                }

            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        public static void RemoveAndDisposeChildSubViews(this UIView view)
        {
            if (view == null)
                return;
            if (view.Handle == IntPtr.Zero)
                return;
            if (view.Subviews == null)
                return;

            foreach (var subview in view.Subviews)
            {
                subview.RemoveFromSuperviewAndDispose();
            }
        }

        public static void RemoveFromSuperviewAndDispose(this UIView view)
        {
            view.RemoveFromSuperview();
            view.DisposeEx();
        }

        public static bool IsDisposedOrNull(this UIView view)
        {
            if (view == null)
                return true;

            if (view.Handle == IntPtr.Zero)
                return true; ;

            return false;
        }

        public interface ISpecialDisposable
        {
            void SpecialDispose();
        }
    }

    public static class UIColorExtensions
    {
        public static UIColor FromHexString(this UIColor color, string hexValue, float alpha = 1.0f)
        {
            var colorString = hexValue.Replace("#", "");
            if (alpha > 1.0f)
            {
                alpha = 1.0f;
            }
            else if (alpha < 0.0f)
            {
                alpha = 0.0f;
            }

            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                    {
                        red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                        green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                        blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                        return UIColor.FromRGBA(red, green, blue, alpha);
                    }
                case 6: // #RRGGBB
                    {
                        red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                        green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        return UIColor.FromRGBA(red, green, blue, alpha);
                    }

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexValue));

            }
        }

        public static string HexStringForColor(this UIColor color)
        {
            var components = color.CGColor.Components;

            var r = components[0];
            var g = components[1];
            var b = components[2];

            var red = (int)(r * 255);
            var green = (int)(g * 255);
            var blue = (int)(b * 255);

            var hexString = string.Format("{0}{1}{2}", red.ToString("X2"), green.ToString("X"), blue.ToString("X"));

            return hexString;
        }
    }
}
