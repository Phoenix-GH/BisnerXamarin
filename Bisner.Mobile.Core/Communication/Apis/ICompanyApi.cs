using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface ICompanyApi
    {
        /// <summary>
        /// Get all companies
        /// </summary>
        /// <returns></returns>
        [Get("/Api/Company/GetAll")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<List<ApiWhitelabelCompanyModel>>> GetAll();

        /// <summary>
        /// Get all companies
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Get("/Api/Company/Get")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiWhitelabelCompanyModel>> Get(Guid id);

        /// <summary>
        /// Get all user companies
        /// </summary>
        /// <returns></returns>
        [Get("/Api/Company/GetMyComanies")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<List<ApiWhitelabelCompanyModel>>> GetMyCompanies();

        /// <summary>
        /// Createa a new company and make the user admin
        /// </summary>
        /// <param name="name">Company name</param>
        /// <returns></returns>
        [Post("/Api/Company/Createasync")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiWhitelabelPublicUserModel>> CreateAsync(string name);

        /// <summary>
        /// Update a company -&gt; Only works for company admins
        /// </summary>
        /// <param name="model">Update model</param>
        /// <returns></returns>
        [Post("/Api/Company/UpdateAsync")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiWhitelabelCompanyModel>> UpdateAsync(ApiWhitelabelCompanyModel model);

        /// <summary>
        /// Change company logo -&gt; Only works for company admins
        /// </summary>
        /// <param name="id">The identifier.</param>
        [Post("/Api/Company/ChangeLogoAsync")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiWhitelabelCompanyModel>> ChangeLogoAsync(Guid id);

        /// <summary>
        /// Change company logo -&gt; Only works for company admins
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Post("/Api/Company/ChangeHeaderAsync")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiWhitelabelCompanyModel>> ChangeHeaderAsync(Guid id);

        /// <summary>
        /// Join a company pending list
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Post("/Api/Company/JoinCompany")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiWhitelabelCompanyModel>> JoinCompany(Guid id);

        /// <summary>
        /// Leave a company
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Post("/Api/Company/LeaveCompany")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiCentralPlatformModel>> LeaveCompany(Guid id);

        /// <summary>
        /// Accept or add user
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
        /// <returns></returns>
        [Post("/Api/Company/AcceptPending")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiCentralPlatformModel>> AcceptPending(Guid id, Guid userId, bool isAdmin = false);

        /// <summary>
        /// Accept or add user
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [Post("/Api/Company/RemoveUser")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<ApiCentralPlatformModel>> RemoveUser(Guid id, Guid userId);

        /// <summary>
        /// Delete a company
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Post("/Api/Company/DeleteAsync")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse> DeleteAsync(Guid id);
    }
}