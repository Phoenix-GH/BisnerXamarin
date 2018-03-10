using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ModernHttpClient;

namespace Bisner.Mobile.Core.Communication
{
    public class TokenNativeMessageHandler : NativeMessageHandler
    {
        #region Constructor

        private readonly Func<Task<string>> _getToken;
        private readonly string _userAgent;
        private readonly Guid _appId;

        public TokenNativeMessageHandler(Func<Task<string>> getToken, string useragent, Guid appId)
        {
            _getToken = getToken;
            _userAgent = useragent;
            _appId = appId;
        }

        #endregion Constructor

        #region Overrides

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (auth != null && _getToken != null)
            {
                var token = await _getToken();
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
            }
            
            // Always add user agent
            request.Headers.UserAgent.ParseAdd(_userAgent);
            request.Headers.Add("AppId", _appId.ToString());

            return await base.SendAsync(request, cancellationToken);
        }

        #endregion Overrides
    }
}