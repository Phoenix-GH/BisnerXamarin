using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.Droid.Controls.Adapters;
using Bisner.Mobile.Droid.Extensions;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using ActionBar = Android.Support.V7.App.ActionBar;

namespace Bisner.Mobile.Droid.Views.Manage
{
    [Activity(NoHistory = false)]
    public class ManageView : BaseToolbarActivity<ManageViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        public ManageView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(ViewModel.Type != ManageType.Default);

            SetupRecyclerView();
        }

        protected override int LayoutId => Resource.Layout.manage_fragment_toolbar;

        protected override string ScreenName => "MoreView";

        #endregion Activity

        #region Setup

        private void SetupRecyclerView()
        {
            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.manage_recyclerview);

            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                recyclerView.Adapter = new MvxGenericRecyclerAdapter<IManageItem>((IMvxAndroidBindingContext)BindingContext)
                {
                    ModifyViewFunc = ModifyViewFunc
                };
            }
        }

        private void ModifyViewFunc(View view)
        {
            SetImageColor(view.FindViewById<ImageView>(Resource.Id.manage_item_arrow));
        }

        private void SetImageColor(ImageView imageView)
        {
            if (imageView != null)
            {
                var drawable = imageView.Drawable;

                var color = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.arrowrightcolor));

                drawable.TintDrawable(color);
            }
        }



        #endregion Setup
    }
}