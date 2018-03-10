using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.PictureChooser;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Bisner.Mobile.Core.ViewModels.Feed
{
    public class AddPostViewModel : MentionViewModelBase
    {
        #region Constructor

        // Backing fields
        private string _input;
        private MvxCommand _sendCommand;

        private readonly IUserService _userService;
        private readonly IFeedService _feedService;

        public AddPostViewModel(IPlatformService platformService, IUserService userService, IFeedService feedService) : base(platformService, userService)
        {
            _userService = userService;
            _feedService = feedService;
        }

        #endregion Constructor

        #region Init

        public async Task Init(Guid feedId)
        {
            if (feedId != Guid.Empty)
            {
                FeedId = feedId;
            }
            else
            {
                FeedId = null;
            }
            
            var personalModel = await _userService.GetPersonalModelAsync(ApiPriority.UserInitiated);

            // Android caches viewmodels so we need to reset all these values to their defaults in the init method
            SelectedImages = new ObservableCollection<SelectedImage>();

            IsNotPosting = true;

            Input = "";

            AvatarUrl = personalModel?.Avatar?.Small;
            PlaceholderText = $"Post an update {personalModel?.FirstName}";

            MentionsProperty = () => Input;
        }

        public Guid? FeedId { get; set; }

        #endregion Init

        #region User

        public string AvatarUrl
        {
            get => _avatarUrl;
            private set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public string PlaceholderText
        {
            get => _placeholderText;
            private set { _placeholderText = value; RaisePropertyChanged(() => PlaceholderText); }
        }

        #endregion User

        #region Input

        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                RaisePropertyChanged(() => Input);
                SendCommand.RaiseCanExecuteChanged();
                Task.Run(async () => { await CheckMentions(value); });
            }
        }

        public MvxCommand SendCommand => _sendCommand ?? (_sendCommand = new MvxCommand(Send, CanSend));

        private void Send()
        {
            InvokeOnMainThread(async () =>
            {
                IsPosting = true;

                Execute(StartPosting, action => action());

                if (CrossConnectivity.Current.IsConnected)
                {
                    // Is connected
                    if (await CreatePost())
                    {
                        // Post was created
                        AfterPostAction?.Invoke();
                    }
                    else
                    {
                        // Post failed
                        await UserDialogs.AlertAsync("An error occured while trying to post, please try again or contact Bisner support");
                    }
                }
                else
                {
                    // No connection
                    await UserDialogs.AlertAsync("Could not establish a connection to the server, please make sure you are connected to the internet", "No connection");
                }

                // Stop posting and call event
                IsPosting = false;
                Execute(StopPosting, action => action());
            });
        }

        private async Task<bool> CreatePost()
        {
            try
            {
                var inputText = EmojiHelper.ToShort(Input);

                if (SelectedImages.Count > 0)
                {
                    var imageStreamList = SelectedImages.Select(selectedImage => new MemoryStream(selectedImage.Bytes)).Cast<Stream>().ToList();

                    return await _feedService.CreateImagePostAsync(inputText, imageStreamList, FeedId, MentionedUserIds);
                }

                return await _feedService.CreateTextPostAsync(inputText, FeedId, MentionedUserIds);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                return false;
            }
        }

        public Action AfterPostAction { get; set; }

        private bool CanSend()
        {
            return !IsPosting && (!string.IsNullOrEmpty(Input) || SelectedImages.Count > 0);
        }

        public bool IsPosting
        {
            get => _isPosting;
            set
            {
                _isPosting = value;
                RaisePropertyChanged(() => IsPosting);
                IsNotPosting = !value;
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsNotPosting
        {
            get => _isNotPosting;
            private set { _isNotPosting = value; RaisePropertyChanged(() => IsNotPosting); }
        }

        public event Action StopPosting;

        public event Action StartPosting;

        #endregion Input

        #region Picture

        private ICommand _choosePictureCommand, _takePictureCommand, _removePictureCommand;
        private ObservableCollection<SelectedImage> _selectedImages;
        private bool _isPosting;
        private bool _isNotPosting;
        private string _avatarUrl;
        private string _placeholderText;

        public ICommand ChoosePictureCommand => _choosePictureCommand ?? (_choosePictureCommand = new MvxAsyncCommand(ChoosePictureAsync));

        public ICommand TakePictureCommand => _takePictureCommand ?? (_takePictureCommand = new MvxAsyncCommand(TakePictureAsync));

        public ICommand RemovePictureCommand => _removePictureCommand ?? (_removePictureCommand = new MvxCommand<SelectedImage>(RemoveImage));

        private async Task TakePictureAsync()
        {
            try
            {
                if (await RequestPermissionAsync(Permission.Camera))
                {
                    var task = Mvx.Resolve<IMvxBisnerImageTask>();
                    task.TakePicture(1024, 30, AddPicture, () =>
                    {
                        Debug.WriteLine("CANCELLED!!!");
                    });
                }
                else
                {
                    await UserDialogs.AlertAsync(GetResource(ResKeys.mobile_no_permission));
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.AlertAsync("Something went wrong", "Error", "Ok");
                ExceptionService.HandleException(ex);
            }
        }

        private async Task ChoosePictureAsync()
        {
            try
            {
                if (await RequestPermissionAsync(Permission.Photos))
                {
                    var task = Mvx.Resolve<IMvxBisnerImageTask>();
                    task.ChoosePictureFromLibrary(1024, 30, AddPicture, () => { });
                }
                else
                {
                    await UserDialogs.AlertAsync(GetResource(ResKeys.mobile_no_permission));
                }
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        private async Task<bool> RequestPermissionAsync(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (permissionStatus != PermissionStatus.Granted)
            {
                // TODO : ONly for android, needs resource
                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                //{
                //    await Userinteraction.AlertAsync("Blalakemr");
                //}

                var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);

                permissionStatus = results[permission];
            }

            return permissionStatus == PermissionStatus.Granted;
        }

        private void AddPicture(Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            SelectedImages.Add(new SelectedImage
            {
                Bytes = memoryStream.ToArray(),
            });
        }

        public ObservableCollection<SelectedImage> SelectedImages
        {
            get => _selectedImages;
            set { _selectedImages = value; RaisePropertyChanged(() => SelectedImages); }
        }

        public void RemoveImage(SelectedImage selectedImage)
        {
            SelectedImages.Remove(selectedImage);
        }

        #endregion Picture
    }
}

