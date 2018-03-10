using System;
using System.Collections.Generic;

namespace Bisner.ApiModels.Whitelabel
{
    public class OpeningInformation
    {
        private List<OpeningDay> _specialDays;
        public OpeningDay Monday { get; set; }
        public OpeningDay Tuesday { get; set; }
        public OpeningDay Wednesday { get; set; }
        public OpeningDay Thursday { get; set; }
        public OpeningDay Friday { get; set; }
        public OpeningDay Saturday { get; set; }
        public OpeningDay Sunday { get; set; }

        /// <summary>
        /// Holidays, closed days
        /// </summary>
        public List<OpeningDay> SpecialDays
        {
            get { return _specialDays ?? (_specialDays = new List<OpeningDay>()); }
            set { _specialDays = value; }
        }

        /// <summary>
        /// Load default values
        /// </summary>
        public void LoadDefault()
        {
            Monday = new OpeningDay()
            {
                Open = true,
                DayOfWeek = DayOfWeek.Monday,
                HourOpen = 8,
                MinuteOpen = 30,
                HourClose = 18,
                MinuteClose = 0,
                UseDayOfWeek = true
            };

            Tuesday = new OpeningDay()
            {
                Open = true,
                DayOfWeek = DayOfWeek.Tuesday,
                HourOpen = 8,
                MinuteOpen = 30,
                HourClose = 18,
                MinuteClose = 0,
                UseDayOfWeek = true
            };

            Wednesday = new OpeningDay()
            {
                Open = true,
                DayOfWeek = DayOfWeek.Wednesday,
                HourOpen = 8,
                MinuteOpen = 30,
                HourClose = 18,
                MinuteClose = 0,
                UseDayOfWeek = true
            };

            Thursday = new OpeningDay()
            {
                Open = true,
                DayOfWeek = DayOfWeek.Thursday,
                HourOpen = 8,
                MinuteOpen = 30,
                HourClose = 18,
                MinuteClose = 0,
                UseDayOfWeek = true
            };

            Friday = new OpeningDay()
            {
                Open = true,
                DayOfWeek = DayOfWeek.Friday,
                HourOpen = 8,
                MinuteOpen = 30,
                HourClose = 18,
                MinuteClose = 0,
                UseDayOfWeek = true
            };

            Saturday = new OpeningDay()
            {
                Open = false,
                DayOfWeek = DayOfWeek.Saturday,
                UseDayOfWeek = true
            };

            Sunday = new OpeningDay()
            {
                Open = false,
                DayOfWeek = DayOfWeek.Sunday,
                UseDayOfWeek = true
            };

            SpecialDays = new List<OpeningDay>();
        }
    }

    public class OpeningDay
    {
        /// <summary>
        /// Open on this day / false = closed
        /// </summary>
        public bool Open { get; set; }

        /// <summary>
        /// Day of the week
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// If set to true use the enum else use the specific day object for a specific date
        /// </summary>
        public bool UseDayOfWeek { get; set; }

        /// <summary>
        /// Specific date (For example holidays)
        /// </summary>
        public DateTime SpecificDate { get; set; }

        /// <summary>
        /// Hour opening
        /// </summary>
        public int HourOpen { get; set; }

        /// <summary>
        /// Minute opening
        /// </summary>
        public int MinuteOpen { get; set; }

        /// <summary>
        /// Hour closing
        /// </summary>
        public int HourClose { get; set; }

        /// <summary>
        /// Minute closing
        /// </summary>
        public int MinuteClose { get; set; }
    }
}