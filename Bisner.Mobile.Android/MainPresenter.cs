using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Content;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using Bisner.Mobile.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Presenter;
using Plugin.CurrentActivity;

namespace Bisner.Mobile.Droid
{
    public class MainPresenter : MvxFragmentsPresenter
    {
        #region Constructor

        public MainPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        #endregion Constructor

        #region Show

        protected override void ShowActivity(MvxViewModelRequest request, MvxViewModelRequest fragmentRequest = null)
        {
            if (request.ViewModelType == typeof(MainViewModel))
            {
                Activity.Finish();
            }

            if (request.ViewModelType == typeof(LoginViewModel))
            {
                Activity.Finish();
            }

            if (request.ViewModelType == typeof(UserViewModel))
            {
                Activity.OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
            }

            base.ShowActivity(request, fragmentRequest);
        }

        #endregion Show

        #region Presentation hints

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            CheckCreateChat(hint);

            //CheckDetail(hint);

            CheckLogout(hint);

            CheckLogin(hint);

            //CheckGroup(hint);

            //CheckEvent(hint);

            CheckChangeTab(hint);

            CheckChangeLanguage(hint);

            base.ChangePresentation(hint);
        }

        private void CheckCreateChat(MvxPresentationHint hint)
        {
            if (hint is ChatConversationHint)
            {
                Show(new MvxViewModelRequest(typeof(ChatConversationViewModel),
                   new MvxBundle(new Dictionary<string, string>
                   {
                        {"id", ((ChatConversationHint)hint).SelectedUser.ToString()},
                        { "download", true.ToString() },
                   }), null, null));
            }
        }

        private void CheckDetail(MvxPresentationHint hint)
        {
            throw new NotImplementedException();
        }

        private void CheckLogout(MvxPresentationHint hint)
        {
            if (hint is LogOutPresentationHint)
            {
                Show(new MvxViewModelRequest(typeof(LoginViewModel), null, null, null));
            }
        }

        private void CheckLogin(MvxPresentationHint hint)
        {
            if (hint is LogInPresentationHint)
            {
                Show(new MvxViewModelRequest(typeof(MainViewModel), null, null, null));
            }
        }

        private void CheckGroup(MvxPresentationHint hint)
        {
            throw new NotImplementedException();
        }

        private void CheckEvent(MvxPresentationHint hint)
        {
            throw new NotImplementedException();
        }

        private void CheckChangeTab(MvxPresentationHint hint)
        {
            if (hint is ChangeTabHint)
            {
                if (CrossCurrentActivity.Current.Activity is MainView)
                {
                    ((MainView)CrossCurrentActivity.Current.Activity).ViewPager.SetCurrentItem(4, true);
                }
            }
        }

        private void CheckChangeLanguage(MvxPresentationHint hint)
        {
            if (hint is LanguageChangedPresentationHint)
            {
                var intent = new Intent(Application.Context, typeof(MainView));
                intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                CrossCurrentActivity.Current.Activity.StartActivity(intent);
            }
        }

        #endregion Presentation hints
    }
}