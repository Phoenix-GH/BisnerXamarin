using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.Droid.Controls.Adapters;
using Bisner.Mobile.Droid.Extensions;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Manage
{
    [Register("bisner.mobile.droid.views.manage.ManageFragment")]
    public class ManageFragment : BaseToolbarFragment<AndroidManageViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Fragment

        protected override int FragmentId => Resource.Layout.manage_fragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(ViewModel.Type != ManageType.Default);

            SetupRecyclerView(view);

            return view;
        }

        protected override string ScreenName => "MoreView";

        #endregion Fragment

        #region Setup

        private void SetupRecyclerView(View view)
        {
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.manage_recyclerview);

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