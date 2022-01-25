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
        #region AccessConfigurationRole

        public async Task<HttpResponseMessage> AccessConfigurationRoleAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/AccessConfiguration/Role/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/AccessConfiguration/Role/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleAInsert(Role e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/AccessConfiguration/Role/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleAUpdate(Role e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/AccessConfiguration/Role/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/AccessConfiguration/Role/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleAFilter(string moduleCodeName, string featureCodeName, MdRoleFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/AccessConfiguration/Role/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleAFilterSupport(string moduleCodeName, string featureCodeName, MdRoleFilter md = null)
        {
            if (md == null)
                md = new MdRoleFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/AccessConfiguration/Role/AFilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region AccessConfigurationRoleFeature

        public async Task<HttpResponseMessage> AccessConfigurationRoleFeatureASearchByRoleIdIncludeFeature(int roleId)
        {
            return await HttpClient.GetAsync($"Api/v1.0/AccessConfiguration/RoleFeature/ASearchByRoleIdIncludeFeature?culture=" + CultureInfo.CurrentCulture.Name + "&roleId=" + roleId.ToString());
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleFeatureAInsert(RoleFeature e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/AccessConfiguration/RoleFeature/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> AccessConfigurationRoleFeatureADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/AccessConfiguration/RoleFeature/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        #endregion
    }
}
