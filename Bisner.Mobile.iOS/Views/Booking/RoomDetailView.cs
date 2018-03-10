using System;
using System.Collections.Specialized;
using System.Linq;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.MvvmcrossApp;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using Bisner.Mobile.iOS.Views.Layout;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [MvxFromStoryboard]
    public partial class RoomDetailView : ViewBase<RoomDetailViewModel>
    {
        public RoomDetailView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Set body slider (carousel))
            var bodySliderSource = new BodySliderCollectionViewSource(clvBodySlider, true);

            var set = this.CreateBindingSet<RoomDetailView, RoomDetailViewModel>();
            set.Bind(lblTitle).To(vm => vm.Title);
            set.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
            set.Bind(vwStatus).For("AvailableBackground").To(vm => vm.IsAvailable);
            set.Bind(bodySliderSource).For(c => c.ItemsSource).To(vm => vm.RoomList);
            set.Bind(btnCheck).To(vm => vm.CheckBtnClickedCommand);
            set.Bind(lblAboutTitle1).To(vm => vm.AboutLocationTitle);
            set.Bind(lblAboutTitle1).For("Visibility").To(vm => vm.AboutLocationVisible).WithConversion("Visibility");
            set.Bind(lblAboutDesc1).For(l => l.AttributedText).To(vm => vm.AboutLocationText).WithConversion("HtmlAttributedText");
            set.Bind(lblAboutDesc1).For("Visibility").To(vm => vm.AboutLocationVisible).WithConversion("Visibility");
            set.Bind(lblAboutTitle2).To(vm => vm.AboutRoomTitle);
            set.Bind(lblAboutTitle2).For("Visibility").To(vm => vm.AboutLocationVisible).WithConversion("Visibility");
            set.Bind(lblAboutDesc2).For(l => l.AttributedText).To(vm => vm.AboutRoomText).WithConversion("HtmlAttributedText");
            set.Bind(lblAboutDesc2).For("Visibility").To(vm => vm.AboutLocationVisible).WithConversion("Visibility");
            set.Bind(btnPersons).For("Title").To(vm => vm.NumberOfPersons);
            set.Bind(btnLocationName).For("Title").To(vm => vm.LocationName);
            set.Apply();

            clvBodySlider.SetCollectionViewLayout(new LineLayoutForBodySlider(), true);
            clvBodySlider.Delegate = new UICollectionViewDelegateFlowLayout();
            clvBodySlider.Source = bodySliderSource;
            clvBodySlider.ReloadData();

            // Collection changed is not fired when no images are in the list, so we set the constraint to 0 on load
            ImageContainerHeight.Constant = 0;

            InitStyle();
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = true;

            if (IsMovingToParentViewController)
            {
                btnBack.TouchUpInside += BtnBackOnTouchUpInside;
                ViewModel.RoomList.CollectionChanged += RoomListOnCollectionChanged;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                btnBack.TouchUpInside -= BtnBackOnTouchUpInside;
                ViewModel.RoomList.CollectionChanged -= RoomListOnCollectionChanged;

                Mvx.Resolve<IMainViewPresenter>().ShowInTab = true;
            }
        }

        public void InitStyle()
        {
            clvBodySlider.ShowsHorizontalScrollIndicator = false;
            btnCheck.Layer.CornerRadius = btnCheck.Frame.Height / 2f;
            btnCheck.Layer.MasksToBounds = true;
            btnCheck.Font = Appearance.Fonts.LatoBoldWithSize(16);
            lblTitle.Font = Appearance.Fonts.LatoBlackWithSize(28);
            lblAboutDesc1.Font = Appearance.Fonts.LatoWithSize(14);
            lblAboutDesc2.Font = Appearance.Fonts.LatoWithSize(14);
            lblAboutTitle1.Font = Appearance.Fonts.LatoBoldWithSize(14);
            lblAboutTitle2.Font = Appearance.Fonts.LatoBoldWithSize(14);
            //btnPersons.Font = Appearance.Fonts.LatoWithSize(14);
            //btnLocationName.Font = Appearance.Fonts.LatoWithSize(14);

            ivContent.Transparency = 0.5f;
            ivContent.ContentMode = UIViewContentMode.ScaleAspectFill;
            ScrollView.BackgroundColor = Appearance.Colors.BackgroundColor;
        }

        private void BtnBackOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            NavigationController.PopViewController(true);
        }

        private void RoomListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ImageContainerHeight.Constant = ViewModel.RoomList.Any() ? 117 : 0;
        }
    }
}