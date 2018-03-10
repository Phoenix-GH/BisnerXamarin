using Bisner.Constants;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.iOS.Controls.Bindings;
using Bisner.Mobile.iOS.Helpers;
using Bisner.Mobile.iOS.Service;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Plugins.PictureChooser;
using UIKit;

namespace Bisner.Mobile.iOS.MvvmcrossApp
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.RegisterSingleton(() => new App());

            return Mvx.Resolve<App>();
        }

        protected override IMvxIosViewsContainer CreateIosViewsContainer()
        {
            return new StoryboardContainer();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            // Create our custom presenter
            var mainPresenter = new MainPresenter(ApplicationDelegate as UIApplicationDelegate, Window);

            // Register in IOC
            Mvx.RegisterSingleton<IMainViewPresenterHost>(mainPresenter);

            return mainPresenter;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<UIButton>("Like", button => new LikeButtonBinding(button));
            registry.RegisterCustomBindingFactory<UIButton>("Comment", button => new CommentButtonBinding(button));
            registry.RegisterCustomBindingFactory<UIButton>("Members", button => new MembersButtonBinding(button));
            registry.RegisterCustomBindingFactory<UIControl>("ErrorColor", control => new ControlErrorBinding(control));
            registry.RegisterCustomBindingFactory<UIButton>("GreenCheckBox", button => new GreenCheckBoxButtonBinding(button));
            registry.RegisterCustomBindingFactory<UIActivityIndicatorView>("ActivityHidden", view => new ActivityIndicatorViewHiddenTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIWebView>("HtmlString", view => new WebviewHtmlStringBinding(view));
            registry.RegisterCustomBindingFactory<UIWebView>("WebviewUrl", view => new WebviewUrlBinding(view));
            registry.RegisterCustomBindingFactory<UIImageView>("AvatarImageUrl", view => new ImageViewUrlBinding(view, "Icons/default_avatar.jpg"));
            registry.RegisterCustomBindingFactory<UIImageView>("ImageUrl", view => new ImageViewUrlBinding(view));

            registry.RegisterCustomBindingFactory<UIView>("AvailableBackground", view => new ViewBackgroundBoolBinding(view, UIColor.FromRGBA(0.0f, 1.0f, 0.0f, 0.3f), UIColor.FromRGBA(1.0f, 0.0f, 0.0f, 0.3f)));
            registry.RegisterCustomBindingFactory<UIView>("AccessControlBackground", view => new AccessControlBackgroundBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("AccessControlBorder", view => new AccessControlBorderBinding(view));
            registry.RegisterCustomBindingFactory<UIImageView>("AccessControlImage", view => new AccessControlImageBinding(view));


            // Bridge colors
            // Pink : 207, 0,88
            // Grey : 247, 247, 247


            registry.RegisterCustomBindingFactory<UIView>("NotificationBackground", view => new ViewBackgroundBoolBinding(view, UIColor.Clear, Appearance.Colors.BackPanelBorderBottom));
            
            base.FillTargetFactories(registry);
        }


        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            // Set platform
            App.AppPlatform = AppPlatform.iOS;

            Mvx.RegisterType<IMvxBisnerImageTask, MyImagePickerTask>();
            Mvx.RegisterType<IExceptionService, ExceptionService>();
            Mvx.LazyConstructAndRegisterSingleton<IPushNotificationService, ApplePushNotificationService>();
            Mvx.LazyConstructAndRegisterSingleton<IAnalyticsService, GaService>();
            Mvx.LazyConstructAndRegisterSingleton<INetworkManager, IosNetworkManager>();
            Mvx.RegisterType<ILocale, LocaleIOS>();
        }
    }
}