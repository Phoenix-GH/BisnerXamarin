using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace Bisner.Mobile.Core.ValueConverters
{
    public class TimeAgoValueConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            // Value is UTC
            value = value.ToLocalTime();

            var now = DateTime.Now;

            var elapsed = now.Subtract(value);


            if (Math.Round(elapsed.TotalMinutes) == 1)
            {
                return "1 minute ago";
            }
            if (Math.Round(elapsed.TotalMinutes) > 1 && Math.Round(elapsed.TotalHours) == 0)
            {
                return Math.Round(elapsed.TotalMinutes) + " " + "minutes ago";
            }
            if (Math.Round(elapsed.TotalHours) == 1)
            {
                return "1 hour ago";
            }
            if (Math.Round(elapsed.TotalHours) > 1 && Math.Round(elapsed.TotalDays) == 0)
            {
                return Math.Round(elapsed.TotalHours) + " hours ago";
            }
            if (Math.Round(elapsed.TotalDays) == 1)
            {
                return "1 day ago";
            }
            if (Math.Round(elapsed.TotalDays) > 1)
            {
                return Math.Round(elapsed.TotalDays) + " days ago";
            }

            return "1 minute ago";
        }
    }
}
