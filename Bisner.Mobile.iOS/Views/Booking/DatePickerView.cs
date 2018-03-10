using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.Views.Base;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Softweb.Xamarin.Controls.iOS;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [MvxFromStoryboard]
    public partial class DatePickerView : ViewBase<DatePickerViewModel>
    {
        private Calendar _calendar;

        public DatePickerView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Create required objects
            _calendar = new Calendar();
            //var menuView = new CalendarMenuView
            //{ Frame = new CGRect(0f, 0f, vwCalendar.Frame.Width, 0) };
            var contentView = new CalendarContentView { Frame = new CGRect(0f, 0f, vwCalendar.Frame.Width, vwCalendar.Frame.Height) };

            //Customize calendar's appearance
            var appearance = _calendar.CalendarAppearance;
            appearance.GetNSCalendar().FirstWeekDay = 2;
            appearance.DayCircleRatio = 9f / 10f;
            appearance.DayCircleColorToday = UIColor.FromRGB(255, 128, 0);
            appearance.DayCircleColorSelected = UIColor.FromRGB(40, 174, 219);
            appearance.DayCircleColorSelectedOtherMonth = UIColor.FromRGB(200, 200, 200);
            appearance.DayTextColorOtherMonth = UIColor.FromRGB(200, 200, 200);
            appearance.SetMonthLabelTextCallback((date, cal) => new NSString(((DateTime)date).ToString("MMMM")));

            _calendar.ContentView = contentView;

            //Add the views to the current view
            //vwCalendar.Add(menuView);
            vwCalendar.Add(contentView);

            UpdateMonthTitle();
            InitStyle();

            var bindingSet = this.CreateBindingSet<DatePickerView, DatePickerViewModel>();
            bindingSet.Bind(btnContinue).To(vm => vm.ContinueCommand);
            bindingSet.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = true;

            //Reload calendar
            _calendar.ReloadData();

            if (IsMovingToParentViewController)
            {
                _calendar.DateSelected += DateSelected;
                _calendar.NextPageLoaded += DidLoadNextPage;
                _calendar.PreviousPageLoaded += DidLoadPreviousPage;

                btnClose.TouchUpInside += BackButtonOnClicked;
                btnNextMonth.TouchUpInside += NextMonthBtnClicked;
                btnPrevMonth.TouchUpInside += PrevMonthBtnClicked;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                _calendar.DateSelected += DateSelected;
                _calendar.NextPageLoaded += DidLoadNextPage;
                _calendar.PreviousPageLoaded += DidLoadPreviousPage;

                btnClose.TouchUpInside -= BackButtonOnClicked;
                btnNextMonth.TouchUpInside -= NextMonthBtnClicked;
                btnPrevMonth.TouchUpInside -= PrevMonthBtnClicked;
            }
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public void InitStyle()
        {
            btnContinue.Layer.CornerRadius = btnContinue.Frame.Height / 2f;
            btnContinue.Layer.MasksToBounds = true;
            btnContinue.Font = Appearance.Fonts.LatoBoldWithSize(16);
        }

        public void DateSelected(object sender, DateSelectedEventArgs args)
        {
            var dateTime = ((DateTime)args.Date).ToLocalTime();

            Debug.WriteLine($"Selected date is {dateTime.ToString("dd-MMM-yyyy")}");
            ViewModel.SelectDate(dateTime);
        }

        public void DidLoadPreviousPage(object sender, EventArgs args)
        {
            Console.WriteLine("Loaded previous page");
            UpdateMonthTitle();

        }

        public void DidLoadNextPage(object sender, EventArgs args)
        {
            Console.WriteLine("Loaded next page");
            UpdateMonthTitle();
        }

        private void UpdateMonthTitle()
        {
            NSDateFormatter dateFormatter = new NSDateFormatter();
            dateFormatter.DateFormat = "MMMM";
            lblCurrentMonth.Text = dateFormatter.ToString(_calendar.CurrentDate);

            NSCalendar cal = NSCalendar.CurrentCalendar;
            NSDateComponents comps = cal.Components(NSCalendarUnit.Year | NSCalendarUnit.Month | NSCalendarUnit.Day, _calendar.CurrentDate);
            comps.Month += 1;
            var nextMonth = cal.DateFromComponents(comps);
            btnNextMonth.SetTitle(dateFormatter.ToString(nextMonth) + " >", UIControlState.Normal);
            comps.Month -= 2;
            var prevMonth = cal.DateFromComponents(comps);
            btnPrevMonth.SetTitle("< " + dateFormatter.ToString(prevMonth), UIControlState.Normal);
        }

        private void NextMonthBtnClicked(object sender, EventArgs args)
        {
            NSCalendar cal = NSCalendar.CurrentCalendar;
            NSDateComponents comps = cal.Components(NSCalendarUnit.Year | NSCalendarUnit.Month | NSCalendarUnit.Day, _calendar.CurrentDate);
            comps.Month += 1;
            _calendar.CurrentDate = cal.DateFromComponents(comps);
            UpdateMonthTitle();
        }

        private void PrevMonthBtnClicked(object sender, EventArgs args)
        {
            NSCalendar cal = NSCalendar.CurrentCalendar;
            NSDateComponents comps = cal.Components(NSCalendarUnit.Year | NSCalendarUnit.Month | NSCalendarUnit.Day, _calendar.CurrentDate);
            comps.Month -= 1;
            _calendar.CurrentDate = cal.DateFromComponents(comps);
            UpdateMonthTitle();
        }
    }
}