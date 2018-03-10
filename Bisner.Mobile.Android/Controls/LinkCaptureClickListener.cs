using Android.App;
using Android.Widget;

namespace Bisner.Mobile.Droid.Controls
{
    public class LinkCaptureClickListener : BetterLinkMovementMethod.IOnLinkClickListener
    {
        public bool OnClick(TextView textView, string url)
        {
            //if (isPhoneNumber(url))
            //{
            //    FloatingMenuPhone.show(this, view, url);
            //}
            //else if (isEmailAddress(url))
            //{
            //    EmailFloatingMenu.show(this, view, url);
            //}
            //else if (isMapAddress(url))
            //{
            //    MapFloatingMenu.show(this, view, url);
            //}
            //else
            //{
            //    Toast.MakeText(this, url, ToastLength.Short).Show();
            //}

            Toast.MakeText(Application.Context, url, ToastLength.Short);

            return true;
        }

        //private bool isPhoneNumber(string url)
        //{
        //    return url.EndsWith(getString(R.string.bettermovementmethod_dummy_number));
        //}

        //private bool isEmailAddress(string url)
        //{
        //    return url.Contains("@");
        //}

        //private bool isMapAddress(string url)
        //{
        //    return url.Contains("goo.gl/maps");
        //}
    }
}