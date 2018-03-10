using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Models.Chat;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Chat.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Chat
{
    partial class ChatView : ViewBase<ChatViewModel>
    {
        #region Constructor

        private GenericTableviewSourceWithHeight<ConversationListViewModel> _source;

        private UIBarButtonItem _createButton;

        private MvxUIRefreshControl _refreshControl;

        public ChatView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("CHATVIEW RECIEVED MEMORY WARNING");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetCreateIcon();
            SetupAppearance();
            SetupTable();
            SetupBindings();

            // Set screen name for analytics
            ScreenName = "ChatView";
        }

        #endregion ViewController

        #region Setup

        private void SetupAppearance()
        {
            ConversationTable.BackgroundColor = Appearance.Colors.BackgroundColor;
        }

        private void SetupTable()
        {
            _source = new GenericTableviewSourceWithHeight<ConversationListViewModel>(ConversationTable)
            {
                GetIdentifierFunc = (model, path) => ChatCell.Identifier,
                RowSelectionFunc = (view, path, item) => ViewModel.ConversationSelected(item),
                ModifyCellFunc = (cell, indexPath, item) =>
                {
                    var chatCell = cell as ChatCell;

                    if (chatCell != null)
                    {
                        chatCell.SetRulerFullWidth(indexPath.Row == 0);

                        if (indexPath.Section + 1 == ConversationTable.NumberOfSections() &&
                            ConversationTable.NumberOfRowsInSection(indexPath.Section) == indexPath.Row + 1)
                        {
                            // Show bottom border on last cell
                            chatCell.SetBottomRulerVisible(true);
                        }
                        else
                        {
                            chatCell.SetBottomRulerVisible(false);
                        }
                    }
                },
                GetCellHeightFunc = (view, path, item) =>
                {
                    return 73;
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    return 73;
                },
                AutoDeselect = true,
                UseAnimations = true,
                AddAnimation = UITableViewRowAnimation.Top,
                RemoveAnimation = UITableViewRowAnimation.Bottom,
                ReplaceAnimation = UITableViewRowAnimation.Middle,
            };

            ConversationTable.Source = _source;

            ConversationTable.ContentInset = new UIEdgeInsets(5, 0, 0, 0);

            _refreshControl = new MvxUIRefreshControl();

            ConversationTable.AddSubviews(_refreshControl);
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ChatView, ChatViewModel>();
            set.Bind(_source).To(vm => vm.Conversations);
            set.Bind(_createButton).To(vm => vm.CreateChatCommand);
            set.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Apply();
            ConversationTable.ReloadData();
        }

        private void SetCreateIcon()
        {
            using (
                var createImage =
                    UIImage.FromBundle("Icons/create_post_btn.png")
                        .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                _createButton = new UIBarButtonItem(createImage, UIBarButtonItemStyle.Plain, null, null);
            }

            NavigationItem.SetRightBarButtonItem(_createButton, true);
        }

        #endregion Setup

        #region Base modifiactions

        protected override bool EnableTitleBarLogo { get { return true; } }

        #endregion Base modifications
    }
}
