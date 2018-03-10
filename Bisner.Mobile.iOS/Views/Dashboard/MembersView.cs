using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Company;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.ViewModels.Members;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Dashboard.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    partial class MembersView : ViewBase<MembersViewModel>
    {
        #region Constructor

        private GenericTableViewSource<IItemBase> _membersSource;
        private MvxUIRefreshControl _refreshControl;

        public MembersView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("MEMBERSVIEW RECIEVED MEMORY WARNING!!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupBindings();

            View.BackgroundColor = Appearance.Colors.BackgroundColor;

            // Set screen name for analytics
            ScreenName = "MembersView";
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _membersSource = new GenericTableviewSourceWithHeight<IItemBase>(ItemsTable)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    if (item is MemberSearchItem)
                    {
                        return MembersSearchCell.Identifier;
                    }

                    if (item is MembersButtonItem)
                    {
                        return MembersButtonCell.Identifier;
                    }

                    if (item is ICompany)
                    {
                        return MembersCompanyCell.Identifier;
                    }

                    return MemberCell.Identifier;
                },
                RowSelectionFunc = (view, path, baseItem) =>
                {
                    ViewModel.MemberSelected(baseItem);
                },
                GetCellHeightFunc = (view, path, item) =>
                {
                    if (item is MembersButtonItem)
                    {
                        Debug.WriteLine("BUTTON ITEM AT ROW : {0}", path.Row);
                    }

                    if (item is IUser || item is ICompany)
                    {
                        return 68;
                    }

                    return 62;
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    if (item is IUser || item is ICompany)
                    {
                        return 68;
                    }

                    return 62;
                },
                //ViewToMove = SearchContainer,
                //ConstraintToChange = SearchContainerTopConstraint,
                UseAnimations = true,
                AddAnimation = UITableViewRowAnimation.Left,
                RemoveAnimation = UITableViewRowAnimation.Right,
                ReplaceAnimation = UITableViewRowAnimation.Middle,
                AutoDeselect = true,
            };

            ItemsTable.BackgroundColor = Appearance.Colors.BackgroundColor;
            ItemsTable.RegisterClassForCellReuse(typeof(MembersCompanyCell), MembersCompanyCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(MembersButtonCell), MembersButtonCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(MembersSearchCell), MembersSearchCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(MemberCell), MemberCell.Identifier);
            ItemsTable.Source = _membersSource;

            _refreshControl = new MvxUIRefreshControl();
            ItemsTable.AddSubviews(_refreshControl);
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<MembersView, MembersViewModel>();
            set.Bind(_membersSource).To(vm => vm.Members);
            set.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            //set.Bind(SearchInput).For(i => i.Placeholder).To(vm => vm.SearchPlaceHolder);
            //set.Bind(SearchInput).To(vm => vm.SearchInput);
            set.Apply();

            ItemsTable.ReloadData();
        }

        #endregion Setup

        #region Base modifications

        protected override bool EnableCustomBackButton => true;

        protected override bool EnableTitleBarLogo => true;

        #endregion Base modifications
    }
}
