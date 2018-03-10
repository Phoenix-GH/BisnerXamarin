using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Bisner.ApiModels.Whitelabel;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Manage.User;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.ViewModels.Manage
{
    public enum ManageType
    {
        Default = 0,
        Language = 1,
    }

    public class AndroidManageViewModel : ManageViewModel
    {
        #region Constructor

        public AndroidManageViewModel(IPlatformService platformService, IUserService userService) : base(platformService, userService)
        {
        }

        #endregion Constructor
    }

    /// <summary>
    /// ViewModel for more tab
    /// </summary>
    public class ManageViewModel : BaseViewModel
    {
        #region Constructor

        private List<IManageItem> _items;
        private MvxCommand<IManageItem> _selectedCommand;

        private readonly IUserService _userService;
        
        private ApiWhitelabelPrivateUserModel _personalModel;

        public ManageViewModel(IPlatformService platformService, IUserService userService) : base(platformService)
        {
            _userService = userService;
        }

        #endregion Constructor

        #region Init

        public async Task Init(ManageType type)
        {
            try
            {
                _personalModel = await _userService.GetPersonalModelAsync(ApiPriority.UserInitiated);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }

            Type = type;

            Items = Type == ManageType.Default ? GetDefaultItems() : await GetLanguageItems();
        }

        #endregion Init

        #region Items

        public List<IManageItem> Items
        {
            get => _items;
            private set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public MvxCommand<IManageItem> SelectedCommand => _selectedCommand ?? (_selectedCommand = new MvxCommand<IManageItem>(ItemSelected));

        public void ItemSelected(IManageItem manageItem)
        {
            if (manageItem is ManageItem)
            {
                var action = ((ManageItem)manageItem).Action;

                action?.Invoke();
            }
        }

        #endregion Items

        #region Properties

        public ManageType Type { get; private set; }

        #endregion Properties

        #region Typeitems

        private List<IManageItem> GetDefaultItems()
        {
            return new List<IManageItem>
                {
                    new ManageUser
                    {
                        Text = _personalModel?.DisplayName,
                        ImageUrl = $"{_personalModel?.Avatar?.Small}",
                        Action = () => ShowViewModel<ProfileViewModel>()
                    },
                    new ManageItem
                    {
                        Text = GetResource(ResKeys.mobile_more_title_change_language),
                        Action = () => ShowViewModel<ManageViewModel>(new { type = ManageType.Language})
                    },
                    new ManageItem
                    {
                        Text = GetResource(ResKeys.mobile_more_btn_security_privacy),
                        Action = () => { ShowViewModel<SecurityViewModel>(); }
                    },
                    //new ManageItem { Text = "Mails & Notifications", Action = () => { ShowViewModel<MailsViewModel>(); } },
                    new ManageLabel {Text = GetResource(ResKeys.mobile_more_title_others)},
                    //new ManageItem { Text = "Help"},
                    new ManageItem
                    {
                        Text = GetResource(ResKeys.mobile_more_btn_signout),
                        Action = async () =>
                        {
                            var result = await UserDialogs.ConfirmAsync(new ConfirmConfig
                            {
                                Message = GetResource(ResKeys.mobile_more_signout_message), 
                                OkText = GetResource(ResKeys.mobile_more_btn_signout),
                                CancelText = GetResource(ResKeys.platform_btn_cancel)
                            });

                            if (result)
                            {
                                await App.LogOut();
                            }
                        }
                    },
                };
        }

        private async Task<List<IManageItem>> GetLanguageItems()
        {
            var languages = await PlatformService.GetLanguagesAsync();

            var itemList = new List<IManageItem> { new ManageLabel { Text = "Select language" } };

            itemList.AddRange(languages.Select(language => new ManageItem
            {
                Text = language.Name,
                Action = () =>
                {
                    if (Settings.SelectedLanguageId != language.Id)
                    {
                        Settings.SelectedLanguageId = language.Id;
                    }

                    switch (App.AppPlatform)
                    {
                        case AppPlatform.iOS:
                            // TODO : Workaround for iOS, because the hint changing root viewcontroller had problems with the tab bar being null
                            ShowViewModel<MainViewModel>();
                            break;
                        case AppPlatform.Android:
                            ChangePresentation(new LanguageChangedPresentationHint());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                },
            }));

            return itemList;
        }

        #endregion Typeitems
    }

    public interface IManageItem
    {
        string Text { get; set; }
    }

    public class ManageLabel : IManageItem
    {
        public string Text { get; set; }
    }

    public class ManageItem : IManageItem
    {
        public string Text { get; set; }

        public Action Action { get; set; }
    }

    public class ManageUser : ManageItem
    {
        public string ImageUrl { get; set; }
    }
}
