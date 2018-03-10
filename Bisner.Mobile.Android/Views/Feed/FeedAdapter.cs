using Android.App;
using Android.Support.V4.Text.Util;
using Android.Text;
using Android.Text.Method;
using Android.Text.Util;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Droid.Controls;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Feed
{
    /// <summary>
    /// Special adapter for the feed to set image aspect ratio bacause the PercentageRelativeLayout does not work inside a recyclerview
    /// This is due to the parent of the PercentageRelativeLayout having layout height as wrap content
    /// </summary>
    /// <seealso cref="MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerAdapter" />
    public class FeedAdapter : MvxRecyclerAdapter
    {
        #region Variables

        private readonly DisplayMetrics _displayMetrics;
        private readonly float _pixels;

        #endregion Variables

        #region Constructor

        public FeedAdapter(DisplayMetrics displayMetrics, IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
            _displayMetrics = displayMetrics;

            // Convert 1 dp to number of pixels for reference
            _pixels = TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, _displayMetrics);


        }

        #endregion Constructor

        #region Adapter

        protected override View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            var view = base.InflateViewForHolder(parent, viewType, bindingContext);

            SetupImages(view);

            SetupTextView(view);

            return view;
        }

        #endregion Adapter

        #region Setup

        private void SetupImages(View view)
        {
            // Main image width
            var mainImage = view.FindViewById<MvxImageView>(Resource.Id.feed_item_main_image);

            if (mainImage != null)
            {
                // Set image height to be 50% of the screen width
                mainImage.LayoutParameters.Height = _displayMetrics.WidthPixels / 2;
            }

            // Left image width
            var leftImage = view.FindViewById<MvxImageView>(Resource.Id.feed_item_left_image);

            if (leftImage != null)
            {
                // There is a 4 dp margin the xml layout between both images, so we need to find out how many pixels that is for the current resolution
                var pixelMargin = (int)_pixels * 4;

                // Remove the pixel margin from the screen width and divide by 2 to get the image width, then divide by 2 again for 50% height
                leftImage.LayoutParameters.Height = ((_displayMetrics.WidthPixels - pixelMargin) / 2) / 2;
            }

            // Right image will be matching left in height with layout params in the xml

            // Group item header
            var groupItemHeaderImage = view.FindViewById<MvxImageView>(Resource.Id.feed_item_group_header);

            if (groupItemHeaderImage != null)
            {
                groupItemHeaderImage.LayoutParameters.Height = _displayMetrics.WidthPixels / 2;
            }

            // Event item header
            var companyitemHeaderImage = view.FindViewById<MvxImageView>(Resource.Id.feed_item_company_header);

            if (companyitemHeaderImage != null)
            {
                companyitemHeaderImage.LayoutParameters.Height = _displayMetrics.WidthPixels / 2;
            }
        }

        /// <summary>
        /// Setups the text view.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetupTextView(View view)
        {
            var textView = view.FindViewById<TextView>(Resource.Id.feed_item_post_text);

            if (textView != null)
            {
                // Clickable links
                //textView.MovementMethod = BetterLinkMovementMethod.GetInstance();
                BetterLinkMovementMethod.LinkifyHtml(textView).SetOnLinkClickListener(new LinkCaptureClickListener());
            }
        }

        #endregion Setup
    }
}