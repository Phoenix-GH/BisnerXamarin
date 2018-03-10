using System;
using System.Globalization;
using Android.OS;
using Android.Text;
using MvvmCross.Platform.Converters;

namespace Bisner.Mobile.Droid.ValueConverters
{
    public class HtmlTextValueConverter : MvxValueConverter<string, ISpanned>
    {
        protected override ISpanned Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertToHtml(value);
        }

        private ISpanned ConvertToHtml(string value)
        {
            ISpanned result;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                result = Html.FromHtml(value, FromHtmlOptions.ModeLegacy);
            }
            else
            {
                result = Html.FromHtml(value);
            }
            return result;
        }
    }
}