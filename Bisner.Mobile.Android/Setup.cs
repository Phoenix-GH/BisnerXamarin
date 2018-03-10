using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Android.Views;
using Android.Widget;
using Bisner.Constants;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Droid.Controls;
using Bisner.Mobile.Droid.Controls.Bindings;
using Bisner.Mobile.Droid.Helpers;
using Bisner.Mobile.Droid.Service;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using PluginLoader = MvvmCross.Plugins.DownloadCache.PluginLoader;

namespace Bisner.Mobile.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new MainPresenter(AndroidViewAssemblies);
            Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(presenter);
            return presenter;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            // Set platform
            App.AppPlatform = AppPlatform.Android;

            Mvx.RegisterType<IMvxBisnerImageTask, MvxBisnerImageTask>();
            Mvx.RegisterType<IExceptionService, ExceptionService>();
            Mvx.RegisterType<IPushNotificationService, AndroidPushNotificationService>();
            Mvx.RegisterType<ILocale, LocaleAndroid>();
            Mvx.LazyConstructAndRegisterSingleton<IAnalyticsService, GaService>();
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(Android.Support.Design.Widget.NavigationView).Assembly,
            typeof(Android.Support.Design.Widget.FloatingActionButton).Assembly,
            typeof(Android.Support.V7.Widget.Toolbar).Assembly,
            typeof(Android.Support.V7.Widget.CardView).Assembly,
            typeof(Android.Support.V4.Widget.DrawerLayout).Assembly,
            typeof(Android.Support.V4.View.ViewPager).Assembly,
            typeof(MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView).Assembly
        };

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
            registry.RegisterCustomBindingFactory<Button>("FeedButton", button => new FeedButtonBinding(button));
            registry.RegisterCustomBindingFactory<ImageView>("FeedImage", imageView => new FeedImageBinding(imageView));
            registry.RegisterCustomBindingFactory<Button>("MembersButton", button => new MembersButtonBinding(button));
            registry.RegisterCustomBindingFactory<View>("NotificationBackground", view => new BooleanBackgroundColorBinding(Resource.Color.white, Resource.Color.backpanelborderbottom, view));
            registry.RegisterCustomBindingFactory<EditText>("SecurityEditText", editText => new EditTextErrorBinding(Resource.Color.error, Resource.Color.white, editText));

            registry.RegisterCustomBindingFactory<MvxRoundedImageView>("AccessControlItemImage", imageView => new AccessControlImageBinding(imageView));
            registry.RegisterCustomBindingFactory<View>("AccessControlItemBackground", view => new AccessControlBackgroundBinding(view));
            registry.RegisterCustomBindingFactory<TextView>("AccessControlTitleColor", view => new AccessControlTextColorBinding(view, Resource.Color.defaulttextcolor, Resource.Color.defaulttextcolor, Resource.Color.white));
            registry.RegisterCustomBindingFactory<TextView>("AccessControlSubtitleColor", view => new AccessControlTextColorBinding(view, Resource.Color.subtextcolor, Resource.Color.subtextcolor, Resource.Color.white));

            registry.RegisterCustomBindingFactory<TextView>("HtmlLinkClickText", view => new TextViewHtmlClickBinding(view));

            base.FillTargetFactories(registry);
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            PluginLoader.Instance.EnsureLoaded();
        }
    }
}
