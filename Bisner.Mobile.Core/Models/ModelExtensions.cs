using System;
using System.Collections.Generic;
using System.Linq;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.General;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Company;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.Models.Feed.DataModels;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.Service;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace Bisner.Mobile.Core.Models
{
    public static class ModelExtensions
    {
        public static IImage ToModel(this ApiImageModel imageModel)
        {
            if (imageModel == null)
            {
                return null;
            }

            return new Image
            {
                Id = imageModel.Id,
                Large = imageModel.Large,
                Medium = imageModel.Medium,
                MimeType = imageModel.MimeType,
                OriginalFileName = imageModel.OriginalFileName,
                Small = imageModel.Small,
            };
        }

        #region Company

        public static ICompany ToModel(this ApiWhitelabelCompanyModel companyModel)
        {
            return new Company.Company
            {
                Id = companyModel.Id,
                AdminIds = companyModel.AdminIds,
                GuestUserIds = companyModel.GuestUserIds,
                IndustryId = companyModel.IndustryId,
                PendingGuestUserIds = companyModel.PendingGuestUserIds,
                City = companyModel.City,
                FacebookUrl = companyModel.FacebookUrl,
                LinkedInUrl = companyModel.LinkedInUrl,
                TwitterUrl = companyModel.TwitterUrl,
                InstagramUrl = companyModel.InstagramUrl,
                Name = companyModel.Name,
                LocationId = companyModel.LocationId,
                CollaborationEnabled = companyModel.CollaborationEnabled,
                Telephone = companyModel.Telephone,
                Address = companyModel.Address,
                Address2 = companyModel.Address2,
                Country = companyModel.Country,
                Description = companyModel.Description,
                Founded = companyModel.Founded,
                Summary = companyModel.Summary,
                WebUrl = companyModel.WebUrl,
                UserIds = companyModel.UserIds,
                PendingGuests = companyModel.PendingGuestUserIds,
                PendingUsersIds = companyModel.PendingGuestUserIds,
                GuestsIds = companyModel.GuestUserIds,
                Logo = companyModel.Logo.ToModel()
            };
        }

        #endregion Company

        #region User

        public static IUser ToModel(this ApiWhitelabelPublicUserModel userModel, string companyName = null)
        {
            return new User
            {
                Id = userModel.Id,
                DisplayName = userModel.DisplayName,
                LinkedInUrl = userModel.LinkedInUrl,
                InstagramUrl = userModel.InstagramUrl,
                LocationId = userModel.LocationId,
                TwitterUrl = userModel.TwitterUrl,
                FacebookUrl = userModel.FacebookUrl,
                City = userModel.City,
                LastLoginDate = userModel.LastLoginDate,
                FirstName = userModel.FirstName,
                About = userModel.About,
                IsPending = userModel.IsPending,
                GooglePlusUrl = userModel.GooglePlusUrl,
                Disabled = userModel.Disabled,
                LastName = userModel.LastName,
                ShortAbout = userModel.ShortAbout,
                Avatar = userModel.Avatar.ToModel(),
                Email = userModel.Email,
                Header = userModel.Header.ToModel(),
                CustomField1 = userModel.CustomField1,
                CustomField2 = userModel.CustomField2,
                CustomField3 = userModel.CustomField3,
                CustomField4 = userModel.CustomField4,
                CustomField5 = userModel.CustomField5,
                CustomField6 = userModel.CustomField6,
                CustomField7 = userModel.CustomField7,
                CustomField8 = userModel.CustomField8,
                CustomField9 = userModel.CustomField9,
                CustomField10 = userModel.CustomField10,
                CustomField11 = userModel.CustomField11,
                CustomField12 = userModel.CustomField12,
                CustomField13 = userModel.CustomField13,
                CustomField14 = userModel.CustomField14,
                CustomField15 = userModel.CustomField15,
                CustomField16 = userModel.CustomField16,
                CustomField17 = userModel.CustomField17,
                CustomField18 = userModel.CustomField18,
                CustomField19 = userModel.CustomField19,
                CustomField20 = userModel.CustomField20,
                CustomField21 = userModel.CustomField21,
                CustomField22 = userModel.CustomField22,
                CustomField23 = userModel.CustomField23,
                CustomField24 = userModel.CustomField24,
                CustomField25 = userModel.CustomField25,
                CustomField26 = userModel.CustomField26,
                CustomField27 = userModel.CustomField27,
                CustomField28 = userModel.CustomField28,
                CustomField29 = userModel.CustomField29,
                CustomField30 = userModel.CustomField30,
                FromCentralDatabase = userModel.FromCentralDatabase,
                Skills = userModel.Skills,
                CompanyName = companyName,
            };
        }

        #endregion User

        #region Event

        public static IEvent ToModel(this ApiWhitelabelEventModel eventModel, string eventClosedText, string unattendButtonText, string attendButtonText, string peopleAttendingText, string eventInfoText, string eventDateText, string eventTimeLabel, string eventLocationLabel, string aboutHeaderLabel)
        {
            if (eventModel == null)
                return null;

            var evnt = new Event(eventClosedText, unattendButtonText, attendButtonText, peopleAttendingText, eventInfoText, eventDateText, eventTimeLabel, eventLocationLabel, aboutHeaderLabel)
            {
                Id = eventModel.Id,
                DateTime = eventModel.DateTime,
                CreationDateTime = eventModel.CreationDateTime,
                Summary = eventModel.Summary,
                ParentId = eventModel.ParentId,
                AttendeesIds = eventModel.AttendeesIds,
                CategoryId = eventModel.CategoryId,
                IsPublished = eventModel.IsPublished,
                LinkedPostedId = eventModel.LinkedPostedId,
                Location = eventModel.Location,
                Title = eventModel.Title,
                Details = eventModel.Details,
                SubTitle = eventModel.SubTitle,
                Header = eventModel.Header?.ToModel(),
                Images = eventModel.Images?.Select(i => i.ToModel()).ToList(),
            };

            if (eventModel.CreatedByUser != null)
            {
                evnt.UserId = eventModel.CreatedByUser.Id;
                evnt.DisplayName = eventModel.CreatedByUser?.DisplayName;
                evnt.AvatarUrl = eventModel.CreatedByUser.Avatar?.Small;
            }

            if (eventModel.Attendees != null)
            {
                evnt.Attendees = eventModel.Attendees.Select(a => a.ToModel()).ToList();
                evnt.HasAttendees = eventModel.Attendees.Any();
            }

            return evnt;
        }

        #endregion Event

        #region EventCategory

        public static IEventCategory ToModel(this ApiCentralEventCategoryModel apiCentralEventCategoryModel)
        {
            var eventCategory = new EventCategory
            {
                Id = apiCentralEventCategoryModel.Id,
                Image = apiCentralEventCategoryModel.Image?.ToModel(),
                Name = apiCentralEventCategoryModel.Name,
                Type = (EventCategoryType)apiCentralEventCategoryModel.Type,
                UsersCanCreateEvent = apiCentralEventCategoryModel.UsersCanCreateEvent,
            };

            return eventCategory;
        }

        #endregion EventCategory

        #region Comment

        public static IComment ToModel(this ApiWhitelabelCommentModel commentModel)
        {
            if (commentModel == null) return null;

            var comment = new Comment
            {
                Id = commentModel.Id,
                UserId = commentModel.User?.Id ?? Guid.Empty,
                AvatarUrl = commentModel.User?.Avatar?.Small,
                DateTime = commentModel.DateTime,
                DisplayName = commentModel.User?.DisplayName,
                Text = commentModel.Comment,
            };

            if (!string.IsNullOrWhiteSpace(commentModel.Comment))
            {
                var text = EmojiHelper.ShortnameToUnicode(commentModel.Comment);
                text = BbCode.ConvertToHtml(text);

                comment.Text = text;
            }

            return comment;
        }

        #endregion Comment

        #region Data models
        public static FeedPostDataModel ToDataModel(this string dataString)
        {
            try
            {
                return JsonConvert.DeserializeObject<FeedPostDataModel>(dataString);
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);

                return null;
            }
        }

        #endregion Data moels
    }
}
