using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json")]
    public interface IPlatformApi
    {
        [Get("/Api/Platform/Get")]
        Task<ApiResponse<ApiCentralPublicPlatformModel>> Get();
        
        [Get("/Api/Platform/GetBasic")]
        Task<ApiResponse<ApiCentralPublicPlatformModel>> GetBasic();

        [Get("/Api/Platform/GetCustomMenu")]
        Task<ApiResponse<PlatformCustomMenuModel>> GetCustomMenu();

        [Post("/Api/User/RequestPasswordReset")]
        Task<ApiResponse> RequestPasswordReset(string email);
    }
}