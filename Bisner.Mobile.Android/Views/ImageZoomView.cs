using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Widget;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Droid.Controls;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Platform;
using MvvmCross.Plugins.DownloadCache;

namespace Bisner.Mobile.Droid.Views
{
    [Activity(NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ImageZoomView : BaseActivity<ImageZoomViewModel>
    {
        #region Variables

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Activity

        protected override int LayoutId => Resource.Layout.image_zoom_view;

        protected override string ScreenName => "ImageZoomView";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var closeButton = FindViewById<Button>(Resource.Id.image_zoom_close_button);

            if (closeButton != null)
            {
                closeButton.Text = "Close";
                closeButton.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
            }
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            var scaleImage = FindViewById<ScaleImageView>(Resource.Id.image_zoom_image_view);

            if (scaleImage != null)
            {
                var mvxImageCache = Mvx.Resolve<IMvxImageCache<Bitmap>>();

                mvxImageCache.RequestImage(Settings.BlobUrl + ViewModel.ImageUrl).ContinueWith(async task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted && task.Result != null)
                    {
                        await Task.Delay(1000);

                        RunOnUiThread(() =>
                        {
                            scaleImage.SetImageBitmap(task.Result);
                        });
                    }
                });
            }
        }

        #endregion Activity
    }
}