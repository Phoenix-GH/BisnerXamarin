//using System;
//using System.Collections.Generic;
//using System.Linq;
//using MessageUI;
//using UIKit;

//namespace Bisner.Mobile.iOS.Helpers
//{
//    public interface ICanCleanUpMyself
//    {
//        void CleanUp();
//    }

//    public static class MemoryUtility
//    {
//        public static void ReleaseUIViewWithChildren(UIView view, bool enableSelfCleaning = true)
//        {
//            try
//            {
//                if (view == null)
//                    return;

//                if (view.Subviews != null)
//                {
//                    view.Subviews
//                        .ToList()
//                        .ForEach(subview =>
//                            ReleaseUIViewWithChildren(subview, enableSelfCleaning)
//                        );
//                }

//                ReleaseObject(view, enableSelfCleaning);
//            }
//            catch (Exception exception)
//            {
//                // Logging ...
//            }
//        }

//        public static void ReleaseObject(IDisposable disposableObject, bool enableSelfCleaning = true)
//        {
//            try
//            {
//                if (disposableObject == null)
//                    return;

//                ReleaseUIImageView(disposableObject);
//                ReleaseUIButton(disposableObject);
//                ReleaseUIView(disposableObject);
//                ReleaseUINavigationController(disposableObject);
//                ReleaseUIPageviewController(disposableObject);
//                ReleaseUIViewController(disposableObject);

//                if (enableSelfCleaning)
//                    LetObjectCleanUpItself(disposableObject);

//                disposableObject.Dispose();
//            }
//            catch (Exception exception)
//            {
//                // Logging ...
//            }
//        }

//        public static void ReleaseObject<T>(WeakReference<T> weakReference,
//            bool enableSelfCleaning = true) where T : class, IDisposable
//        {
//            T reference;
//            if (weakReference == null ||
//                !weakReference.TryGetTarget(out reference) || reference == null)
//                return;

//            ReleaseObject(reference, enableSelfCleaning);
//        }

//        public static void ReleaseUITableViewCell(IDisposable disposableObject)
//        {
//            try
//            {
//                var cell = disposableObject as UITableViewCell;

//                if (cell == null)
//                    return;

//                ReleaseUIViewWithChildren(cell.BackgroundView);
//                ReleaseUIViewWithChildren(cell.SelectedBackgroundView);
//                ReleaseUIViewWithChildren(cell.AccessoryView);
//                ReleaseUIViewWithChildren(cell.ContentView);
//            }
//            catch (Exception exception)
//            {
//                // Logging ...
//            }
//        }

//        public static void ReleaseSubviews(UIView[] subviews)
//        {
//            try
//            {
//                if (subviews == null || subviews.Length == 0)
//                    return;

//                foreach (UIView subview in subviews)
//                {
//                    if (subview is UIActivityIndicatorView)
//                        continue;

//                    if (subview != null)
//                        ReleaseSubviews(subview.Subviews);

//                    ReleaseObject(subview);
//                }
//            }
//            catch (Exception exception)
//            {
//                // Logging ...
//            }
//        }

//        private static void ReleaseUIButton(IDisposable disposableObject)
//        {
//            var button = disposableObject as UIButton;

//            if (button == null)
//                return;

//            if (button.ImageView != null)
//                ReleaseUIImageView(button.ImageView);

//            if (button.CurrentBackgroundImage != null)
//                button.CurrentBackgroundImage.Dispose();

//            if (button.CurrentImage != null)
//                button.CurrentImage.Dispose();
//        }

//        private static void ReleaseUIImageView(IDisposable disposableObject)
//        {
//            var imageView = disposableObject as UIImageView;

//            if (imageView == null || imageView.Image == null)
//                return;

//            imageView.Image.Dispose();
//            imageView.Image = null;
//        }

//        private static void ReleaseUINavigationController(IDisposable disposableObject)
//        {
//            var navigationController = disposableObject as UINavigationController;

//            if (navigationController == null)
//                return;

//            if (navigationController is MFMailComposeViewController)
//                return;

//            ReleaseChildControllers(navigationController.ChildViewControllers);
//        }

//        private static void ReleaseUIPageviewController(IDisposable disposableObject)
//        {
//            var pageViewController = disposableObject as UIPageViewController;

//            if (pageViewController == null)
//                return;

//            var children = pageViewController.ChildViewControllers
//                .Union(pageViewController.ViewControllers)
//                .ToList();

//            ReleaseChildControllers(children);

//            pageViewController.RemoveFromParentViewController();
//        }

//        private static void ReleaseChildControllers(IEnumerable<UIViewController> children)
//        {
//            foreach (UIViewController child in children)
//            {
//                var baseDialog = child as BaseDialog;

//                if (baseDialog != null)
//                {
//                    if (!baseDialog.IsDead)
//                    {
//                        baseDialog.DetachEventHandlers();
//                        baseDialog.CleanUpAfterDisappearing();
//                        baseDialog.CleanUp();

//                        child.RemoveFromParentViewController();
//                        child.Dispose();
//                    }

//                    continue;
//                }

//                var timelineDialog = child as TimelineDialog;

//                if (timelineDialog != null)
//                {
//                    timelineDialog.DetachEventHandlers();
//                    timelineDialog.CleanUp();

//                    timelineDialog.RemoveFromParentViewController();
//                    timelineDialog.Dispose();

//                    continue;
//                }

//                ReleaseUIViewWithChildren(child.View);
//                ReleaseObject(child);
//            }
//        }

//        private static void ReleaseUIViewController(IDisposable disposableObject)
//        {
//            var viewController = disposableObject as UIViewController;

//            if (viewController == null || viewController is UINavigationController || viewController is UIPageViewController)
//                return;

//            ReleaseUIViewWithChildren(viewController.View);

//            foreach (UIViewController child in viewController.ChildViewControllers)
//            {
//                ReleaseObject(child);
//            }

//            viewController.RemoveFromParentViewController();
//        }

//        private static void ReleaseUIView(IDisposable disposableObject)
//        {
//            var view = disposableObject as UIView;

//            if (view == null || view.Superview == null)
//                return;

//            view.RemoveFromSuperview();
//        }

//        private static void LetObjectCleanUpItself(IDisposable dispoableObject)
//        {
//            var selfCleaningObject = dispoableObject as ICanCleanUpMyself;

//            if (selfCleaningObject == null)
//                return;

//            selfCleaningObject.CleanUp();
//        }
//    }
//}