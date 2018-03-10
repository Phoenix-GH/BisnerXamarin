using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Graphics.Drawable;

namespace Bisner.Mobile.Droid.Extensions
{
    public static class DrawableExtensions
    {
        public static Drawable TintDrawable(this Drawable drawable, Color color)
        {
            drawable = DrawableCompat.Wrap(drawable);
            DrawableCompat.SetTint(drawable.Mutate(), color);

            return drawable;
        }

        private static Bitmap ScaleDrawableBitmap(this Drawable drawable)
        {
            // Get top drawable
            var bitmapDrawable = (BitmapDrawable)drawable;
            var bitmap = Bitmap.CreateScaledBitmap(bitmapDrawable.Bitmap, (int)(bitmapDrawable.IntrinsicHeight * 1.7), (int)(bitmapDrawable.IntrinsicWidth * 1.7), false);

            return bitmap;
        }
    }
}