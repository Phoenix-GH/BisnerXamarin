using System;
using System.Globalization;
using Bisner.ApiModels.Central;
using Bisner.Mobile.Core.Helpers;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace Bisner.Mobile.Core.ValueConverters
{
    public class ImageUrlValueConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var configuration = Mvx.Resolve<IConfiguration>();

                if (value == "default")
                {
                    return configuration.BaseUrl + "Content/Dashboard/Assets/Img/Avatars/default_user_avatar.png";
                }

                if (value == Defaults.EventHeaderDefaultString)
                {
                    var eventHeaderUrl = Settings.EventHeaderUrl;

                    if (!string.IsNullOrEmpty(eventHeaderUrl))
                    {
                        return Settings.BlobUrl + eventHeaderUrl;
                    }

                    return configuration.BaseUrl + ApiCentralPlatformImages.EventsHeaderDefault;
                }

                if (value == Defaults.CompanyHeaderDefaultString)
                {
                    var companyHeaderUrl = Settings.CompanyHeaderUrl;

                    if (!string.IsNullOrEmpty(companyHeaderUrl))
                    {
                        return Settings.BlobUrl + companyHeaderUrl;
                    }

                    var bla = configuration.BaseUrl + ApiCentralPlatformImages.CompanyHeaderDefault;

                    return bla;
                }

                if (value == Defaults.GroupHeaderDefault)
                {
                    var groupsHeaderUrl = Settings.GroupsHeaderUrl;

                    if (!string.IsNullOrEmpty(groupsHeaderUrl))
                    {
                        return Settings.BlobUrl + groupsHeaderUrl;
                    }

                    return configuration.BaseUrl + ApiCentralPlatformImages.GroupsHeaderDefault;
                }

                if (value == Defaults.RoomHeaderDefault)
                {
                    return configuration.BaseUrl + ApiCentralPlatformImages.RoomHeaderDefault;
                }

                var fullUrl = Settings.BlobUrl + value;

                return fullUrl;
            }

            return null;
        }
    }
}
