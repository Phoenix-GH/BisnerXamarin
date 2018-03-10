using Foundation;

namespace Bisner.Mobile.iOS.Extensions
{
    public static class NSObjectExtensions
    {

        public static TType GetTypeValueFromOptions<TType>(this NSDictionary options, string key) where TType : class
        {
            var nsStringKey = new NSString(key);

            if (options.ContainsKey(nsStringKey))
            {
                // Key is present
                var value = options[nsStringKey];

                var parsedValue = value as TType;

                if (parsedValue != null)
                {
                    return parsedValue;
                }
            }

            return null;
        }
    }
}
