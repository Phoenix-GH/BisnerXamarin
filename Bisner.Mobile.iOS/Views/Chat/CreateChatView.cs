using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Controls.Gestures;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Chat.Cells;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Chat
{
    partial class CreateChatView : KeyboardListenerViewBase<CreateChatViewModel>
    {
        #region Constructor

        private GenericTableViewSource<IUser> _source;

        private UIBarButtonItem _createButton, _backButton;

        public CreateChatView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("CREATECHATVIEW RECIEVED MEMORY WARNING!!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetCreateIcon();
            SetupAppearance();
            SetupTable();
            SetupBindings();

            // Set the navigation bar hidden so we don't get a double nav bar
            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            // Set screen name for analytics
            ScreenName = "CreateChatView";
        }

        #endregion ViewController

        #region Setup

        private void SetupAppearance()
        {
            ContactTable.BackgroundColor = Appearance.Colors.BackgroundColor;
        }

        private void SetupTable()
        {
            _source = new GenericTableViewSource<IUser>(ContactTable)
            {
                GetIdentifierFunc = (model, path) => ContactCell.Identifier,
                RowSelectionFunc = (view, path, user) =>
                {
                    NavigationController.PopViewController(false);
                    ViewModel.ItemSelectCommand.Execute(user);
                },
                AutoDeselect = true,
            };
            ContactTable.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<CreateChatView, CreateChatViewModel>();
            set.Bind(Title).To(vm => vm.Title);
            set.Bind(_source).To(vm => vm.Items);
            set.Apply();

            ContactTable.EstimatedRowHeight = new nfloat(150);
            ContactTable.RowHeight = UITableView.AutomaticDimension;
            ContactTable.ReloadData();
        }

        private void SetCreateIcon()
        {
            // Add hamburger icon to left of navigation bar
            _createButton = new UIBarButtonItem
            {
                Title = "chat",
            };

            NavigationItem.SetRightBarButtonItem(_createButton, true);

            // This is to set the back button on the child views (in this case addPost)
            using (
                var closeImage =
                    UIImage.FromBundle("Icons/close.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                _backButton = new UIBarButtonItem(closeImage, UIBarButtonItemStyle.Plain, null, null);
            }
            _backButton.Clicked += (sender, args) => { NavigationController.PopViewController(true); };

            NavigationController.InteractivePopGestureRecognizer.Delegate = new SwipeGestureDelegate();
            NavigationItem.SetLeftBarButtonItem(_backButton, true);
        }

        #endregion Setup
    }
}
