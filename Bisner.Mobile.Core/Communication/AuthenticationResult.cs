using System.Net;

namespace Bisner.Mobile.Core.Communication
{
    public class AuthenticationResult
    {
        public AuthenticationResult(HttpStatusCode statusCode)
        {
            Success = statusCode == HttpStatusCode.OK;
            StatusCode = statusCode;
        }

        public bool Success { get; }

        public HttpStatusCode StatusCode { get; }

        public string Message { get; set; }
    }
}
