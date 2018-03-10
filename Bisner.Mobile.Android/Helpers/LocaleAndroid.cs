using Bisner.Mobile.Core.Helpers;

namespace Bisner.Mobile.Droid.Helpers
{
    public class LocaleAndroid : ILocale
    {
        public string GetCurrentLocaleId()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-");
            return netLanguage.ToLower();
        }
    }
}