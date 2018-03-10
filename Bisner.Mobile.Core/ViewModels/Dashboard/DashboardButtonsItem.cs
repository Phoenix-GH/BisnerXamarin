using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Members;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class DashboardButtonItem : ItemBase
    {
        #region Constructor

        private MvxCommand _membersCommand;
        private MvxCommand _eventsCommand;
        private MvxCommand _groupsCommand;
        private MvxCommand _infoCommand;
        
        #endregion Constructor

        #region Texts

        public string MembersText => Settings.GetResource(ResKeys.mobile_dashboard_btn_members);

        public string EventsText => Settings.GetResource(ResKeys.mobile_dashboard_btn_events);

        public string GroupsText => Settings.GetResource(ResKeys.mobile_dashboard_btn_groups);

        public string MoreText => Settings.GetResource(ResKeys.mobile_dashboard_btn_more);

        #endregion Texts

        #region Commands

        public MvxCommand MembersCommand => _membersCommand ?? (_membersCommand = new MvxCommand(ShowMembers));

        public MvxCommand EventsCommand => _eventsCommand ?? (_eventsCommand = new MvxCommand(ShowEvents));

        public MvxCommand GroupsCommand => _groupsCommand ?? (_groupsCommand = new MvxCommand(ShowGroups));

        public MvxCommand InfoCommand => _infoCommand ?? (_infoCommand = new MvxCommand(ShowInfo));

        private void ShowEvents()
        {
            ShowViewModel<EventCategoriesViewModel>();
        }

        private void ShowMembers()
        {
            ShowViewModel<MembersViewModel>();
        }

        private void ShowGroups()
        {
            ShowViewModel<GroupsViewModel>();
        }

        private void ShowInfo()
        {
            // Show manage screen
            Mvx.Resolve<IMvxViewDispatcher>().ChangePresentation(new ChangeTabHint { TabIndex = 4 });
        }

        #endregion Commands
    }
}