using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Service;
using MvvmCross.Platform;

namespace Bisner.Mobile.iOS.Service
{
    public class ExceptionService : IExceptionService
    {
        public void HandleException(Exception ex, bool isFatal = false)
        {
            Debug.WriteLine(ex);
            Mvx.Resolve<IAnalyticsService>().SendException(ex.Message + " ----- " + ex.StackTrace, isFatal);
        }
    }
}