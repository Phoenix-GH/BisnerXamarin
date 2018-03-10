using System;

namespace Bisner.Mobile.Core.Helpers
{
    public class DefaultConfiguration : IConfiguration
    {
        public bool TestMode => true;

        public string BaseUrl => "https://whitelabel.bisner.com/";

        public virtual TimeSpan TimeOutPeriod => new TimeSpan(0, 0, 3, 0);

        public Guid AppId => Guid.Parse("3b1cbe8f-bf3d-4c82-ab5a-8ab84825552c");

        public string ApiVersion => "2";

        // TODO : Change per platform and version
        public virtual string UserAgent => "BisnerIOS 0.2.20";
    }
}

