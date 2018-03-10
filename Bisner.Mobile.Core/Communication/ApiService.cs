using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Fusillade;
using Newtonsoft.Json;
using Refit;

namespace Bisner.Mobile.Core.Communication
{
    public class ApiService<TApi> : IApiService<TApi>
    {
        #region Constructor

        private readonly Lazy<TApi> _explicit;
        private readonly Lazy<TApi> _speculative;
        private readonly Lazy<TApi> _background;
        private readonly Lazy<TApi> _userInitiated;

        private readonly IConfiguration _configuration;

        private readonly Func<Task<string>> _getToken;

        public ApiService(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _getToken = tokenService.GetTokenAsync;

            _explicit = new Lazy<TApi>(() => CreateApi(Priority.Explicit));
            _speculative = new Lazy<TApi>(() => CreateApi(Priority.Speculative));
            _background = new Lazy<TApi>(() => CreateApi(Priority.Background));
            _userInitiated = new Lazy<TApi>(() => CreateApi(Priority.UserInitiated));
        }

        #endregion Constructor

        #region Apis

        public TApi GetApi(ApiPriority priority)
        {
            switch (priority)
            {
                case ApiPriority.Explicit:
                    return Explicit;
                case ApiPriority.Speculative:
                    return Speculative;
                case ApiPriority.Background:
                    return Background;
                case ApiPriority.UserInitiated:
                    return UserInitiated;
                default:
                    throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
            }
        }

        public TApi Explicit => _explicit.Value;

        public TApi Speculative => _speculative.Value;

        public TApi Background => _background.Value;

        public TApi UserInitiated => _userInitiated.Value;

        #endregion Apis

        #region Helpers

        /// <summary>
        /// Create an 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        private TApi CreateApi(Priority priority)
        {
            var handler = new RateLimitedHttpMessageHandler(new TokenNativeMessageHandler(_getToken, _configuration.UserAgent, _configuration.AppId)
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            }, priority);

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(_configuration.BaseUrl),
            };
            
            var restService = RestService.For<TApi>(client, new RefitSettings
            {
                UrlParameterFormatter = new DateTimeRefit()
            });

            return restService;
        }

        #endregion Helpers
    }

    public class DateTimeRefit : DefaultUrlParameterFormatter
    {
        public override string Format(object parameterValue, ParameterInfo parameterInfo)
        {
            var date = parameterValue as DateTime?;
            if (date.HasValue)
            {
                return date.Value.ToString(CultureInfo.InvariantCulture);
            }

            return base.Format(parameterValue, parameterInfo);
        }
    }
}
