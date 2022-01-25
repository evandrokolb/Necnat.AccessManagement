using Necnat.Shared.Entities;
using Necnat.Shared.Filters;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Necnat.Shared.HttpClients.NamHttpClients
{
    public partial class NamHttpClient
    {
        #region AccessManagementAuthorization

        public async Task<HttpResponseMessage> AccessManagementAuthorizationAGetUserAuthorizationByApplicationId(string authorizationApiEndPoint, int applicationId)
        {
            return await HttpClient.GetAsync(authorizationApiEndPoint + applicationId);
        }

        #endregion

        #region AccessManagementSecurity

        public async Task<HttpResponseMessage> AccessManagementSecurityAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/AccessManagement/Security/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> AccessManagementSecurityAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/AccessManagement/Security/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> AccessManagementSecurityAInsert(Security e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/AccessManagement/Security/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> AccessManagementSecurityAUpdate(Security e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/AccessManagement/Security/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> AccessManagementSecurityADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/AccessManagement/Security/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> AccessManagementSecurityAFilter(string moduleCodeName, string featureCodeName, MdSecurityFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/AccessManagement/Security/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region AccessManagementUser

        public async Task<HttpResponseMessage> AccessManagementUserList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/AccessManagement/User/List?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> AccessManagementUserGetById(string id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/AccessManagement/User/GetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> AccessManagementUserFilter(string moduleCodeName, string featureCodeName, MdUserFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/AccessManagement/User/Filter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion
    }
}
