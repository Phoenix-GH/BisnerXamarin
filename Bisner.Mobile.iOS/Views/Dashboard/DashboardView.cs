using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using Bisner.Mobile.iOS.Views.Layout;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    [MvxFromStoryboard]
    partial class DashboardView : ViewBase<DashboardViewModel>, IUIScrollViewDelegate
    {
        #region Constructor

        public DashboardView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            scvMain.Delegate = this;
            txfSearch.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            // Set refresh control
            var refreshControl = new MvxUIRefreshControl();
            scvMain.AddSubview(refreshControl);

            //Set Header Slider
            var headerSliderSource = new HeaderSliderCollectionViewSource(this.clvHeaderSlider);
            clvHeaderSlider.SetCollectionViewLayout(new LineLayoutForHaderSlider(), true);
            clvHeaderSlider.Delegate = new UICollectionViewDelegateFlowLayout();
            clvHeaderSlider.Source = headerSliderSource;
            clvHeaderSlider.ReloadData();

            //Set body slider (carousel)
            var bodySliderSource = new BodySliderCollectionViewSource(clvBodySlider);
            clvBodySlider.SetCollectionViewLayout(new LineLayoutForBodySlider(), true);
            clvBodySlider.Delegate = new UICollectionViewDelegateFlowLayout();
            clvBodySlider.Source = bodySliderSource;
            clvBodySlider.ReloadData();

            var routerTableViewSource = new GenericTableviewSourceWithHeight<CommonItemViewModel>(tvRouter)
            {
                GetIdentifierFunc = (model, path) => CommonItemView.Identifier,
                RowSelectionFunc = (view, path, arg3) =>
                {
                    ViewModel.SelectItem(arg3);
                },
                GetCellHeightFunc = (view, path, item) => 50,
                GetEstimatedHeightFunc = (view, path, item) => 50,
                AutoDeselect = true
            };
            tvRouter.RegisterNibForCellReuse(UINib.FromName("CommonItemView", null), CommonItemView.Identifier);
            tvRouter.Source = routerTableViewSource;
            tvRouter.ReloadData();

            InitStyle();

            var bindingSet = this.CreateBindingSet<DashboardView, DashboardViewModel>();
            bindingSet.Bind(bodySliderSource).For(t => t.ItemsSource).To(vm => vm.BodySliderList);
            bindingSet.Bind(headerSliderSource).For(t => t.ItemsSource).To(vm => vm.HeaderSliderList);
            bindingSet.Bind(routerTableViewSource).For(t => t.ItemsSource).To(vm => vm.RouterItems);
            bindingSet.Bind(btnShowMember).To(vm => vm.ShowMembersCommand);
            bindingSet.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            bindingSet.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            bindingSet.Apply();

            // Set screen name for analytics
            ScreenName = "DashboardView";
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("DASHBOARDVIEW RECIEVED MEMORY WARNING!!!!");
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = false;

            if (IsMovingToParentViewController)
            {
                ViewModel.RouterItems.CollectionChanged += RouterItemsOnCollectionChanged;
                ViewModel.HeaderSliderList.CollectionChanged += HeaderSliderListOnCollectionChanged;
                ViewModel.BodySliderList.CollectionChanged += BodySliderListOnCollectionChanged;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                ViewModel.RouterItems.CollectionChanged -= RouterItemsOnCollectionChanged;
                ViewModel.HeaderSliderList.CollectionChanged -= HeaderSliderListOnCollectionChanged;
                ViewModel.BodySliderList.CollectionChanged -= BodySliderListOnCollectionChanged;
            }
        }

        #endregion ViewController

        #region Setup

        public void InitStyle()
        {
            clvHeaderSlider.ShowsHorizontalScrollIndicator = false;
            clvBodySlider.ShowsHorizontalScrollIndicator = false;
            cntSearchbarHeight.Constant = 0f;

            tvRouter.ScrollEnabled = false;

            HeaderSliderHeight.Constant = 0;
            MembersHeightConstraint.Constant = 0;
            MembersBottomConstraint.Constant = 0;

            MembersContainer.Hidden = true;
            MembersContainer.Hidden = true;
            MembersTopBorder.Hidden = true;
            clvBodySlider.Hidden = true;
            MembersMiddleBorder.Hidden = true;
            ivShowAllMembers.Hidden = true;
            lblShowAllMembers.Hidden = true;
            MembersArrowImage.Hidden = true;
            btnShowMember.Hidden = true;

            btnShowMember.Font = Appearance.Fonts.LatoWithSize(14);
            ivShowAllMembers.BackgroundColor = UIColor.FromRGB(226, 226, 226);
            lblShowAllMembers.Font = Appearance.Fonts.LatoWithSize(14);
            txfSearch.Font = Appearance.Fonts.LatoWithSize(14);

            View.LayoutIfNeeded();
        }

        [Export("scrollViewDidEndDragging:willDecelerate:")]
        public void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            // TODO : Search bar animation found here, turned off now
            return;

            if (scrollView.ContentOffset.Y < -60f)
            {
                UIView.BeginAnimations("SearchBarShowAnimatoin");
                UIView.SetAnimationDuration(0.3f);
                cntSearchbarHeight.Constant = 44f;
                View.LayoutIfNeeded();
                UIView.CommitAnimations();
            }
            else
            {
                UIView.BeginAnimations("SearchBarHideAnimatoin");
                UIView.SetAnimationDuration(0.3f);
                cntSearchbarHeight.Constant = 0f;
                View.LayoutIfNeeded();
                UIView.CommitAnimations();
            }
        }

        #endregion Setup

        #region ViewModel listeners

        private void RouterItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            tableHeight.Constant = 50 * ViewModel.RouterItems.Count;
        }

        private void HeaderSliderListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            HeaderSliderHeight.Constant = ViewModel.HeaderSliderList.Any() ? 276 : 0;
        }

        private void BodySliderListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var hidden = !ViewModel.BodySliderList.Any();

            MembersContainer.Hidden = hidden;
            MembersContainer.Hidden = hidden;
            MembersTopBorder.Hidden = hidden;
            clvBodySlider.Hidden = hidden;
            MembersMiddleBorder.Hidden = hidden;
            ivShowAllMembers.Hidden = hidden;
            lblShowAllMembers.Hidden = hidden;
            MembersArrowImage.Hidden = hidden;
            btnShowMember.Hidden = hidden;

            MembersHeightConstraint.Constant = ViewModel.BodySliderList.Any() ? 170 : 0;
            MembersBottomConstraint.Constant = ViewModel.BodySliderList.Any() ? 8 : 0;
        }
        
        #endregion ViewModel listeners

        #region Base modifications

        protected override bool EnableTitleBarLogo => true;

        #endregion Base modifications
    }
}
