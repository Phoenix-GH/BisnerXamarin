using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.iOS.Views.Base;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using SDWebImage;
using UIKit;

namespace Bisner.Mobile.iOS.Views
{
    partial class ImageZoomView : ViewBase<ImageZoomViewModel>, IMvxModalIosView, IUIScrollViewDelegate
    {
        #region Constructor

        private MvxImageView _imageView;

        public ImageZoomView(IntPtr handle) : base(handle)
        {

        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("IMAGEZOOMVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();
            SetupBindings();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Loader.StartAnimating();
            Loader.Hidden = false;

            //CloseButton.TouchUpInside += CloseView;

            // Set screen name for analytics
            ScreenName = "ImageZoomView";
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //CloseButton.TouchUpInside -= CloseView;
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        #endregion ViewController

        #region Setup

        private void SetupView()
        {
            View.BackgroundColor = UIColor.Black;

            // Set the navigation bar hidden so we don't get a double nav bar
            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            ScrollView.Delegate = this;

            CloseButton.SetTitle("Close", UIControlState.Normal);
            CloseButton.SetTitleColor(UIColor.White, UIControlState.Normal);

            _imageView = new MvxImageView();
            _imageView.SetImage(new NSUrl(Settings.BlobUrl + ViewModel.ImageUrl), null, (image, error, cacheType, finished) =>
            {
                if (image == null)
                {
                    return;
                }

                if (image.Size.Width == 0 || image.Size.Height == 0)
                {
                    Debug.WriteLine("ERROR : Image size is 0, trying to load again");
                    return;
                }

                _imageView.Frame = new CGRect(0, 0, image.Size.Width, image.Size.Height);

                ScrollView.AddSubviews(_imageView);

                ScrollView.ContentSize = _imageView.Frame.Size;

                SetupScrollViewScales();
                SetupScrollViewGestures();

                Loader.Hidden = true;
            });
        }

        private void SetupScrollViewScales()
        {
            var scrollViewFrame = ScrollView.Frame;
            var scaleWidth = scrollViewFrame.Size.Width / ScrollView.ContentSize.Width;
            var scaleHeight = scrollViewFrame.Size.Height / ScrollView.ContentSize.Height;
            var minscale = NMath.Min(scaleWidth, scaleHeight);
            ScrollView.MinimumZoomScale = minscale;
            ScrollView.MaximumZoomScale = 1.0f;
            ScrollView.ZoomScale = minscale;

            //_imageView.Center = ScrollView.Center;
            CenterScrollViewContents();
        }

        private void SetupScrollViewGestures()
        {
            var doubleTapRecognizer = new UITapGestureRecognizer(DoubleTapped) { NumberOfTapsRequired = 2, NumberOfTouchesRequired = 1 };
            var twoFingerTapRecognizer = new UITapGestureRecognizer(TwoFingerTapped) { NumberOfTapsRequired = 1, NumberOfTouchesRequired = 2 };

            ScrollView.AddGestureRecognizer(doubleTapRecognizer);
            ScrollView.AddGestureRecognizer(twoFingerTapRecognizer);
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ImageZoomView, ImageZoomViewModel>();
            set.Bind(CloseButton).To(vm => vm.CloseCommand);
            set.Apply();
        }

        #endregion Setup

        #region ScrollView

        [Export("viewForZoomingInScrollView:")]
        public virtual UIView ViewForZoomingInScrollView(UIScrollView scrollView)
        {
            return _imageView;
        }

        [Export("scrollViewDidZoom:")]
        public virtual void DidZoom(UIScrollView scrollView)
        {
            CenterScrollViewContents();
        }

        #endregion ScrollView

        #region Handlers

        private void CenterScrollViewContents()
        {
            var boundsSize = ScrollView.Bounds.Size;
            var contentsFrame = _imageView.Frame;

            if (contentsFrame.Size.Width < boundsSize.Width)
            {
                contentsFrame.X = (boundsSize.Width - contentsFrame.Size.Width) / 2.0f;
            }
            else
            {
                contentsFrame.X = 0.0f;
            }

            if (contentsFrame.Size.Height < boundsSize.Height)
            {
                contentsFrame.Y = (boundsSize.Height - contentsFrame.Size.Height) / 2.0f;
            }
            else
            {
                contentsFrame.Y = 0.0f;
            }

            _imageView.Frame = contentsFrame;
        }

        private void DoubleTapped(UITapGestureRecognizer recognizer)
        {
            var pointInView = recognizer.LocationInView(_imageView);

            var newZoomScale = ScrollView.ZoomScale * 1.5f;
            newZoomScale = NMath.Min(newZoomScale, ScrollView.MaximumZoomScale);

            var scrollViewSize = ScrollView.Bounds.Size;

            var w = scrollViewSize.Width / newZoomScale;
            var h = scrollViewSize.Height / newZoomScale;
            var x = pointInView.X - w / 2.0f;
            var y = pointInView.Y - h / 2.0f;

            var rectToZoomTo = new CGRect(x, y, w, h);

            ScrollView.ZoomToRect(rectToZoomTo, true);
        }

        private void TwoFingerTapped()
        {
            // Zoom out slightly, capping at the minimum zoom scale specified by the scroll view
            var newZoomScale = ScrollView.ZoomScale / 1.5f;
            newZoomScale = NMath.Max(newZoomScale, ScrollView.MinimumZoomScale);
            ScrollView.SetZoomScale(newZoomScale, true);
        }

        #endregion Handlers

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _imageView = null;
            }
        }

        #endregion Dispose
    }
}
