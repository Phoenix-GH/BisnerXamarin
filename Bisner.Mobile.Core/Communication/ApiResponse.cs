using System.Collections.Generic;
using System.Net;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.Mobile.Core.Communication
{
    /// <summary>
    /// Generic API response object including additional data
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TAdditionalData"></typeparam>
    public class ApiResponse<TData, TAdditionalData> : ApiResponse<TData>
    {
        public TAdditionalData AdditionalData { get; set; }
    }

    /// <summary>
    /// Generic API response object
    /// </summary>
    /// <typeparam name="TData">Type parameter for json desirializing <see cref="TData"/></typeparam>
    public class ApiResponse<TData> : ApiResponse
    {
        /// <summary>
        /// Data object carrying the deserialized JSON data of type <see cref="TData"/>
        /// </summary>
        public TData Data { get; set; }
    }

    /// <summary>
    /// Api response object
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Indicates if the request was successfull or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error code indicating what exactly went wrong
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Message containing information about the request processing
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The cookiecontainer retrieved from the request
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        public long ResultTime { get; set; }
    }

    public class MembersApiResponse
    {
        public List<ApiWhitelabelPublicUserModel> Users { get; set; }

        public List<ApiWhitelabelCompanyModel> Companies { get; set; }
    }

    public class JwtResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
