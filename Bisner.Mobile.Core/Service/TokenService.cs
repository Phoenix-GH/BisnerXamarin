using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using Fusillade;
using JWT;
using JWT.exceptions;
using ModernHttpClient;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Polly;
using Refit;

namespace Bisner.Mobile.Core.Service
{
    public class TokenService : ITokenService
    {
        #region Constructor

        private readonly IExceptionService _exceptionService;
        private readonly IAuthenticationApi _authenticationApi;
        private readonly IConfiguration _configuration;

        public TokenService(IExceptionService exceptionService, IConfiguration configuration)
        {
            _exceptionService = exceptionService;
            _configuration = configuration;

            _authenticationApi =
                RestService.For<IAuthenticationApi>(
                    new HttpClient(new RateLimitedHttpMessageHandler(
                        new NativeMessageHandler
                        {
                            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                        }, Priority.UserInitiated))
                    { BaseAddress = new Uri(configuration.BaseUrl) });
        }

        #endregion Constructor

        #region Token

        public async Task<AuthenticationResult> GetTokenAsync(string username, string password)
        {
            Logger.Log("AUTHENTICATIONSERVICE : Getting token");

            var response = await Policy.Handle<ApiException>().WaitAndRetryAsync(5, i => new TimeSpan(0, 0, 0, i * 5))
                .ExecuteAsync(() => _authenticationApi.RequestJwt(_configuration.AppId, username,
                    password));

            return HandleResponse(response);
        }

        public async Task<string> GetTokenAsync()
        {
            // Check expiration
            if (DateTime.UtcNow < Settings.TokenExpiration.AddMinutes(-5))
            {
                return Settings.Token;
            }

            // Refresh
            await RefreshTokenAsync();

            return Settings.Token;
        }

        public async Task<AuthenticationResult> RefreshTokenAsync()
        {
            Logger.Log("AUTHENTICATIONSERVICE : Refreshing token");

            var response = await Policy.Handle<ApiException>().WaitAndRetryAsync(5, i => new TimeSpan(0, 0, 0, i * 5))
                .ExecuteAsync(() => _authenticationApi.RefreshJwt(Mvx.Resolve<IConfiguration>().AppId, Settings.RefreshToken));

            var authenticationResponse = HandleResponse(response);

            if (!authenticationResponse.Success)
            {
                await App.LogOut();
            }

            return authenticationResponse;
        }

        private AuthenticationResult HandleResponse(ApiResponse<JwtResponse> response)
        {
            if (response != null && response.Success)
            {
                GetAndSetJwtInfo(response.Data.Token);

                // Update settings
                Settings.Token = response.Data?.Token;
                Settings.RefreshToken = response.Data?.RefreshToken;
                var expirationDateTime = DateTime.UtcNow.AddSeconds(response.Data.ExpiresIn);
                Settings.TokenExpiration = expirationDateTime;
            }

            return ValidateResponse(response);
        }

        private AuthenticationResult ValidateResponse(ApiResponse<JwtResponse> response)
        {
            var statusCode = HttpStatusCode.OK;

            if (!response.Success)
            {
                // App id not valid
                if (response.ErrorCode == ApiErrorCodes.GeneralError)
                    statusCode = HttpStatusCode.BadRequest;

                // App no access to platform
                if (response.ErrorCode == ApiErrorCodes.NoAccess)
                    statusCode = HttpStatusCode.Forbidden;

                // Invalid login
                if (response.ErrorCode == ApiErrorCodes.InvalidLogin)
                    statusCode = HttpStatusCode.Unauthorized;
            }
            else if (response.Data == null)
            {
                statusCode = HttpStatusCode.ExpectationFailed;
            }
            
            Logger.Log($"AUTHENTICATIONSERVICE : Response code [{statusCode}]");

            return new AuthenticationResult(statusCode)
            {
                Message = response.Message
            };
        }

        private void GetAndSetJwtInfo(string token)
        {
            //Get info from JWT token
            try
            {
                var tokenPayload = JsonWebToken.DecodeToObject<TokenPayload>(token, string.Empty, false);

                Settings.UserId = tokenPayload.nameid;
            }
            catch (SignatureVerificationException ex)
            {
                _exceptionService.HandleException(ex);
            }
        }

        private class TokenPayload
        {
            public Guid nameid { get; set; }
            public string unique_name { get; set; }
            public string platformId { get; set; }
            public int nbf { get; set; }
            public int exp { get; set; }
            public int iat { get; set; }
            public string iss { get; set; }
            public string aud { get; set; }
        }

        #endregion Token
    }
}