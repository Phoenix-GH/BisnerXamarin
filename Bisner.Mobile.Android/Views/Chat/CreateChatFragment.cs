using Android.App;
using Android.OS;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Chat
{
    [Activity(NoHistory = false)]
    public class CreateChatFragment : BaseToolbarActivity<CreateChatViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        public CreateChatFragment()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override int LayoutId => Resource.Layout.create_chat_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetupRecyclerView();
        }

        protected override string ScreenName => "CreateChatView";

        private void SetupRecyclerView()
        {
            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.create_chat_recycler_view);

            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
            }
        }

        #endregion Activity
    }
}