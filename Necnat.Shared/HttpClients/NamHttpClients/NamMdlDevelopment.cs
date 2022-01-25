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
        #region DevelopmentApplication

        public async Task<HttpResponseMessage> DevelopmentApplicationAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Application/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Application/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationInsertAndAddToHierarchyApplications(Application e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Application/InsertAndAddToHierarchyApplications?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationAUpdate(Application e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/Development/Application/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationADeleteAndRemoveFromHierarchicalStructure(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/Development/Application/ADeleteAndRemoveFromHierarchicalStructure?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationAFilter(string moduleCodeName, string featureCodeName, MdApplicationFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Application/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationAFilterSupport(string moduleCodeName, string featureCodeName, MdApplicationFilter md = null)
        {
            if (md == null)
                md = new MdApplicationFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Application/AFilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region DevelopmentApplicationHierarchy

        public async Task<HttpResponseMessage> DevelopmentApplicationHierarchyASearchByApplicationIdIncludeHierarchy(int applicationId)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/ApplicationHierarchy/ASearchByApplicationIdIncludeHierarchy?culture=" + CultureInfo.CurrentCulture.Name + "&applicationId=" + applicationId.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationHierarchyAInsert(ApplicationHierarchy e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/ApplicationHierarchy/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentApplicationHierarchyADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/Development/ApplicationHierarchy/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        #endregion

        #region DevelopmentModule

        public async Task<HttpResponseMessage> DevelopmentModuleAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Module/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> DevelopmentModuleAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Module/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentModuleAInsert(Module e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Module/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentModuleAUpdate(Module e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/Development/Module/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentModuleADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/Development/Module/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentModuleAFilter(string moduleCodeName, string featureCodeName, MdModuleFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Module/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> DevelopmentModuleAFilterSupport(string moduleCodeName, string featureCodeName, MdModuleFilter md = null)
        {
            if (md == null)
                md = new MdModuleFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Module/AFilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region DevelopmentController

        public async Task<HttpResponseMessage> DevelopmentControllerAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Controller/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> DevelopmentControllerAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Controller/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentControllerAInsert(Controller e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Controller/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentControllerAUpdate(Controller e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/Development/Controller/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentControllerADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/Development/Controller/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentControllerAFilter(string moduleCodeName, string featureCodeName, MdControllerFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Controller/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> DevelopmentControllerAFilterSupport(string moduleCodeName, string featureCodeName, MdControllerFilter md = null)
        {
            if (md == null)
                md = new MdControllerFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Controller/AFilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region DevelopmentApi

        public async Task<HttpResponseMessage> DevelopmentApiAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Api/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> DevelopmentApiAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Api/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentApiASearchByControllerId(int controllerId)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Api/ASearchByControllerId?culture=" + CultureInfo.CurrentCulture.Name + "&controllerId=" + controllerId.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentApiAInsert(Api e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Api/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentApiAUpdate(Api e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/Development/Api/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentApiADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/Development/Api/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentApiAFilter(string moduleCodeName, string featureCodeName, MdApiFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Api/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> DevelopmentApiAFilterSupport(string moduleCodeName, string featureCodeName, MdApiFilter md = null)
        {
            if (md == null)
                md = new MdApiFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Api/AFilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region DevelopmentFeature

        public async Task<HttpResponseMessage> DevelopmentFeatureAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Feature/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/Feature/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureAInsert(Feature e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Feature/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureAUpdate(Feature e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/Development/Feature/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/Development/Feature/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureAFilter(string moduleCodeName, string featureCodeName, MdFeatureFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Feature/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureAFilterSupport(string moduleCodeName, string featureCodeName, MdFeatureFilter md = null)
        {
            if (md == null)
                md = new MdFeatureFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/Feature/AFilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region DevelopmentFeatureApi

        public async Task<HttpResponseMessage> DevelopmentFeatureApiASearchByFeatureIdIncludeApi(int featureId)
        {
            return await HttpClient.GetAsync($"Api/v1.0/Development/FeatureApi/ASearchByFeatureIdIncludeApi?culture=" + CultureInfo.CurrentCulture.Name + "&featureId=" + featureId.ToString());
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureApiAInsert(FeatureApi e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/Development/FeatureApi/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> DevelopmentFeatureApiADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/Development/FeatureApi/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        #endregion
    }
}
