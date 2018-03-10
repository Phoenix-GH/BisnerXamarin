using System;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime dateTime)
        {
            if (DateTime.Now.Year == dateTime.Year)
            {
                return $"{dateTime.Day} {GetMonth(dateTime.Month)}";
            }

            return $"{dateTime.Day} {GetMonth(dateTime.Month)} {dateTime.Year}";
        }

        public static string ToMonthString(this DateTime dateTime)
        {
            return GetMonth(dateTime.Month);
        }

        public static string ToSimpleTimeString(this DateTime dateTime)
        {
            var hour = dateTime.Hour.ToString();
            var minute = dateTime.Minute.ToString();

            if (dateTime.Hour < 10)
            {
                hour = hour.Insert(0, "0");
            }

            if (dateTime.Minute < 10)
            {
                minute = minute.Insert(0, "0");
            }

            return $"{hour}:{minute}";
        }

        public static string ToSimpleDateString(this DateTime dateTime)
        {
            return $"{GetMonth(dateTime.Month)} {dateTime.Day}";
        }

        public static string ToLongDateString(this DateTime dateTime)
        {
            var dayInt = (int)dateTime.DayOfWeek;

            var result = $"{GetDay(dayInt)}, {GetMonth(dateTime.Month)} {dateTime.Day}, {dateTime.Year}";

            return result;
        }

        private static string GetMonth(int month)
        {
            string result;

            switch (month)
            {
                case 1:
                    result = Settings.GetResource(ResKeys.general_januari);
                    break;
                case 2:
                    result = Settings.GetResource(ResKeys.general_febuari);
                    break;
                case 3:
                    result = Settings.GetResource(ResKeys.general_march);
                    break;
                case 4:
                    result = Settings.GetResource(ResKeys.general_april);
                    break;
                case 5:
                    result = Settings.GetResource(ResKeys.general_may);
                    break;
                case 6:
                    result = Settings.GetResource(ResKeys.general_june);
                    break;
                case 7:
                    result = Settings.GetResource(ResKeys.general_juli);
                    break;
                case 8:
                    result = Settings.GetResource(ResKeys.general_august);
                    break;
                case 9:
                    result = Settings.GetResource(ResKeys.general_september);
                    break;
                case 10:
                    result = Settings.GetResource(ResKeys.general_october);
                    break;
                case 11:
                    result = Settings.GetResource(ResKeys.general_november);
                    break;
                case 12:
                    result = Settings.GetResource(ResKeys.general_december);
                    break;
                default:
                    result = "Unknown month";
                    break;
            }

            return result;
        }

        private static string GetDay(int day)
        {
            string result;

            switch (day)
            {
                case 0:
                    result = Settings.GetResource(ResKeys.general_sunday);
                    break;
                case 1:
                    result = Settings.GetResource(ResKeys.general_monday);
                    break;
                case 2:
                    result = Settings.GetResource(ResKeys.general_tuesday);
                    break;
                case 3:
                    result = Settings.GetResource(ResKeys.general_wednesday);
                    break;
                case 4:
                    result = Settings.GetResource(ResKeys.general_thursday);
                    break;
                case 5:
                    result = Settings.GetResource(ResKeys.general_friday);
                    break;
                case 6:
                    result = Settings.GetResource(ResKeys.general_saturday);
                    break;
                default:
                    result = "Unknown day";
                    break;
            }

            return result;
        }

        public static string ToChatTime(this DateTime value, bool amPmNotation = false)
        {
            var localTime = value.ToLocalTime();

            // Check the hour
            var hour = localTime.Hour.ToString();
            if (!amPmNotation && localTime.Hour < 10)
            {
                hour = "0" + hour;
            }
            else if (amPmNotation && localTime.Hour > 12)
            {
                hour = GetPmHour(localTime.Hour);
            }

            // Check the minute
            var minute = localTime.Minute.ToString();
            if (localTime.Minute < 10)
            {
                minute = "0" + minute;
            }

            // AM/PM indicator
            var ampm = "";
            if (amPmNotation)
            {
                ampm = localTime.Hour >= 12 ? " pm" : " am";
            }

            return $"{hour}:{minute}{ampm}";
        }

        private static string GetPmHour(int localTimeHour)
        {
            switch (localTimeHour)
            {
                case 13: return "1";
                case 14: return "2";
                case 15: return "3";
                case 16: return "4";
                case 17: return "5";
                case 18: return "6";
                case 19: return "7";
                case 20: return "8";
                case 21: return "9";
                case 22: return "10";
                case 23: return "11";
                case 24: return "12";
                default: return "0";
            }
        }
    }
}
