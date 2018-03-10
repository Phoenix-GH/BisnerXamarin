using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Droid.Controls.Adapters;
using Bisner.Mobile.Droid.Extensions;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Activity(NoHistory = false)]
    public class EventCategoriesView : BaseToolbarActivity<EventCategoriesViewModel>
    {
        #region Variables

        private DisplayMetrics _displayMetrics;
        private float _densityPixels;

        #endregion Variables

        #region Constructor

        public EventCategoriesView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Fragment

        protected override int LayoutId => Resource.Layout.eventcategories_view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _displayMetrics = new DisplayMetrics();

            WindowManager.DefaultDisplay.GetMetrics(_displayMetrics);

            _densityPixels = TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, _displayMetrics);

            SetupRecyclerView();

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        protected override string ScreenName => "EventCategoriesView";

        #endregion Fragment

        #region Setup

        private void SetupRecyclerView()
        {
            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.eventcategories_recycler_view);

            if (recyclerView != null)
            {
                //recyclerView.HasFixedSize = true;
                recyclerView.Adapter = new MvxGenericRecyclerAdapter<IItemBase>((IMvxAndroidBindingContext)BindingContext)
                {
                    ModifyViewFunc = ModifyItem
                };
            }
        }

        private void ModifyItem(View view)
        {
            SetImageHeight(view.FindViewById<MvxImageView>(Resource.Id.eventcategories_item_double_image1));
            SetImageHeight(view.FindViewById<MvxImageView>(Resource.Id.eventcategories_item_double_image2));
            SetImageColor(view.FindViewById<ImageView>(Resource.Id.eventcategory_right_arrow));
        }

        private void SetImageHeight(MvxImageView imageView)
        {
            if (imageView != null)
            {
                // Margins on the imags are :
                // Left : 15dp
                // Right : 15dp
                // Middle : 15dp

                var marginPixels = _densityPixels * 45;

                imageView.LayoutParameters.Height = (int)((_displayMetrics.WidthPixels - marginPixels) / 2) / 2;
            }
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