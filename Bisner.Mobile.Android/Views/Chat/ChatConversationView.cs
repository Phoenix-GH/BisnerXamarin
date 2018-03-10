using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Droid.Extensions;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform;
using MvvmCross.Plugins.DownloadCache;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Droid.Views.Chat
{
    [Activity(NoHistory = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ChatConversationView : BaseToolbarActivity<ChatConversationViewModel>
    {
        #region Variables

        private MvxRecyclerView _recyclerView;
        private MvxSubscriptionToken _newMessageToken;

        #endregion Variables

        #region Constructor

        public ChatConversationView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Fragment

        protected override int LayoutId => Resource.Layout.chat_conversation_view;

        protected override string ScreenName => "ChatConversationView";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Intent.PrintExtras();

            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = ViewModel.Title;

            SetupToolbar();
            SetupRecyclerView();

            // Scroll to bottom message
            _newMessageToken = Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<ChatConversationAddMessage>(
                message =>
                {
                    _recyclerView.ScrollToPosition(ViewModel.Messages.Count - 1);
                });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Dispose and remove references for GC
            _newMessageToken.Dispose();
            _newMessageToken = null;
            _recyclerView = null;
        }

        #endregion Fragment

        #region Setup

        private void SetupToolbar()
        {
            var avatarImage = FindViewById<ImageView>(Resource.Id.toolbar_user_avatar);

            if (avatarImage != null)
            {
                var mvxImageCache = Mvx.Resolve<IMvxImageCache<Bitmap>>();

                mvxImageCache.RequestImage(Settings.BlobUrl + ViewModel.AvatarUrl).ContinueWith(task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted && task.Result != null)
                    {
                        avatarImage.SetImageBitmap(task.Result);
                    }
                });
            }
        }

        private void SetupRecyclerView()
        {
            _recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.chat_conversation_recycler_view);

            if (_recyclerView != null)
            {
                var layoutManager = new LinearLayoutManager(this) { StackFromEnd = true };
                _recyclerView.SetLayoutManager(layoutManager);
            }
        }

        #endregion Setup
    }
}