using System;

namespace Bisner.Mobile.Core.Helpers
{
    public interface IConfiguration
    {
        bool TestMode { get; }

        Guid AppId { get; }
        
        string BaseUrl { get; }
        
        string ApiVersion { get; }

        string UserAgent { get; }
    }
}
