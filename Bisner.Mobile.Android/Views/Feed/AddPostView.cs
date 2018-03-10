using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using ActionMenuView = Android.Support.V7.Widget.ActionMenuView;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Bisner.Mobile.Droid.Views.Feed
{
    [Activity(NoHistory = false)]
    public class AddPostView : BaseToolbarActivity<AddPostViewModel>
    {
        #region Variables

        private IMenuItem _addPostMenuItem;
        private ProgressBar _toolbarProgressbar;
        private ActionMenuView _actionMenuView;

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Fragment

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetupEditText();

            SetupRecyclerView();

            SetupBottomToolbar();

            SetupProgressBar();

            // Add handlers
            ViewModel.AfterPostAction = AfterPostAction;
            ViewModel.StartPosting += ViewModelOnStartPosting;
            ViewModel.StopPosting += ViewModelOnStopPosting;
        }

        protected override int LayoutId => Resource.Layout.add_post_view;

        protected override string ScreenName => "AddPostView feedId = " + ViewModel.FeedId;

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Remove handlers
            ViewModel.AfterPostAction = null;
            ViewModel.StartPosting -= ViewModelOnStartPosting;
            ViewModel.StopPosting -= ViewModelOnStopPosting;
            _actionMenuView.MenuItemClick -= ActionMenuViewOnMenuItemClick;

            _addPostMenuItem = null;
            _toolbarProgressbar = null;
            _actionMenuView = null;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_post_toolbar, menu);

            // Get the add post item
            _addPostMenuItem = menu.GetItem(0);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.add_post_post_btn)
            {
                ViewModel.SendCommand.Execute(null);

                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion Fragment

        #region Setup

        private void SetupEditText()
        {
            var editText = FindViewById<EditText>(Resource.Id.add_post_text);

            var set = this.CreateBindingSet<AddPostView, AddPostViewModel>();
            set.Bind(editText).For(e => e.Hint).To(vm => vm.PlaceholderText);
            set.Apply();
        }

        private void SetupRecyclerView()
        {
            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.add_post_image_recyclerview);

            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;

                var layoutManager = new LinearLayoutManager(this, RecyclerView.Horizontal, false);
                //var gridLayoutManager = new GridLayoutManager(Activity, 1, RecyclerView.Horizontal, false);

                recyclerView.SetLayoutManager(layoutManager);
            }
        }

        #endregion Setup

        #region Progress

        private void ViewModelOnStartPosting()
        {
            // Set the actionbar progress visible
            if (_toolbarProgressbar != null)
            {
                RunOnUiThread(() =>
                {
                    _toolbarProgressbar.Visibility = ViewStates.Visible;
                    _addPostMenuItem.SetEnabled(false);
                });
            }
        }

        private void ViewModelOnStopPosting()
        {
            // Set the actionbar progress invisible
            if (_toolbarProgressbar != null)
            {
                RunOnUiThread(() =>
                {
                    _toolbarProgressbar.Visibility = ViewStates.Invisible;
                    _addPostMenuItem.SetEnabled(true);
                });
            }
        }

        private void AfterPostAction()
        {
            // Hide keyboard
            HideKeyboard(this);

            // Close the viewmodel
            ViewModel.CloseCommand.Execute(null);
        }

        public void HideKeyboard(Context context)
        {
            var inputManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);

            // check if no view has focus:
            var currentFocusView = CurrentFocus;
            if (currentFocusView == null)
                return;

            inputManager.HideSoftInputFromInputMethod(currentFocusView.WindowToken, 0);
        }

        #endregion

        #region Toolbar

        private void SetupBottomToolbar()
        {
            var bottomToolbar = FindViewById<Toolbar>(Resource.Id.add_post_bottom_toolbar);

            _actionMenuView = bottomToolbar?.FindViewById<ActionMenuView>(Resource.Id.add_post_bottom_toolbar_actionmenu);

            if (_actionMenuView != null)
            {
                MenuInflater.Inflate(Resource.Menu.add_post_bottom, _actionMenuView.Menu);

                _actionMenuView.MenuItemClick += ActionMenuViewOnMenuItemClick;
            }
        }

        private void ActionMenuViewOnMenuItemClick(object sender, ActionMenuView.MenuItemClickEventArgs menuItemClickEventArgs)
        {
            switch (menuItemClickEventArgs.Item.ItemId)
            {
                case Resource.Id.add_post_camera:
                    ViewModel.TakePictureCommand.Execute(null);
                    break;
                case Resource.Id.add_post_image:
                    ViewModel.ChoosePictureCommand.Execute(null);
                    break;
            }
        }

        private void SetupProgressBar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            if (toolbar != null)
            {
                _toolbarProgressbar = toolbar.FindViewById<ProgressBar>(Resource.Id.toolbar_progress_bar);

                if (_toolbarProgressbar != null)
                {
                    // Set invisible on start
                    _toolbarProgressbar.Visibility = ViewStates.Invisible;
                }
            }
        }

        #endregion Toolbar
    }
}