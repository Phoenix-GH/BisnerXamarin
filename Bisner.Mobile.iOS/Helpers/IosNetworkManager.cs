using System;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Foundation;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Helpers
{
    public class IosNetworkManager : INetworkManager
    {
        public void OpenUrl(string url)
        {
            try
            {
                var nsUrl = new NSUrl(url);

                UIApplication.SharedApplication.OpenUrl(nsUrl);
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }
    }
}