using System;

namespace Bisner.Mobile.Core.Service
{
    public interface IExceptionService
    {
        void HandleException(Exception ex, bool isFatal = false);
    }
}
