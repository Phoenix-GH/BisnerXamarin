using System;
using Bisner.Mobile.Core.ViewModels.Booking;

namespace Bisner.Mobile.Core.Models.Booking
{
    public class HeaderSliderData
    {
        public HeaderSliderData(Guid id, string _imageUrl, string _title, string _subTitle, string _date, HeaderSliderItemType _type, bool _isAvailable = true)
        {
            Id = id;
            ImageUrl = _imageUrl;
            Title = _title;
            SubTitle = _subTitle;
            Date = _date;
            Type = _type;
            IsAvailable = _isAvailable;
        }

        public HeaderSliderData(Guid id, string _imageUrl, string _title, HeaderSliderItemType _type, bool _isAvailable = true)
        {
            Id = id;
            ImageUrl = _imageUrl;
            Title = _title;
            Type = _type;
            IsAvailable = _isAvailable;
        }

        #region property

        public Guid Id { get; set; }

        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Date { get; set; }
        public HeaderSliderItemType Type { get; set; }
        public bool IsAvailable { get; set; }

        #endregion
    }
}
