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
    public partial class BookingConfirmedView : HideTabBarViewBase<BookingConfirmedViewModel>
    {
        #region Constructor

        public BookingConfirmedView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            txfTitle.ShouldReturn = ShouldReturn;

            //Set body slider (carousel))
            var bodySliderSource = new BodySliderCollectionViewSource(clvBodySlider, true);

            InitStyle();
            
            var bindingSet = this.CreateBindingSet<BookingConfirmedView, BookingConfirmedViewModel>();
            bindingSet.Bind(ivContent).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
            bindingSet.Bind(lblReservationCode).To(vm => vm.ReservationCode);
            bindingSet.Bind(lblDate).To(vm => vm.Date);
            bindingSet.Bind(lblCheckin).To(vm => vm.Checkin);
            bindingSet.Bind(lblCheckout).To(vm => vm.Checkout);
            bindingSet.Bind(lblRoom).To(vm => vm.Room);
            bindingSet.Bind(btnMore).To(vm => vm.MoreCommand);
            bindingSet.Bind(btnSave).To(vm => vm.SaveCommand);
            bindingSet.Bind(lblLocation).To(vm => vm.Location);
            bindingSet.Bind(txvNote).To(vm => vm.Message);
            bindingSet.Bind(bodySliderSource).For(s => s.ItemsSource).To(vm => vm.RoomList);
            bindingSet.Bind(txfTitle).To(vm => vm.Title);
            bindingSet.Bind(swPrivateMeeting).To(vm => vm.IsPrivate);
            bindingSet.Apply();

            clvBodySlider.SetCollectionViewLayout(new LineLayoutForBodySlider(), true);
            clvBodySlider.Delegate = new UICollectionViewDelegateFlowLayout();
            clvBodySlider.Source = bodySliderSource;
            clvBodySlider.ReloadData();

            PositionToScrollView = UITableViewScrollPosition.Bottom;
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            RoomImagesHeightConstraint.Constant = 0;
            RoomImagesBottomConstraint.Constant = 0;
            
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
            }
        }

        #endregion ViewController

        #region Setup

        public void InitStyle()
        {
            clvBodySlider.ShowsHorizontalScrollIndicator = false;
            btnSave.Layer.CornerRadius = btnSave.Frame.Height / 2f;
            btnSave.Layer.MasksToBounds = true;
            btnGoogleCalendar.Layer.CornerRadius = btnGoogleCalendar.Frame.Height / 2f;
            btnGoogleCalendar.Layer.MasksToBounds = true;
            btnGoogleCalendar.Layer.BorderWidth = 1;
            btnGoogleCalendar.Layer.BorderColor = UIColor.FromRGB(0, 122, 255).CGColor;
            btnICal.Layer.CornerRadius = btnICal.Frame.Height / 2f;
            btnICal.Layer.MasksToBounds = true;
            btnICal.Layer.BorderWidth = 1;
            btnICal.Layer.BorderColor = UIColor.FromRGB(0, 122, 255).CGColor;


            btnGoogleCalendar.Font = Appearance.Fonts.LatoWithSize(16);
            btnICal.Font = Appearance.Fonts.LatoWithSize(16);
            btnSave.Font = Appearance.Fonts.LatoWithSize(16);
            lblBookingConfirmed.Font = Appearance.Fonts.LatoBlackWithSize(24);
            lblCheckin.Font = Appearance.Fonts.LatoWithSize(14);
            lblCheckinTitle.Font = Appearance.Fonts.LatoWithSize(14);
            lblCheckout.Font = Appearance.Fonts.LatoWithSize(14);
            lblCheckoutTitle.Font = Appearance.Fonts.LatoWithSize(14);
            lblConfirmedText.Font = Appearance.Fonts.LatoWithSize(14);
            lblDate.Font = Appearance.Fonts.LatoWithSize(14);
            lblDateTitle.Font = Appearance.Fonts.LatoWithSize(14);
            lblDetailsTitle.Font = Appearance.Fonts.LatoWithSize(14);
            lblLocation.Font = Appearance.Fonts.LatoWithSize(14);
            lblLocationTitle.Font = Appearance.Fonts.LatoWithSize(14);
            lblMeetingDetails.Font = Appearance.Fonts.LatoWithSize(14);
            lblNotePlaceholder.Font = Appearance.Fonts.LatoWithSize(14);
            lblPrivateMeeting.Font = Appearance.Fonts.LatoWithSize(14);
            lblReservationCode.Font = Appearance.Fonts.LatoWithSize(14);
            lblRoom.Font = Appearance.Fonts.LatoWithSize(14);
            lblRoomTitle.Font = Appearance.Fonts.LatoWithSize(14);
            lblSaveMeetingTitle.Font = Appearance.Fonts.LatoWithSize(14);
            txfTitle.Font = Appearance.Fonts.LatoWithSize(14);
            txvNote.Font = Appearance.Fonts.LatoWithSize(14);

            ivContent.Transparency = 0.5f;
            ivContent.ContentMode = UIViewContentMode.ScaleAspectFill;
        }

        protected override bool EnableCustomBackButton => true;

        #endregion Setup

        #region Handlers

        private void BtnBackOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            NavigationController.PopViewController(true);
        }

        private void RoomListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (ViewModel.RoomList.Any())
            {
                RoomImagesHeightConstraint.Constant = 128;
                RoomImagesBottomConstraint.Constant = 8;
            }
            else
            {
                RoomImagesHeightConstraint.Constant = 0;
                RoomImagesBottomConstraint.Constant = 0;
            }
            clvBodySlider.ReloadData();
        }

        /// <summary>
        /// Textfield handler to resign the keyboard
        /// </summary>
        /// <param name="textField"></param>
        /// <returns></returns>
        private bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();

            return true;
        }

        #endregion Handlers

        #region Tabbar & keyboard

        protected override bool SupportsHideTabBar => Mvx.Resolve<IMainViewPresenter>().ShowInTab;

        protected override NSLayoutConstraint TabBarTopConstraint => BottomConstraint;

        public override bool HandlesKeyboardNotifications => true;

        protected override nfloat KeyboardScrollExtraOffset => 5;

        #endregion Tabbar & keyboard
    }
}