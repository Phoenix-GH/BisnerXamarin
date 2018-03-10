using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [MvxFromStoryboard]
    public partial class TimePickerView : ViewBase<TimePickerViewModel>
    {
        public TimePickerView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var timePickerTableViewSource = new GenericTableviewSourceWithHeight<TimePickerItemViewModel>(tvTimePicker)
            {
                GetIdentifierFunc = (model, path) => TimePickerItemView.Identifier,
                GetCellHeightFunc = (view, path, item) => 80,
                ModifyCellFunc = (cell, path, item) =>
                {
                    var timePickerCell = cell as TimePickerItemView;
                    timePickerCell?.InitStyle();
                },
                AutoDeselect = true,
                RowSelectionFunc = (view, path, item) =>
                {
                    ViewModel.SelectItem(path.Row);
                }
            };

            tvTimePicker.Source = timePickerTableViewSource;
            tvTimePicker.ReloadData();

            var set = this.CreateBindingSet<TimePickerView, TimePickerViewModel>();
            set.Bind(timePickerTableViewSource).For(t => t.ItemsSource).To(vm => vm.TimeList);
            set.Bind(btnBack).To(vm => vm.BackBtnClickedCommand);
            set.Bind(btnClose).To(vm => vm.CloseBtnClickedCommand);
            set.Bind(btnBook).To(vm => vm.BookBtnClickedCommand);
            set.Apply();


            InitStyle();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.NavigationBarHidden = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.NavigationController.NavigationBarHidden = false;
        }

        public void InitStyle()
        {
            this.tvTimePicker.ShowsHorizontalScrollIndicator = false;
            this.btnBook.Layer.CornerRadius = this.btnBook.Frame.Height / 2f;
            this.btnBook.Layer.MasksToBounds = true;
        }
    }
}