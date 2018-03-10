using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Droid.Views.Base;
using Java.Net;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using CookieManager = Android.Webkit.CookieManager;
using Debug = System.Diagnostics.Debug;

namespace Bisner.Mobile.Droid.Views.Dashboard
{
    [Activity(NoHistory = false, WindowSoftInputMode = SoftInput.StateHidden)]
    public class WebBrowserView : BaseToolbarActivity<WebBrowserViewModel>
    {
        #region Constructor

        private WebView _webView;

        public WebBrowserView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region View

        private MvxPropertyChangedListener _urlChangedListener;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _webView = FindViewById<WebView>(Resource.Id.webbrowser_view_webview);
            WebSettings settings = _webView.Settings;
            settings.JavaScriptEnabled = true;
            settings.SetAppCacheEnabled(true);
            settings.BuiltInZoomControls = true;
            settings.SetPluginState(WebSettings.PluginState.On);

            _webView.SetWebChromeClient(new WebChromeClient());

            CookieSyncManager.CreateInstance(this);
            CookieManager cookieManager = CookieManager.Instance;
            cookieManager.RemoveSessionCookie();
            String cookieString = "param=value";
            cookieManager.SetCookie("https://thebridge.spaces.nexudus.com/", cookieString);
            CookieSyncManager.Instance.Sync();

            var abc = new Dictionary<String, String> {["Cookie"] = cookieString};
            _webView.LoadUrl(ViewModel.Url, abc);

            try
            {
                if (!string.IsNullOrEmpty(ViewModel.Url))
                {
                    _webView?.SetWebViewClient(new WebViewClient());
                    _webView?.LoadUrl(ViewModel.Url);
                }
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }

            _urlChangedListener = new MvxPropertyChangedListener(ViewModel).Listen(() => ViewModel.Url, () =>
            {
                RunOnUiThread(() =>
                {
                    _webView.StopLoading();
                    _webView.LoadUrl(ViewModel.Url);
                });
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _urlChangedListener.Dispose();
            _urlChangedListener = null;
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.chat_toolbar, menu);

        //    return base.OnCreateOptionsMenu(menu);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    if (item.ItemId == Resource.Id.create_chat_item)
        //    {
        //        ViewModel.SwapCommand.Execute(null);

        //        return true;
        //    }

        //    return base.OnOptionsItemSelected(item);
        //}

        protected override int LayoutId => Resource.Layout.webbrowser_view;

        protected override string ScreenName => "WebBroserView";

        #endregion View
    }
}