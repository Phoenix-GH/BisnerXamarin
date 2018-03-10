// Helpers/Settings.cs

using System;
using System.Collections.Generic;
using Bisner.ApiModels.Central;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Linq;
using Newtonsoft.Json;

namespace Bisner.Mobile.Core.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        #region Settings

        private static ISettings AppSettings => CrossSettings.Current;

        #endregion Settings

        #region Setting Constants

        private static readonly string StringDefault = string.Empty;
        private static readonly Guid GuidDefault = Guid.Empty;
        private static readonly bool BoolDefault = false;
        private static readonly int IntDefault = 0;
        private static readonly DateTime DateTimeDefault = DateTime.MinValue;
        private static readonly LanguageModel LanguageModelDefault = null;

        #endregion Setting Constants

        #region User

        private const string UsernameKey = "Username";
        public static string Username
        {
            get { return AppSettings.GetValueOrDefault(UsernameKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(UsernameKey, value); }
        }

        private const string UserIdKey = "UserId";
        public static Guid UserId
        {
            get { return AppSettings.GetValueOrDefault(UserIdKey, GuidDefault); }
            set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
        }
        
        #endregion User

        #region Token

        private const string RefreshTokenKey = "RefreshToken";
        public static string RefreshToken
        {
            get { return AppSettings.GetValueOrDefault(RefreshTokenKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(RefreshTokenKey, value); }
        }

        private const string TokenKey = "Token";
        public static string Token
        {
            get { return AppSettings.GetValueOrDefault(TokenKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(TokenKey, value); }
        }

        private const string TokenExpirationKey = "TokenExpiration";
        public static DateTime TokenExpiration
        {
            get { return AppSettings.GetValueOrDefault(TokenExpirationKey, DateTimeDefault); }
            set { AppSettings.AddOrUpdateValue(TokenExpirationKey, value); }
        }

        #endregion Token

        #region Image

        private const string BlobUrlKey = "BlobUrl";
        public static string BlobUrl
        {
            get { return AppSettings.GetValueOrDefault(BlobUrlKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(BlobUrlKey, value); }
        }

        private const string EventHeaderUrlKey = "EventHeader";
        public static string EventHeaderUrl
        {
            get { return AppSettings.GetValueOrDefault(EventHeaderUrlKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(EventHeaderUrlKey, value); }
        }

        private const string CompanyHeaderUrlKey = "CompanyHeader";
        public static string CompanyHeaderUrl
        {
            get { return AppSettings.GetValueOrDefault(CompanyHeaderUrlKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(CompanyHeaderUrlKey, value); }
        }

        private const string GroupsHeaderUrlKey = "GroupsHeader";
        public static string GroupsHeaderUrl
        {
            get { return AppSettings.GetValueOrDefault(GroupsHeaderUrlKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(GroupsHeaderUrlKey, value); }
        }

        #endregion Image

        #region Translations

        private const string SelectedLanguageIdKey = "SelectedLanguageId";
        public static Guid SelectedLanguageId
        {
            get { return AppSettings.GetValueOrDefault(SelectedLanguageIdKey, GuidDefault); }
            set { AppSettings.AddOrUpdateValue(SelectedLanguageIdKey, value); }
        }

        private const string SelectedLanguageKey = "SelectedLanguage";
        public static LanguageModel SelectedLanguage
        {
            get
            {
                var modelstring = AppSettings.GetValueOrDefault(SelectedLanguageKey, StringDefault);

                if (modelstring == null)
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<LanguageModel>(modelstring);
            }
            set
            {
                string result = null;

                if (value != null)
                {
                    result = JsonConvert.SerializeObject(value);
                }

                AppSettings.AddOrUpdateValue(SelectedLanguageKey, result);
            }
        }

        private const string DefaultLanguageKey = "DefaultLanguageKey";
        public static LanguageModel DefaultLanguage
        {
            get
            {
                var modelstring = AppSettings.GetValueOrDefault(DefaultLanguageKey, StringDefault);

                if (modelstring == null)
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<LanguageModel>(modelstring);
            }
            set
            {
                string result = null;

                if (value != null)
                {
                    result = JsonConvert.SerializeObject(value);
                }

                AppSettings.AddOrUpdateValue(DefaultLanguageKey, result);
            }
        }

        public static string GetResource(string key)
        {
            // Try selected language
            var translation = FindTranslation(SelectedLanguage, key);

            if (translation != null)
            {
                return translation;
            }

            // Try default language
            translation = FindTranslation(DefaultLanguage, key);

            if (translation != null)
            {
                return translation;
            }

            // Language not found, or no translation found in language or default language
            return key;
        }

        private static string FindTranslation(LanguageModel languageModel, string key)
        {
            // Get translation
            var translation = languageModel?.Translations?.FirstOrDefault(t => t.Key == key);

            if (translation != null)
            {
                // Check override value
                if (!string.IsNullOrWhiteSpace(translation.OverrideValue))
                    return translation.OverrideValue;

                // Check default value
                if (!string.IsNullOrWhiteSpace(translation.DefaultValue))
                    return translation.DefaultValue;
            }

            return null;
        }

        #endregion Get

        #region Platform

        private const string NotificationConnectionStringKey = "NotificationConnectionString";
        public static string NotificationConnectionString
        {
            get { return AppSettings.GetValueOrDefault(NotificationConnectionStringKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(NotificationConnectionStringKey, value); }
        }

        private const string NotificationHubPathkey = "NotificationHubPath";
        public static string NotificationHubPath
        {
            get { return AppSettings.GetValueOrDefault(NotificationHubPathkey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(NotificationHubPathkey, value); }
        }

        private const string AmPmNotationPathkey = "AmPmNotation";
        public static bool AmPmNotation
        {
            get { return AppSettings.GetValueOrDefault(AmPmNotationPathkey, BoolDefault); }
            set { AppSettings.AddOrUpdateValue(AmPmNotationPathkey, value); }
        }

        private const string TimeFormatkey = "TimeFormat";
        public static string TimeFormat
        {
            get { return AppSettings.GetValueOrDefault(TimeFormatkey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(TimeFormatkey, value); }
        }

        private const string DateFormatKey = "DateFormat";
        public static string DateFormat
        {
            get { return AppSettings.GetValueOrDefault(DateFormatKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(DateFormatKey, value); }
        }

        private const string SenderIdKey = "SenderId";
        public static string SenderId
        {
            get { return AppSettings.GetValueOrDefault(SenderIdKey, StringDefault); }
            set { AppSettings.AddOrUpdateValue(SenderIdKey, value); }
        }

        private const string AccessControlKey = "AccessControl";
        public static bool AccessControlEnabled
        {
            get { return AppSettings.GetValueOrDefault(AccessControlKey, BoolDefault); }
            set { AppSettings.AddOrUpdateValue(AccessControlKey, value); }
        }

        private const string ShowExternalUrlWarningKey = "ShowExternalUrlWarning";
        public static bool ShowExternalUrlWarning
        {
            get { return AppSettings.GetValueOrDefault(ShowExternalUrlWarningKey, BoolDefault); }
            set { AppSettings.AddOrUpdateValue(ShowExternalUrlWarningKey, value); }
        }

        #endregion Platform

        #region SecurityGroups

        private const string UserRolesKey = "UserRoles";
        public static List<string> UserRoles
        {
            get { return AppSettings.GetValueOrDefault(UserRolesKey, StringDefault).Split(';').ToList(); }
            set { AppSettings.AddOrUpdateValue(UserRolesKey, string.Join(";", value)); }
        }

        private const string CustomLoginKey = "CustomLogin";
        public static bool CustomLogin
        {
            get { return AppSettings.GetValueOrDefault(CustomLoginKey, BoolDefault); }
            set { AppSettings.AddOrUpdateValue(CustomLoginKey, value); }
        }

        #endregion SecurityGroups
    }
}