using System;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Service;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.WebBrowser;

namespace Bisner.Mobile.Core.Models.Feed
{
    public class CompanyFeedItem : ItemBase, IFeedItem
    {
        private ApiWhitelabelCompanyModel _company;
        private string _name, _header, _logoUrl, _industry;
        private string _about;
        private string _telephone;
        private string _location;
        private string _website;
        private string _facebook;
        private string _twitter;
        private string _linkedIn;
        private string _instagram;
        private DateTime _dateTime;
        private MvxCommand _facebookCommand;
        private MvxCommand _twitterCommand;
        private MvxCommand _linkedInCommand;
        private MvxCommand _instagramCommand;

        public CompanyFeedItem(ApiWhitelabelCompanyModel company, ApiIndustryModel industry)
        {
            Company = company;
            Name = _company.Name;
            HeaderUrl = _company.Header?.Medium;
            LogoUrl = _company.Logo?.Medium;
            Industry = industry?.Name;
            About = _company.Summary;
            Telephone = _company.Telephone;
            Website = _company.WebUrl;
            Facebook = _company.FacebookUrl;
            Twitter = _company.TwitterUrl;
            LinkedIn = _company.LinkedInUrl;
            Instagram = _company.InstagramUrl;
            DateTime = DateTime.MaxValue;
        }

        public ApiWhitelabelCompanyModel Company
        {
            get => _company;
            private set { _company = value; RaisePropertyChanged(() => Company); }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string HeaderUrl
        {
            get => _header ?? Defaults.CompanyHeaderDefaultString;
            set
            {
                _header = value;
                RaisePropertyChanged(() => HeaderUrl);
            }
        }

        public string LogoUrl
        {
            get => _logoUrl;
            set
            {
                _logoUrl = value;
                RaisePropertyChanged(() => LogoUrl);
            }
        }

        public string Industry
        {
            get => _industry;
            set
            {
                _industry = value;
                RaisePropertyChanged(() => Industry);
            }
        }

        public string AboutTitle => Settings.GetResource(ResKeys.mobile_members_company_aboutus);

        public string About
        {
            get => _about;
            set { _about = value; RaisePropertyChanged(() => About); RaisePropertyChanged(() => HasAbout); }
        }

        public bool HasAbout => !string.IsNullOrWhiteSpace(About);

        public string ContactTitle => Settings.GetResource(ResKeys.mobile_members_company_contactus);

        public string Telephone
        {
            get => _telephone;
            set { _telephone = value; RaisePropertyChanged(() => Telephone); }
        }

        public bool HasTelephone => !string.IsNullOrWhiteSpace(Telephone);

        public DateTime ItemDateTime { get; set; }

        public string Location
        {
            get => _location;
            set { _location = value; RaisePropertyChanged(() => Location); }
        }

        public bool HasLocation => !string.IsNullOrWhiteSpace(Location);

        public string Website
        {
            get => _website;
            set { _website = value; RaisePropertyChanged(() => Website); }
        }

        public bool HasWebsite => !string.IsNullOrWhiteSpace(Website);

        public bool HasContactInfo => HasTelephone || HasLocation || HasWebsite;

        public string Facebook
        {
            get => _facebook;
            set { _facebook = value; RaisePropertyChanged(() => Facebook); }
        }

        public string Twitter
        {
            get => _twitter;
            set { _twitter = value; RaisePropertyChanged(() => Twitter); }
        }

        public string LinkedIn
        {
            get => _linkedIn;
            set { _linkedIn = value; RaisePropertyChanged(() => LinkedIn); }
        }

        public string Instagram
        {
            get { return _instagram; }
            set { _instagram = value; RaisePropertyChanged(() => Instagram); }
        }

        public DateTime DateTime
        {
            get => _dateTime;
            set { _dateTime = value; RaisePropertyChanged(() => DateTime); }
        }

        #region Social

        public MvxCommand FacebookCommand
        {
            get { return _facebookCommand ?? (_facebookCommand = new MvxCommand(ShowFacebook, () => CanShowFacebook)); }
        }

        private void ShowFacebook()
        {
            Mvx.Resolve<IMvxWebBrowserTask>().ShowWebPage(Facebook);
        }

        public bool CanShowFacebook => !string.IsNullOrWhiteSpace(Facebook);

        public MvxCommand TwitterCommand
        {
            get { return _twitterCommand ?? (_twitterCommand = new MvxCommand(ShowTwitter, () => CanShowTwitter)); }
        }

        private void ShowTwitter()
        {
            Mvx.Resolve<IMvxWebBrowserTask>().ShowWebPage(Facebook);
        }

        public bool CanShowTwitter => !string.IsNullOrWhiteSpace(Twitter);

        public MvxCommand LinkedInCommand
        {
            get { return _linkedInCommand ?? (_linkedInCommand = new MvxCommand(ShowLinkedIn, () => CanShowLinkedIn)); }
        }

        private void ShowLinkedIn()
        {
            Mvx.Resolve<IMvxWebBrowserTask>().ShowWebPage(LinkedIn);
        }

        public bool CanShowLinkedIn => !string.IsNullOrWhiteSpace(LinkedIn);

        public MvxCommand InstagramCommand
        {
            get { return _instagramCommand ?? (_instagramCommand = new MvxCommand(ShowInstagram, () => CanShowInstagram)); }
        }

        private void ShowInstagram()
        {
            Mvx.Resolve<IMvxWebBrowserTask>().ShowWebPage(Instagram);
        }

        public bool CanShowInstagram => !string.IsNullOrWhiteSpace(Instagram);

        #endregion Social
    }
}