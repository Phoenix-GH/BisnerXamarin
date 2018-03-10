using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Droid.Controls.Adapters;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Activity(NoHistory = false, WindowSoftInputMode = SoftInput.StateHidden)]
    public class EventView : BaseToolbarActivity<EventViewModel>, View.IOnLayoutChangeListener
    {
        #region Variables

        private MvxRecyclerView _recyclerView;

        #endregion Variables

        #region Constructor

        public EventView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Fragment

        protected override int LayoutId => Resource.Layout.event_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetupRecyclerView();

            // Scroll to bottom
            ViewModel.Commented = () => { _recyclerView.ScrollToPosition(ViewModel.Items.Count - 1); };

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            if (Settings.UserRoles.All(r => r != Event.EventComment))
            {
                var inputBox = FindViewById<LinearLayout>(Resource.Id.event_input_container);

                if (inputBox != null)
                {
                    var layoutParams = inputBox.LayoutParameters;
                    layoutParams.Height = 0;
                    inputBox.LayoutParameters = layoutParams;
                }
            }
        }

        protected override string ScreenName => "EventView id = " + (ViewModel.Event?.Id.ToString() ?? Guid.Empty.ToString());

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _recyclerView = null;

            ViewModel.Commented = null;
        }

        private void SetupRecyclerView()
        {
            _recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.event_recyclerview);

            if (_recyclerView == null) return;

            _recyclerView.AddOnLayoutChangeListener(this);

            _recyclerView.Adapter = new MvxGenericRecyclerAdapter<IItemBase>((MvxAndroidBindingContext)BindingContext)
            {
                ModifyViewFunc = view1 =>
                {
                    SetupTextView(view1);
                    SetupAttendeeRecyclerView(view1);
                }
            };
        }

        #endregion Fragment

        #region Adapter item modifications

        /// <summary>
        /// Setups the text view.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetupTextView(View view)
        {
            var textView = view.FindViewById<TextView>(Resource.Id.event_detail_about_text);

            if (textView != null)
            {
                // Clickable links
                textView.MovementMethod = LinkMovementMethod.Instance;
            }
        }

        private void SetupAttendeeRecyclerView(View view)
        {
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.event_detail_info_attendees);

            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;

                var layoutManager = new LinearLayoutManager(this, RecyclerView.Horizontal, false);

                recyclerView.SetLayoutManager(layoutManager);
            }
        }

        #endregion Adapter item modifiactions

        #region LayoutChangeListener

        public void OnLayoutChange(View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
        {
            // If the bottom changed scroll to bottom
            if (bottom < oldBottom)
            {
                _recyclerView.PostDelayed(() =>
                {
                    _recyclerView.SmoothScrollToPosition(ViewModel.Items?.Count ?? 0);
                }, 100);
            }
        }

        #endregion LayoutChangeListener
    }
}