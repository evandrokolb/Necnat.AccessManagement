using Necnat.Shared.Entities;
using Necnat.Shared.Filters;
using Necnat.Shared.Models;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Necnat.Shared.HttpClients.NamHttpClients
{
    public partial class NamHttpClient
    {
        #region HierarchyManagementHierarchy

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyAList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/Hierarchy/AList?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyAGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/Hierarchy/AGetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyInsertAndAddToHierarchyHierarchies(Hierarchy e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/Hierarchy/InsertAndAddToHierarchyHierarchies?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyAUpdate(Hierarchy e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/HierarchyManagement/Hierarchy/AUpdate?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyADeleteAndRemoveFromHierarchicalStructure(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/HierarchyManagement/Hierarchy/ADeleteAndRemoveFromHierarchicalStructure?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyAFilter(string moduleCodeName, string featureCodeName, MdHierarchyFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/Hierarchy/AFilter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyAFilterSupport(string moduleCodeName, string featureCodeName, MdHierarchyFilter md = null)
        {
            if (md == null)
                md = new MdHierarchyFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/Hierarchy/AFilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region HierarchyManagementHierarchyComponentType

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyComponentTypeList()
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/HierarchyComponentType/List?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyComponentTypeGetById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/HierarchyComponentType/GetById?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyComponentTypeInsert(HierarchyComponentType e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/HierarchyComponentType/Insert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyComponentTypeUpdate(HierarchyComponentType e)
        {
            return await HttpClient.PutAsJsonAsync($"Api/v1.0/HierarchyManagement/HierarchyComponentType/Update?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyComponentTypeDelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/HierarchyManagement/HierarchyComponentType/Delete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyComponentTypeFilter(string moduleCodeName, string featureCodeName, MdHierarchyComponentTypeFilter md)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/HierarchyComponentType/Filter?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyComponentTypeFilterSupport(string moduleCodeName, string featureCodeName, MdHierarchyComponentTypeFilter md = null)
        {
            if (md == null)
                md = new MdHierarchyComponentTypeFilter();

            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/HierarchyComponentType/FilterSupport?culture=" + CultureInfo.CurrentCulture.Name + "&moduleCodeName=" + moduleCodeName + "&featureCodeName=" + featureCodeName, md);
        }

        #endregion

        #region HierarchyManagementHierarchyHierarchyComponentType

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyHierarchyComponentTypeASearchByHierarchyIdIncludeHierarchyComponentType(int hierarchyId)
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/HierarchyHierarchyComponentType/ASearchByHierarchyIdIncludeHierarchyComponentType?culture=" + CultureInfo.CurrentCulture.Name + "&hierarchyId=" + hierarchyId.ToString());
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyHierarchyComponentTypeAInsert(HierarchyHierarchyComponentType e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/HierarchyHierarchyComponentType/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchyHierarchyComponentTypeADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/HierarchyManagement/HierarchyHierarchyComponentType/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        #endregion

        #region HierarchyManagementHierarchicalStructure

        public async Task<HttpResponseMessage> HierarchyManagementHierarchicalStructureASearchHierarchyForTree()
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/HierarchicalStructure/ASearchHierarchyForTree?culture=" + CultureInfo.CurrentCulture.Name);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchicalStructureASearchTree(MdSearchTree e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/HierarchicalStructure/ASearchTree?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchicalStructureASearchTreeById(int id)
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/HierarchicalStructure/ASearchTreeById?culture=" + CultureInfo.CurrentCulture.Name + "&Id=" + id.ToString());
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchicalStructureASearchHierarchyComponentByHierarchyId(int hierarchyId)
        {
            return await HttpClient.GetAsync($"Api/v1.0/HierarchyManagement/HierarchicalStructure/ASearchHierarchyComponentByHierarchyId?culture=" + CultureInfo.CurrentCulture.Name + "&hierarchyId=" + hierarchyId.ToString());
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchicalStructureAInsert(HierarchicalStructure e)
        {
            return await HttpClient.PostAsJsonAsync($"Api/v1.0/HierarchyManagement/HierarchicalStructure/AInsert?culture=" + CultureInfo.CurrentCulture.Name, e);
        }

        public async Task<HttpResponseMessage> HierarchyManagementHierarchicalStructureADelete(int id)
        {
            return await HttpClient.DeleteAsync($"Api/v1.0/HierarchyManagement/HierarchicalStructure/ADelete?culture=" + CultureInfo.CurrentCulture.Name + "&id=" + id.ToString());
        }

        #endregion
    }
}
