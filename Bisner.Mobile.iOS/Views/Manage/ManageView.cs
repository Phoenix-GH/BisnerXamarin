using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Manage.Cells;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Manage
{
    partial class ManageView : ViewBase<ManageViewModel>
    {
        #region Constructor

        private GenericTableViewSource<IManageItem> _source;

        public ManageView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("MANAGEVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupBindings();

            // Set screen name for analytics
            ScreenName = "ManageView";
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _source = new GenericTableViewSource<IManageItem>(ItemTable)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    NSString identifier;

                    if (item is ManageUser)
                    {
                        identifier = ManageUserCell.Identifier;
                    }
                    else if (item is ManageLabel)
                    {
                        identifier = ManageLabelCell.Identifier;
                    }
                    else
                    {
                        identifier = ManageItemCell.Identifier;
                    }

                    return identifier;
                },
                RowSelectionFunc = (view, path, item) =>
                {
                    ViewModel.ItemSelected(item);
                },
                ModifyCellFunc = (cell, path, arg3) =>
                {
                    if (cell is ManageItemCell && ItemTable.CheckIfLastRow(path))
                    {
                        ((ManageItemCell)cell).SetBottomRuler(true);
                    }
                    else if (cell is ManageItemCell)
                    {
                        ((ManageItemCell)cell).SetBottomRuler(false);
                    }
                },
                AutoDeselect = true,
            };

            ItemTable.RegisterClassForCellReuse(typeof(ManageUserCell), ManageUserCell.Identifier);
            ItemTable.RegisterClassForCellReuse(typeof(ManageLabelCell), ManageLabelCell.Identifier);
            ItemTable.RegisterClassForCellReuse(typeof(ManageItemCell), ManageItemCell.Identifier);

            ItemTable.Bounces = true;
            ItemTable.ContentInset = new UIEdgeInsets(5, 0, 0, 0);
            ItemTable.BackgroundColor = Appearance.Colors.BackgroundColor;
            ItemTable.EstimatedRowHeight = 50;
            ItemTable.RowHeight = UITableView.AutomaticDimension;
            ItemTable.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ManageView, ManageViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Apply();
        }

        #endregion Setup

        #region Base modifications

        protected override bool EnableTitleBarLogo => true;

        protected override bool EnableCustomBackButton => ViewModel.Type != ManageType.Default;

        #endregion Base modifications
    }
}
