using System;
using System.Collections.Generic;
using Bisner.ApiModels.General;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.ApiModels.Central
{
    public class ApiCentralPlatformSecurityGroupSettingsModel
    {
        public Guid DefaultRegisteredGroup { get; set; }

        public Guid DefaultInvitedGroup { get; set; }
    }

    public class ApiCentralPlatformEmailSettingsModel
    {
        /// <summary>
        /// From email text
        /// </summary>
        public string EmailFromText { get; set; }

        /// <summary>
        /// From email email adres
        /// </summary>
        public string EmailFromEmail { get; set; }

        /// <summary>
        /// SMTP Host
        /// </summary>
        public string EmailCustomSMTPHost { get; set; }

        /// <summary>
        /// SMTP Port
        /// </summary>
        public int EmailCustomSMTPPort { get; set; }

        /// <summary>
        /// SMTP SSL enabled
        /// </summary>
        public bool EmailCustomSMTPEnableSSL { get; set; }

        /// <summary>
        /// SMTP Username
        /// </summary>
        public string EmailCustomSMTPUsername { get; set; }

        /// <summary>
        /// SMTP Password
        /// </summary>
        public string EmailCustomSMTPPassword { get; set; }
    }

    public enum Layout
    {
        Default = 0,
        Cobot = 1,
        Deskpass = 2
    }

    public class ApiCentralPlatformStylesheet
    {
        /// <summary>
        /// Load styling default values
        /// </summary>
        public void LoadDefault()
        {
            UseWhiteHeader = false;
            FontColor = "#444";
            Font = "Calibri,Arial,sans-serif";
            LinkColor = "#24ADDB";
            SidebarBackdrop = "#1C4152";
            ModalBackdrop = "#1C4152";
            ButtonColor = "#63C1AC";
            ButtonHoverColor = "#48B79E";
            ButtonFont = "'Lato', sans-serif";
            ButtonRadiusRounded = "37px";
            ButtonRadiusNormal = "4px";
            NavigationBackgroundColor = "#175366";
            NavigationFontSize = "14px";
            NavigationFont = "'Lato', sans-serif";
            NavigationButtonRadius = "30px";
            HeadingFontColor = "#444";
            HeadingFont = "'Lato', sans-serif";
            MainColor1 = "#24ADDB";
            MainColor2 = "#444";
            MainColor3 = "#63C1AC";
            MainColor4 = "#5988AA";
            FeedTitleUserFont = "'Lato', sans-serif";
            FeedTitleUserFontWeight = "bolder";
            FeedTitleUserFontSize = "14px";
            FeedPostFontSize = "14px";
            FeedPostLinkColor = "#24ADDB";
            MemberCardTitleFontColor = "#444";
            EventDateAvatarBackgroundColor = "#7589AA";
            SidebarSortListFont = "'Lato', sans-serif";
            SidebarSortListFontColor = "#444";
            CheckNull();
        }

        public void CheckNull()
        {
            if (string.IsNullOrWhiteSpace(BodyBackgroundImage))
            {
                BodyBackgroundImage = "";
            }


            if (string.IsNullOrWhiteSpace(NavigationFontColorNormal))
            {
                NavigationFontColorNormal = "rgba(255,255,255,0.6) !important";
            }
            if (string.IsNullOrWhiteSpace(NavigationFontColorHover))
            {
                NavigationFontColorHover = "rgba(255,255,255,1.0) !important";
            }

            if (string.IsNullOrWhiteSpace(NavigationFontColorActive))
            {
                NavigationFontColorActive = "rgba(255,255,255,1.0) !important";
            }

            if (string.IsNullOrWhiteSpace(SubNavigationLinkBackgroundNormal))
            {
                SubNavigationLinkBackgroundNormal = "rgba(0,0,0,0.0);";
            }
            if (string.IsNullOrWhiteSpace(SubNavigationLinkBackgroundHover))
            {
                SubNavigationLinkBackgroundHover = "rgba(0,0,0,0.2);";
            }

            if (string.IsNullOrWhiteSpace(SubNavigationLinkBackgroundActive))
            {
                SubNavigationLinkBackgroundActive = "rgba(0,0,0,0.0)";
            }


            if (string.IsNullOrWhiteSpace(SubNavigationTextAlign))
            {
                SubNavigationTextAlign = "center";
            }
            if (string.IsNullOrWhiteSpace(SubNavigationPadding))
            {
                SubNavigationPadding = "0 18px";
            }
            if (string.IsNullOrWhiteSpace(SubNavigationLineheight))
            {
                SubNavigationLineheight = "27px";
            }
            if (string.IsNullOrWhiteSpace(SubNavigationMargin))
            {
                SubNavigationMargin = "0 10px 0 0";
            }

            if (string.IsNullOrWhiteSpace(BackGroundColor))
            {
                BackGroundColor = "#F9F9F9";
            }
            if (string.IsNullOrWhiteSpace(ChatHeadingColor))
            {
                ChatHeadingColor = "rgba(0,0,0,0,0.0)";
            }
            if (string.IsNullOrWhiteSpace(ChatHeadingHoverColor))
            {
                ChatHeadingHoverColor = "rgba(0,0,0,0.04)";
            }
            if (string.IsNullOrWhiteSpace(ChatHeadingFontColor))
            {
                ChatHeadingFontColor = "#555";
            }
            if (string.IsNullOrWhiteSpace(BackgroundIntroLoader))
            {
                BackgroundIntroLoader = "#0D2F39";
            }
            if (string.IsNullOrWhiteSpace(Spinner))
            {
                Spinner = "40,174,219";
            }
            if (string.IsNullOrWhiteSpace(SpinnerIntroLoader))
            {
                SpinnerIntroLoader = "255,255,255";
            }
            if (string.IsNullOrWhiteSpace(ProgressBar))
            {
                ProgressBar = "#28AEDB";
            }

            if (string.IsNullOrWhiteSpace(MainNotificationColor))
            {
                MainNotificationColor = "#FE735D";
            }
            if (string.IsNullOrWhiteSpace(SubNotificationColor))
            {
                SubNotificationColor = "#5988aa";
            }

            if (string.IsNullOrWhiteSpace(ButtonColor2))
            {
                ButtonColor2 = "#28AEDB";
            }
            if (string.IsNullOrWhiteSpace(ButtonHoverColor2))
            {
                ButtonHoverColor2 = "#2291BF";
            }


            if (string.IsNullOrWhiteSpace(Headerlargefontweight))
            {
                Headerlargefontweight = "900";
            }
            if (string.IsNullOrWhiteSpace(Headerlargemargin))
            {
                Headerlargemargin = "0";
            }
            if (string.IsNullOrWhiteSpace(Subheaderfontsize))
            {
                Subheaderfontsize = "25px";
            }
            if (string.IsNullOrWhiteSpace(Subheaderfontfamily))
            {
                Subheaderfontfamily = "'Times New Roman', Times, serif";
            }
            if (string.IsNullOrWhiteSpace(Subheaderfontweight))
            {
                Subheaderfontweight = "700";
            }
        }

        public string CustomHeadTags { get; set; } = "";

        public string NavigationFontColorNormal { get; set; }

        public string NavigationFontColorHover { get; set; }
        public string NavigationFontColorActive { get; set; }

        public string SubNavigationLinkBackgroundNormal { get; set; }
        public string SubNavigationLinkBackgroundHover { get; set; }
        public string SubNavigationLinkBackgroundActive { get; set; }


        public string SubNavigationTextAlign { get; set; }
        public string SubNavigationPadding { get; set; }
        public string SubNavigationLineheight { get; set; }
        public string SubNavigationMargin { get; set; }

        public string Headerlargefontweight { get; set; }
        public string Headerlargemargin { get; set; }
        public string Subheaderfontsize { get; set; }
        public string Subheaderfontfamily { get; set; }
        public string Subheaderfontweight { get; set; }


        public string BodyBackgroundImage { get; set; }

        /// <summary>
        /// White headers
        /// </summary>
        public bool UseWhiteHeader { get; set; }

        /// <summary>
        /// Layout to use for the website
        /// </summary>
        public Layout Layout { get; set; }

        /// <summary>
        /// General background color
        /// </summary>
        public string BackGroundColor { get; set; }

        /// <summary>
        /// Default font color
        /// </summary>
        public string FontColor { get; set; }

        /// <summary>
        /// Default font faces
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        /// Link color
        /// </summary>
        public string LinkColor { get; set; }

        /// <summary>
        /// Sidebar backdrop color when open
        /// </summary>
        public string SidebarBackdrop { get; set; }

        /// <summary>
        /// Modal backdrop color when open
        /// </summary>
        public string ModalBackdrop { get; set; }

        /// <summary>
        /// Button color
        /// </summary>
        public string ButtonColor { get; set; }
        public string ButtonColor2 { get; set; }

        /// <summary>
        /// Button hover color
        /// </summary>
        public string ButtonHoverColor { get; set; }
        public string ButtonHoverColor2 { get; set; }

        /// <summary>
        /// Button font
        /// </summary>
        public string ButtonFont { get; set; }

        /// <summary>
        /// Radius of a rounded button
        /// </summary>
        public string ButtonRadiusRounded { get; set; }

        /// <summary>
        /// Radius of a normal button
        /// </summary>
        public string ButtonRadiusNormal { get; set; }

        /// <summary>
        /// Background color of the navigation bar
        /// </summary>
        public string NavigationBackgroundColor { get; set; }

        /// <summary>
        /// Font size of the text in the navigation bar
        /// </summary>
        public string NavigationFontSize { get; set; }

        /// <summary>
        /// Font face of the text in the navigation bar
        /// </summary>
        public string NavigationFont { get; set; }

        /// <summary>
        /// Radius of the buttons in the navigation bar
        /// </summary>
        public string NavigationButtonRadius { get; set; }

        /// <summary>
        /// Headings font color
        /// </summary>
        public string HeadingFontColor { get; set; }

        /// <summary>
        /// Headings font face
        /// </summary>
        public string HeadingFont { get; set; }


        public string MainColor1 { get; set; }
        public string MainColor2 { get; set; }
        public string MainColor3 { get; set; }
        public string MainColor4 { get; set; }

        /// <summary>
        /// Feed font settings
        /// </summary>
        public string FeedTitleUserFont { get; set; }
        public string FeedTitleUserFontWeight { get; set; }
        public string FeedTitleUserFontSize { get; set; }

        public string FeedPostFontSize { get; set; }

        public string FeedPostLinkColor { get; set; }

        public string MemberCardTitleFontColor { get; set; }

        public string EventDateAvatarBackgroundColor { get; set; }

        public string SidebarSortListFont { get; set; }
        public string SidebarSortListFontColor { get; set; }

        public string ChatHeadingColor { get; set; }
        public string ChatHeadingHoverColor { get; set; }
        public string ChatHeadingFontColor { get; set; }

        public string BackgroundIntroLoader { get; set; }

        public string SpinnerIntroLoader { get; set; }
        public string Spinner { get; set; }
        public string ProgressBar { get; set; }

        public string MainNotificationColor { get; set; }
        public string SubNotificationColor { get; set; }
    }

    public class ApiCentralPlatformImages
    {
        /// <summary>
        /// Welcome flow / login page image 1
        /// </summary>
        public ApiImageModel WelcomeImage1 { get; set; }

        /// <summary>
        /// Welcome flow / login page image 2
        /// </summary>
        public ApiImageModel WelcomeImage2 { get; set; }

        /// <summary>
        /// Welcome flow / login page image 3
        /// </summary>
        public ApiImageModel WelcomeImage3 { get; set; }

        /// <summary>
        /// Members index header
        /// </summary>
        public ApiImageModel MembersHeader { get; set; }

        /// <summary>
        /// Events index header
        /// </summary>
        public ApiImageModel EventsHeader { get; set; }

        /// <summary>
        /// Groups index header
        /// </summary>
        public ApiImageModel GroupsHeader { get; set; }

        /// <summary>
        /// Location index header
        /// </summary>
        public ApiImageModel LocationHeader { get; set; }

        /// <summary>
        /// Company card header
        /// </summary>
        public ApiImageModel CompanyCardHeader { get; set; }

        /// <summary>
        /// User card header
        /// </summary>
        public ApiImageModel UserCardHeader { get; set; }

        /// <summary>
        /// Event header
        /// </summary>
        public ApiImageModel EventHeader { get; set; }

        /// <summary>
        /// Default images
        /// </summary>
        public const string UserDefault = "/Content/Dashboard/Assets/Img/Avatars/default_user_avatar.png";
        public const string CompanyHeaderDefault = "/Content/Dashboard/Assets/Img/Temp/community_header_002.png";
        public const string MembersHeaderDefault = "/Content/Dashboard/Assets/Img/Temp/community_header_001.png";
        public const string EventsHeaderDefault = "/Content/Dashboard/Assets/Img/Temp/group_header_006.png";
        public const string GroupsHeaderDefault = "/Content/Dashboard/Assets/Img/Temp/group_header_001.png";
        public const string LocationHeaderDefault = "/Content/Dashboard/Assets/Img/Temp/group_header_001.png";
        public const string RoomHeaderDefault = "/Content/Dashboard/Assets/Img/Temp/temp_header_event2.png";
    }

    public class PlatformCustomMenuModel
    {
        private List<PlatformCustomMenuItemModel> _customItems;
        public bool Home { get; set; }

        public bool Members { get; set; }

        public bool Events { get; set; }

        public bool Groups { get; set; }

        public bool Booking { get; set; }

        public bool BetaRedirect { get; set; }

        public List<PlatformCustomMenuItemModel> CustomItems
        {
            get { return _customItems ?? (_customItems = new List<PlatformCustomMenuItemModel>()); }
            set { _customItems = value; }
        }
    }

    public class PlatformCustomMenuItemModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string PageName { get; set; }

        public string PageId { get; set; }

        public bool OpenInNewWindow { get; set; }

        public bool OpenInIframe { get; set; }

        public bool InternalLink { get; set; }

        public int Order { get; set; }

        public bool ShowOnWeb { get; set; }

        public bool ShowOnMobile { get; set; }

        public ApiImageModel Icon { get; set; }
    }

    public enum PlatformInstallationStatus
    {
        Unkown = 0,
        New = 1,
        Installing = 2,
        Done = 4
    }

    /// <summary>
    /// Platform domains
    /// </summary>
    public class DomainSettings
    {
        /// <summary>
        /// Gets or sets the name of the domain.
        /// </summary>
        /// <value>
        /// The name of the domain.
        /// </value>
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the certificate thumbprint.
        /// </summary>
        /// <value>
        /// The certificate thumbprint.
        /// </value>
        public string CertificateThumbprint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DomainSettings"/> is default.
        /// </summary>
        /// <value>
        ///   <c>true</c> if default; otherwise, <c>false</c>.
        /// </value>
        public bool Default { get; set; }

        public string SubjectName { get; set; }
    }

    public enum PlatformIntegrationTypes
    {
        Cobot = 1,
        Nexudus = 2,
        PayPal = 3,
        Facebook = 4,
        Google = 5,
        Xero = 6,
        LinkedIn = 7,
        Twitter = 8,
        Microsoft = 9,
        Roomzilla = 10,
    }

    public class PlatformSettings
    {
        /// <summary>
        /// Gets or sets the notification connectionstring.
        /// </summary>
        /// <value>
        /// The notification connectionstring.
        /// </value>
        public string NotificationFullSharedConnectionString { get; set; }

        /// <summary>
        /// Listen shared key
        /// </summary>
        public string NotificationListenSharedConnectionString { get; set; }

        /// <summary>
        /// Google sender id
        /// </summary>
        public string GoogleSenderId { get; set; }

        /// <summary>
        /// Gets or sets the notification hub.
        /// </summary>
        /// <value>
        /// The notification hub.
        /// </value>
        public string NotificationHub { get; set; }
    }

    public class PlatformTimeZones
    {
        /// <summary>
        /// Time zone id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Timezone offset
        /// </summary>
        public int UTCOffset { get; set; }

        /// <summary>
        /// Timezone displayname
        /// </summary>
        public string DisplayName { get; set; }
    }

    public class PlatformGroupSettings
    {
        /// <summary>
        /// If true users can create private groups
        /// </summary>
        public bool AllowUserPrivateGroups { get; set; }

        /// <summary>
        /// If true users can create hidden groups
        /// </summary>
        public bool AllowUserHiddenGroups { get; set; }

        /// <summary>
        /// Allow sub groups
        /// </summary>
        public bool AllowSubGroups { get; set; }
    }

    public class PlatformCustomFields
    {
        public void CheckNull()
        {
            if (CustomField1 == null) { CustomField1 = new PlatformCustomFieldModel(); }
            if (CustomField2 == null) { CustomField2 = new PlatformCustomFieldModel(); }
            if (CustomField3 == null) { CustomField3 = new PlatformCustomFieldModel(); }
            if (CustomField4 == null) { CustomField4 = new PlatformCustomFieldModel(); }
            if (CustomField5 == null) { CustomField5 = new PlatformCustomFieldModel(); }
            if (CustomField6 == null) { CustomField6 = new PlatformCustomFieldModel(); }
            if (CustomField7 == null) { CustomField7 = new PlatformCustomFieldModel(); }
            if (CustomField8 == null) { CustomField8 = new PlatformCustomFieldModel(); }
            if (CustomField9 == null) { CustomField9 = new PlatformCustomFieldModel(); }
            if (CustomField10 == null) { CustomField10 = new PlatformCustomFieldModel(); }
            if (CustomField11 == null) { CustomField11 = new PlatformCustomFieldModel(); }
            if (CustomField12 == null) { CustomField12 = new PlatformCustomFieldModel(); }
            if (CustomField13 == null) { CustomField13 = new PlatformCustomFieldModel(); }
            if (CustomField14 == null) { CustomField14 = new PlatformCustomFieldModel(); }
            if (CustomField15 == null) { CustomField15 = new PlatformCustomFieldModel(); }
            if (CustomField16 == null) { CustomField16 = new PlatformCustomFieldModel(); }
            if (CustomField17 == null) { CustomField17 = new PlatformCustomFieldModel(); }
            if (CustomField18 == null) { CustomField18 = new PlatformCustomFieldModel(); }
            if (CustomField19 == null) { CustomField19 = new PlatformCustomFieldModel(); }
            if (CustomField20 == null) { CustomField20 = new PlatformCustomFieldModel(); }
            if (CustomField21 == null) { CustomField21 = new PlatformCustomFieldModel(); }
            if (CustomField22 == null) { CustomField22 = new PlatformCustomFieldModel(); }
            if (CustomField23 == null) { CustomField23 = new PlatformCustomFieldModel(); }
            if (CustomField24 == null) { CustomField24 = new PlatformCustomFieldModel(); }
            if (CustomField25 == null) { CustomField25 = new PlatformCustomFieldModel(); }
            if (CustomField26 == null) { CustomField26 = new PlatformCustomFieldModel(); }
            if (CustomField27 == null) { CustomField27 = new PlatformCustomFieldModel(); }
            if (CustomField28 == null) { CustomField28 = new PlatformCustomFieldModel(); }
            if (CustomField29 == null) { CustomField29 = new PlatformCustomFieldModel(); }
            if (CustomField30 == null) { CustomField30 = new PlatformCustomFieldModel(); }

        }
        public PlatformCustomFieldModel CustomField1 { get; set; }
        public PlatformCustomFieldModel CustomField2 { get; set; }
        public PlatformCustomFieldModel CustomField3 { get; set; }
        public PlatformCustomFieldModel CustomField4 { get; set; }
        public PlatformCustomFieldModel CustomField5 { get; set; }
        public PlatformCustomFieldModel CustomField6 { get; set; }
        public PlatformCustomFieldModel CustomField7 { get; set; }
        public PlatformCustomFieldModel CustomField8 { get; set; }
        public PlatformCustomFieldModel CustomField9 { get; set; }
        public PlatformCustomFieldModel CustomField10 { get; set; }
        public PlatformCustomFieldModel CustomField11 { get; set; }
        public PlatformCustomFieldModel CustomField12 { get; set; }
        public PlatformCustomFieldModel CustomField13 { get; set; }
        public PlatformCustomFieldModel CustomField14 { get; set; }
        public PlatformCustomFieldModel CustomField15 { get; set; }
        public PlatformCustomFieldModel CustomField16 { get; set; }
        public PlatformCustomFieldModel CustomField17 { get; set; }
        public PlatformCustomFieldModel CustomField18 { get; set; }
        public PlatformCustomFieldModel CustomField19 { get; set; }
        public PlatformCustomFieldModel CustomField20 { get; set; }
        public PlatformCustomFieldModel CustomField21 { get; set; }
        public PlatformCustomFieldModel CustomField22 { get; set; }
        public PlatformCustomFieldModel CustomField23 { get; set; }
        public PlatformCustomFieldModel CustomField24 { get; set; }
        public PlatformCustomFieldModel CustomField25 { get; set; }
        public PlatformCustomFieldModel CustomField26 { get; set; }
        public PlatformCustomFieldModel CustomField27 { get; set; }
        public PlatformCustomFieldModel CustomField28 { get; set; }
        public PlatformCustomFieldModel CustomField29 { get; set; }
        public PlatformCustomFieldModel CustomField30 { get; set; }

        public bool Enabled => CustomField1.Enabled
                               || CustomField2.Enabled
                               || CustomField3.Enabled
                               || CustomField4.Enabled
                               || CustomField5.Enabled
                               || CustomField6.Enabled
                               || CustomField7.Enabled
                               || CustomField8.Enabled
                               || CustomField9.Enabled
                               || CustomField10.Enabled
                               || CustomField11.Enabled
                               || CustomField12.Enabled
                               || CustomField13.Enabled
                               || CustomField14.Enabled
                               || CustomField15.Enabled
                               || CustomField16.Enabled
                               || CustomField17.Enabled
                               || CustomField18.Enabled
                               || CustomField19.Enabled
                               || CustomField20.Enabled
                               || CustomField21.Enabled
                               || CustomField22.Enabled
                               || CustomField23.Enabled
                               || CustomField24.Enabled
                               || CustomField25.Enabled
                               || CustomField26.Enabled
                               || CustomField27.Enabled
                               || CustomField28.Enabled
                               || CustomField29.Enabled
                               || CustomField30.Enabled;

        public bool UsedInWelcomeFlow => CustomField1.UseInWelcomeFlow
                                         || CustomField2.UseInWelcomeFlow
                                         || CustomField3.UseInWelcomeFlow
                                         || CustomField4.UseInWelcomeFlow
                                         || CustomField5.UseInWelcomeFlow
                                         || CustomField6.UseInWelcomeFlow
                                         || CustomField7.UseInWelcomeFlow
                                         || CustomField8.UseInWelcomeFlow
                                         || CustomField9.UseInWelcomeFlow
                                         || CustomField10.UseInWelcomeFlow
                                         || CustomField11.UseInWelcomeFlow
                                         || CustomField12.UseInWelcomeFlow
                                         || CustomField13.UseInWelcomeFlow
                                         || CustomField14.UseInWelcomeFlow
                                         || CustomField15.UseInWelcomeFlow
                                         || CustomField16.UseInWelcomeFlow
                                         || CustomField17.UseInWelcomeFlow
                                         || CustomField18.UseInWelcomeFlow
                                         || CustomField19.UseInWelcomeFlow
                                         || CustomField20.UseInWelcomeFlow
                                         || CustomField21.UseInWelcomeFlow
                                         || CustomField22.UseInWelcomeFlow
                                         || CustomField23.UseInWelcomeFlow
                                         || CustomField24.UseInWelcomeFlow
                                         || CustomField25.UseInWelcomeFlow
                                         || CustomField26.UseInWelcomeFlow
                                         || CustomField27.UseInWelcomeFlow
                                         || CustomField28.UseInWelcomeFlow
                                         || CustomField29.UseInWelcomeFlow
                                         || CustomField30.UseInWelcomeFlow;
    }

    public class PlatformCustomFieldModel
    {
        /// <summary>
        /// Field enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Use this field in the welcome flow
        /// </summary>
        public bool UseInWelcomeFlow { get; set; }

        /// <summary>
        /// If true use a textarea else an input field
        /// </summary>
        public bool IsTextArea { get; set; }
    }

    public class OnboardingFlowModel
    {
        #region Welcome

        public bool ExternalAccounts { get; set; }

        public bool AddCompany { get; set; }

        public bool PersonalQuestions { get; set; }

        public bool TermsAndConditions { get; set; }

        /// <summary>
        /// Show the avatar page
        /// </summary>
        public bool Avatar { get; set; }

        /// <summary>
        /// Show address page
        /// </summary>
        public bool Address { get; set; }

        /// <summary>
        /// Show about page
        /// </summary>
        public bool About { get; set; }

        /// <summary>
        /// Show skills page
        /// </summary>
        public bool Skills { get; set; }

        /// <summary>
        /// Comma seperated list of prefilled skills that will show during the welcome flow
        /// </summary>
        [Obsolete(message: "Moved to MemberSettings")]
        public string PrefilledSkills { get; set; }

        /// <summary>
        /// Show select groups page
        /// </summary>
        public bool Groups { get; set; }

        /// <summary>
        /// Show custom fields page
        /// </summary>
        public bool CustomFields { get; set; }

        #endregion Welcome

        #region Register

        /// <summary>
        /// Turn user registration on or off
        /// </summary>
        public bool AllowRegistration { get; set; }

        /// <summary>
        /// When True all users that register will be a community user instead of gues, and the Admins do not need to approve the user for the welcome email to be send
        /// </summary>
        public bool RegisterSkipAdminApproval { get; set; }

        /// <summary>
        /// Show Address step on register flow
        /// </summary>
        public bool RegisterAddressEnabled { get; set; }

        /// <summary>
        /// Show payment plan and method steps in register flow
        /// </summary>
        public bool RegisterPaymentPlansEnabled { get; set; }

        #endregion Register
    }

    public class MemberSettings
    {
        public bool AllowAllUsersToChat { get; set; }

        public bool AllowUsersToInviteMembers { get; set; }

        public bool AllowUserToUserInvite { get; set; }

        public bool DisableUsersPostingMainFeed { get; set; }

        public bool UseFreeSkills { get; set; }

        public bool UsePredefinedSkills { get; set; }

        /// <summary>
        /// Comma seperated list of prefilled skills that will show during the welcome flow
        /// </summary>
        public string PrefilledSkills { get; set; } = "";

        public bool ShowExternalLinkWarning { get; set; }

        public List<Guid> PublicLandingPageSecurityGroupIds { get; set; } = new List<Guid>();
    }

    public class ApiCentralPlatformModel : ApiCentralPublicPlatformModel
    {
        #region Fields

        private List<DomainSettings> _domainSettings;

        #endregion Fields

        #region Info

        /// <summary>
        /// If set to true this is a live paying platform -> false = demo
        /// </summary>
        public bool IsLive { get; set; }

        /// <summary>
        /// Set to true to disable api and login for all users
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Version string like .net (xx.xx.xx.xxxx)
        /// </summary>
        public string Version { get; set; }

        #endregion Info

        #region Settings

        /// <summary>
        /// If not null all login calls will be redirected to this page
        /// </summary>
        public string CustomLoginPageUrl { get; set; }

        /// <summary>
        /// Send a notification if someone makes a post on the main feed
        /// </summary>
        public bool SendNotificationOnNewFeedPost { get; set; }

        /// <summary>
        /// External css file to be loaded
        /// </summary>
        public string ExternalCss { get; set; }

        /// <summary>
        /// Send a notification if someone makes a comment on a post on the main feed
        /// </summary>
        public bool SendNotificationOnNewFeedPostComment { get; set; }

        /// <summary>
        /// From email text
        /// </summary>
        public string EmailFromText { get; set; }

        /// <summary>
        /// From email email adres
        /// </summary>
        public string EmailFromEmail { get; set; }

        /// <summary>
        /// Show members first on the member tab
        /// </summary>
        /// <value>
        ///   <c>true</c> to show members first; otherwise, <c>false</c>.
        /// </value>
        public bool ShowMembersFirst { get; set; }

        /// <summary>
        /// Enables price information and payments on events
        /// </summary>
        public bool AllowPaidEvents { get; set; }

        /// <summary>
        /// Blob container name for this platform
        /// </summary>
        public string BlobContainerName { get; set; }

        /// <summary>
        /// Database name for platform
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Platform URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Status of the platform
        /// </summary>
        public PlatformInstallationStatus InstallationStatus { get; set; }

        #endregion Settings

        #region Custom lists

        /// <summary>
        /// Returns a list of languages without the resources
        /// </summary>
        public List<LanguageModel> LanguagesOnly
        {
            get
            {
                List<LanguageModel> models = new List<LanguageModel>();

                if (Languages != null)
                {
                    foreach (LanguageModel centralLanguageModel in Languages)
                    {
                        // Only published languages
                        if (centralLanguageModel.IsPublished)
                        {
                            LanguageModel model = new LanguageModel();
                            model.Name = centralLanguageModel.Name;
                            model.Id = centralLanguageModel.Id;
                            model.Code = centralLanguageModel.Code;
                            model.IsDefault = centralLanguageModel.IsDefault;
                            models.Add(model);
                        }
                    }
                }
                return models;
            }
        }

        #endregion Custom lists

        #region Setting models

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public PlatformSettings Settings { get; set; }

        /// <summary>
        /// Group settings
        /// </summary>
        public PlatformGroupSettings GroupSettings { get; set; }
        
        /// <summary>
        /// Welcome flow pages
        /// </summary>
        public OnboardingFlowModel WelcomeFlow { get; set; }

        /// <summary>
        /// Email settings
        /// </summary>
        public ApiCentralPlatformEmailSettingsModel EmailSettings { get; set; }

        /// <summary>
        /// Security group settings
        /// </summary>
        public ApiCentralPlatformSecurityGroupSettingsModel SecurityGroupSettings { get; set; }

        /// <summary>
        /// Platform domain settings
        /// </summary>
        /// <value>
        /// The domain settings.
        /// </value>
        public List<DomainSettings> DomainSettings
        {
            get { return _domainSettings ?? (_domainSettings = new List<DomainSettings>()); }
            set { _domainSettings = value; }
        }

        #endregion Setting models
    }

    public class ApiCentralPublicPlatformModel
    {
        #region Fields

        private List<PlatformTimeZones> _timeZones;
        private List<LanguageModel> _languages;
        private List<ApiIndustryModel> _industries;
        private List<ApiCentralLocationModel> _locations;

        #endregion Fields

        #region Info

        /// <summary>
        /// Platform ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Minimal supported api version
        /// </summary>
        public int MinimalSupportedApiVersion { get; set; }

        /// <summary>
        /// Base path for all images and files
        /// </summary>
        public string CdnBasePath { get; set; }

        /// <summary>
        /// Platform created on
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Platform name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Basic or pro
        /// </summary>
        public bool IsPro { get; set; }

        #endregion Info

        #region Settings

        /// <summary>
        /// Allow collaboration on the whitelabel
        /// </summary>
        public bool AllowCollaboration { get; set; }

        /// <summary>
        /// Platform has booking system enabled
        /// </summary>
        public bool AllowBookingSystem { get; set; }

        /// <summary>
        /// Platform has public events enabled
        /// </summary>
        public bool AllowPublicEvents { get; set; }

        /// <summary>
        /// Allow digital membership
        /// </summary>
        public bool AllowDigitalMembership { get; set; }

        /// <summary>
        /// Gets or sets the notification hub.
        /// </summary>
        /// <value>
        /// The notification hub.
        /// </value>
        public string NotificationHub { get; set; }

        /// <summary>
        /// Gets or sets the notification listen shared connection string.
        /// </summary>
        /// <value>
        /// The notification listen shared connection string.
        /// </value>
        public string NotificationListenSharedConnectionString { get; set; }

        /// <summary>
        /// Google sender id
        /// </summary>
        public string GoogleSenderId { get; set; }

        /// <summary>
        /// Google analytics code
        /// </summary>
        public string UACode { get; set; }

        #endregion Settings

        #region Custom lists

        /// <summary>
        /// List of supported languages
        /// </summary>
        public List<LanguageModel> Languages
        {
            get { return _languages ?? (_languages = new List<LanguageModel>()); }
            set { _languages = value; }
        }

        /// <summary>
        /// Platform industries
        /// </summary>
        public List<ApiIndustryModel> Industries
        {
            get { return _industries ?? (_industries = new List<ApiIndustryModel>()); }
            set { _industries = value; }
        }

        /// <summary>
        /// Platform locations
        /// </summary>
        public List<ApiCentralLocationModel> Locations
        {
            get { return _locations ?? (_locations = new List<ApiCentralLocationModel>()); }
            set { _locations = value; }
        }

        /// <summary>
        /// Platform event categories
        /// </summary>
        public List<ApiCentralEventCategoryModel> EventCategories { get; set; }

        /// <summary>
        /// Platform event types
        /// </summary>
        public List<ApiCentralEventCategoryModel> EventTypes { get; set; }

        #endregion Custom Lists

        #region Time settings

        public List<PlatformTimeZones> TimeZones
        {
            get { return _timeZones ?? (_timeZones = new List<PlatformTimeZones>()); }
            set { _timeZones = value; }
        }

        public string TimeZoneId { get; set; }

        /// <summary>
        /// Date string (Default = "dddd, MMMM Do, YYYY")
        /// </summary>
        public string DateString { get; set; }

        /// <summary>
        /// Date string (Default = "dd/MM")
        /// </summary>
        public string ShortDateString { get; set; }

        /// <summary>
        /// Use am / pm notation intead of 24h
        /// </summary>
        public bool ShowAmPmNotation { get; set; }

        /// <summary>
        /// Time string (Default = "HH:mm")
        /// </summary>
        public string TimeString { get; set; }

        #endregion Time settings

        #region Member settings

        /// <summary>
        /// Allow all users to chat with eachother or only contacts
        /// </summary>
        [Obsolete(message: "Moved to MemberSettings")]
        public bool AllowAllUsersToChat { get; set; }

        /// <summary>
        /// If true group admins and event creators can send out invites
        /// </summary>
        [Obsolete(message: "Moved to MemberSettings")]
        public bool AllowUsersToInviteMembers { get; set; }

        /// <summary>
        /// If true users can invite other users into the platform
        /// </summary>
        [Obsolete(message: "Moved to MemberSettings")]
        public bool AllowUserToUserInvite { get; set; }

        /// <summary>
        /// Disable option for non admins to post on the main feed
        /// </summary>
        [Obsolete(message: "Moved to MemberSettings")]
        public bool DisableUsersPostingMainFeed { get; set; }

        #endregion Members settings                     

        #region Images

        /// <summary>
        /// Custom logo
        /// </summary>
        public ApiImageModel Logo { get; set; }

        /// <summary>
        /// Custom logo for login page
        /// </summary>
        public ApiImageModel LogoLogin { get; set; }

        /// <summary>
        /// Fav icon
        /// </summary>
        public ApiImageModel FavIcon { get; set; }

        /// <summary>
        /// Platform images
        /// </summary>
        public ApiCentralPlatformImages Images { get; set; }

        /// <summary>
        /// Platform styling
        /// </summary>
        public ApiCentralPlatformStylesheet Stylesheet { get; set; }

        #endregion Images

        #region Setting models

        /// <summary>
        /// Platform opening hours
        /// </summary>
        public OpeningInformation OpeningInformation { get; set; }
        
        /// <summary>
        /// Custom menu for the platform
        /// </summary>
        public PlatformCustomMenuModel CustomMenu { get; set; }

        /// <summary>
        /// Platform custom fields
        /// </summary>
        public PlatformCustomFields CustomFields { get; set; }

        /// <summary>
        /// Member settings
        /// </summary>
        public MemberSettings MemberSettings { get; set; }

        #endregion Setting models

        #region Integration settings

        /// <summary>
        /// Nexudus integration enabled or not
        /// </summary>
        public bool NexudusEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable integration booking option].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable integration booking option]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableIntegrationBookingOption { get; set; }

        public bool EnableIntegrationBookingOptionInNav { get; set; }


        public bool EnableIntegrationBookingSearchOption { get; set; }

        public bool EnableIntegrationBookingSearchOptionInNav { get; set; }


        public bool EnableIntegrationBookingCalendarOption { get; set; }

        public bool EnableIntegrationBookingCalendarOptionInNav { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable integration allowance option].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable integration allowance option]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableIntegrationAllowanceOption { get; set; }

        public bool EnableIntegrationAllowanceOptionInNav { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable integration invoices option].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable integration invoices option]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableIntegrationInvoicesOption { get; set; }
        public bool EnableIntegrationInvoicesOptionInNav { get; set; }

        /// <summary>
        /// Roomzilla integration enabled or not
        /// </summary>
        public bool RoomzillaEnabled { get; set; }

        /// <summary>
        /// True if any access control integrations are active
        /// </summary>
        public bool AccessControlEnabled { get; set; }

        #endregion Integration settings
    }
}