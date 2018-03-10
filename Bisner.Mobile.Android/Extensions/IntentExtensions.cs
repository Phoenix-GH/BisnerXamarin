using System.Diagnostics;
using System.Text;
using Android.Content;

namespace Bisner.Mobile.Droid.Extensions
{
    public static class IntentExtensions
    {
        public static void PrintExtras(this Intent intent)
        {
            if (intent?.Extras != null)
            {
                var msg = new StringBuilder();

                foreach (var key in intent.Extras.KeySet())
                {
                    var extra = intent.Extras.Get(key);

                    Debug.WriteLine($"GCM Messages Key [{key}] value [{extra}]");
                    msg.AppendLine(key + "=" + extra);
                }
            }
        }
    }
}