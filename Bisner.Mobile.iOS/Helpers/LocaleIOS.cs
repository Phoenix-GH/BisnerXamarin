using Bisner.Mobile.Core.Helpers;
using Foundation;

namespace Bisner.Mobile.iOS.Helpers
{
    public class LocaleIOS : ILocale
    {
        public string GetCurrentLocaleId()
        {
            var iOSLocale = NSLocale.CurrentLocale.CountryCode;
            return iOSLocale.ToLower();
        }
    }
}