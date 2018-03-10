using System.Collections.Generic;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.ApiModels.Integrations
{
    public class ApiSocialLoginResult
    {
        public bool Success { get; set; }

        public List<string> Errors { get; set; }

        public int ErrorCode { get; set; }

        public string Provider { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public ApiWhitelabelPrivateUserModel User { get; set; }

        public string ProviderUserId { get; set; }
    }
}