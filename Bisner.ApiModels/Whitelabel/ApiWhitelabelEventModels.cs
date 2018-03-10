using System;
using System.Collections.Generic;
using Bisner.ApiModels.Collaboration;
using Bisner.ApiModels.General;

namespace Bisner.ApiModels.Whitelabel
{

    public enum EventType
    {
        /// <summary>
        /// Members only event
        /// </summary>
        Members = 0,

        /// <summary>
        /// Public event (also for external people)
        /// </summary>
        Public = 1,

        /// <summary>
        /// Invite only event
        /// </summary>
        Private = 2
    }
    public class ApiWhitelabelEventUpdateModel
    {
        /// <summary>
        /// Event id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Header class
        /// </summary>
        public string HeaderClass { get; set; }

        /// <summary>
        /// Event type
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// If true show livestroom box
        /// </summary>
        public bool WillBeLiveStreamed { get; set; }

        /// <summary>
        /// If true show livestroom box
        /// </summary>
        public bool IsLiveStreaming { get; set; }

        /// <summary>
        /// Youtube link to embed as livestream
        /// </summary>
        public string YoutubeLiveStreamUrl { get; set; }

        /// <summary>
        /// Event title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Subtitle
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Summary for the feedpost
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Event details
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Comma seperated list of tags
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Datetime of the event
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// End Datetime of the event
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Location string
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// If set to true redirect user to external link when clicking rsvp button
        /// </summary>
        public bool UseExternalRSVP { get; set; }

        /// <summary>
        /// Link to redirect to
        /// </summary>
        public string ExternalRSVPLink { get; set; }

        /// <summary>
        /// Paid event
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Category id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Type id
        /// </summary>
        public Guid TypeId { get; set; }
    }

    public class ApiWhitelabelCreatedEventModel
    {
        /// <summary>
        /// New feedpost
        /// </summary>
        public ApiWhitelabelFeedPostModel FeedPost { get; set; }

        /// <summary>
        /// Event model
        /// </summary>
        public ApiWhitelabelEventModel Event { get; set; }
    }

    public class EventSpeakerModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }

        public string About { get; set; }

        public ApiImageModel Avatar { get; set; }
    }

    public class ApiWhitelabelEventModel
    {
        private List<ApiWhitelabelCommentModel> _comments;
        private List<ApiImageModel> _images;
        private List<ApiWhitelabelPublicUserModel> _attendees;
        private List<Guid> _attendeesIds;
        private List<EventSpeakerModel> _externalSpeakers;
        private List<ApiFileItemModel> _files;
        private List<Guid> _invitedUserIds;

        private List<Guid> _otherCategories;
        private List<Guid> _otherTypes;

        /// <summary>
        /// Event id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Class for the header (default header_large)
        /// </summary>
        public string HeaderClass { get; set; }

        /// <summary>
        /// Event type
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Event type
        /// </summary>
        public Guid EventTypeId { get; set; }

        /// <summary>
        /// Main event category id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Event type id
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// Is published
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// if true show is nowlivestreaming and youtube code box
        /// </summary>
        public bool WillBeLiveStreamed { get; set; }

        /// <summary>
        /// If true show livestroom box
        /// </summary>
        public bool IsLiveStreaming { get; set; }

        /// <summary>
        /// Youtube link to embed as livestream
        /// </summary>
        public string YoutubeLiveStreamUrl { get; set; }

        /// <summary>
        /// Post id
        /// </summary>
        public Guid LinkedPostedId { get; set; }

        /// <summary>
        /// Parent id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// User creating the event
        /// </summary>
        public ApiWhitelabelPublicUserModel CreatedByUser { get; set; }

        /// <summary>
        /// Event title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Seo friendly url
        /// </summary>
        public string Url => Id.ToString("N");

        /// <summary>
        /// Subtitle
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Event details
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Event Summary
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Comma seperated list of tags
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// If set to true redirect user to external link when clicking rsvp button
        /// </summary>
        public bool UseExternalRSVP { get; set; }

        /// <summary>
        /// Link to redirect to
        /// </summary>
        public string ExternalRSVPLink { get; set; }

        /// <summary>
        /// Ids of attendees
        /// </summary>
        public List<Guid> AttendeesIds
        {
            get { return _attendeesIds ?? (_attendeesIds = new List<Guid>()); }
            set { _attendeesIds = value; }
        }

        /// <summary>
        /// Users attending
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Attendees
        {
            get { return _attendees ?? (_attendees = new List<ApiWhitelabelPublicUserModel>()); }
            set { _attendees = value; }
        }

        /// <summary>
        /// Other event categories
        /// </summary>
        public List<Guid> OtherCategories
        {
            get { return _otherCategories ?? (_otherCategories = new List<Guid>()); }
            set { _otherCategories = value; }
        }
        
        /// <summary>
        /// Other event types
        /// </summary>
        public List<Guid> OtherTypes
        {
            get { return _otherTypes ?? (_otherTypes = new List<Guid>()); }
            set { _otherTypes = value; }
        }

        /// <summary>
        /// Datetime of the event
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the end date time.
        /// </summary>
        /// <value>
        /// The end date time.
        /// </value>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Creation datetime
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Location string
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Images
        /// </summary>
        public List<ApiImageModel> Images
        {
            get { return _images ?? (_images = new List<ApiImageModel>()); }
            set { _images = value; }
        }

        /// <summary>
        /// Event logo
        /// </summary>
        public ApiImageModel Logo { get; set; }

        /// <summary>
        /// Event header
        /// </summary>
        public ApiImageModel Header { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public List<ApiWhitelabelCommentModel> Comments
        {
            get { return _comments ?? (_comments = new List<ApiWhitelabelCommentModel>()); }
            set { _comments = value; }
        }

        public List<Guid> InvitedUserIds
        {
            get { return _invitedUserIds ?? (_invitedUserIds = new List<Guid>()); }
            set { _invitedUserIds = value; }
        }

        /// <summary>
        /// External speakers (Not a member)
        /// </summary>
        public List<EventSpeakerModel> ExternalSpeakers
        {
            get { return _externalSpeakers ?? (_externalSpeakers = new List<EventSpeakerModel>()); }
            set { _externalSpeakers = value; }
        }

        /// <summary>
        /// External files
        /// </summary>
        public List<ApiFileItemModel> Files
        {
            get { return _files ?? (_files = new List<ApiFileItemModel>()); }
            set { _files = value; }
        }
        /// <summary>
        /// Paid event
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency sign
        /// </summary>
        public string Currency { get; set; }

        public ApiImageModel Thumbnail { get; set; }

        /// <summary>
        /// Header used on the index
        /// </summary>
        public ApiImageModel IndexHeader { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditEventApiModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string HeaderClass { get; set; }

        public string SubTitle { get; set; }

        public string Summary { get; set; }

        public string Location { get; set; }

        public string Details { get; set; }

        public DateTime? Datetime { get; set; }

        public DateTime? Enddatetime { get; set; }

        public bool WillBeLiveStreamed { get; set; }

        public bool IsLiveStreaming { get; set; }

        public string YoutubeLiveStreamUrl { get; set; }

        public bool UseExternalRSVP { get; set; }

        public string ExternalRSVPLink { get; set; }

        public string Tags { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; }

        public EventType EventType { get; set; }

        public Guid TypeId { get; set; }
        public Guid CategoryId { get; set; }
    }
}