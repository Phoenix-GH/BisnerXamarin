using Android.App;
using Android.OS;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Droid.Views.Base;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Activity(NoHistory = false)]
    public class EventsView : BaseToolbarActivity<EventsViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        public EventsView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Fragment

        protected override int LayoutId => Resource.Layout.events_fragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        protected override string ScreenName => "EventsView";

        #endregion Fragment
    }
}