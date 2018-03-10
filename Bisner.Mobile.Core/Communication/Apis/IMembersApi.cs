using System.Threading.Tasks;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IMembersApi
    {
        [Get("/Api/Members/GetAll")]
        Task<ApiResponse<MembersApiResponse>> GetAll();
    }
}