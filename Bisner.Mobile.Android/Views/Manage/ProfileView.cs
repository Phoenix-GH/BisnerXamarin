using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.ViewModels.Manage.User;
using Bisner.Mobile.Droid.Controls;
using Bisner.Mobile.Droid.Views.Base;
using Square.Picasso;

namespace Bisner.Mobile.Droid.Views.Manage
{
    [Activity(NoHistory = false)]
    public class ProfileView : BaseToolbarActivity<ProfileViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        public ProfileView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override int LayoutId => Resource.Layout.profile_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var headerImage = FindViewById<ImageView>(Resource.Id.profile_fragment_header);

            Picasso.With(this).Load(Resource.Drawable.contact_background).Resize(1280, 700).CenterCrop().Into(headerImage);
        }

        protected override string ScreenName => "ProfileView";

        #endregion Activity

        #region Menu

        private IMenuItem _addMenuItem;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.profile_toolbar, menu);

            _addMenuItem = menu.GetItem(0);
           

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.update_profile_btn)
            {
                ViewModel.UpdateCommand.Execute(null);

                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion Menu
    }
}