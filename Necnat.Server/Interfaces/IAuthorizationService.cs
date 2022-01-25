using Necnat.Shared.Entities;
using Necnat.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Necnat.Server.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<List<MdAllowedRole>> SearchAllowedRoleListWithoutHierarchyAsync(string userId, int applicationId);

        public Task<List<MdAllowedModuleFeature>> SearchAllowedFeatureListWithoutHierarchyAsync(string userId, int applicationId);

        public Task<List<MdAllowedRole>> SearchAllowedRoleListAsync(string userId, int applicationId);

        public Task<List<MdAllowedModuleFeature>> SearchAllowedFeatureListAsync(string userId, int applicationId);

        public Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync(int hierarchicalStructureId);

        public Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync(int hierarchicalStructureId);

        public Task<List<int?>> SearchAllowedHierarchicalStructureIdAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion);

        public Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion);

        public Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion);

        public Task<List<int?>> SearchAllowedHierarchicalStructureIdAsync(string userId, int applicationId, string moduleCodeName, string featureCodeName);

        public Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync(string userId, int applicationId, string moduleCodeName, string featureCodeName);

        public Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync(string userId, int applicationId, string moduleCodeName, string featureCodeName);

        public Task<bool> IsAllowedApiAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion);

        public Task<bool> IsAllowedApiAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion, int componentTypeId, int componenteId);

        public void ClearCache();
    }
}
