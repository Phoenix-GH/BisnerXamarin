using System;
using System.Diagnostics;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Service;

namespace Bisner.Mobile.Droid.Service
{
    public class ExceptionService : IExceptionService
    {
        public void HandleException(Exception ex, bool isFatal = false)
        {
            Debug.WriteLine(ex);
        }
    }
}