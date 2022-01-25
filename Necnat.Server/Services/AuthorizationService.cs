using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Necnat.Server.Interfaces;
using Necnat.Server.Views;
using Necnat.Shared.Entities;
using Necnat.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Necnat.Server.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        Dictionary<int, List<MdHierarchyComponent>> _searchAllowedHierarchyComponentCache = new Dictionary<int, List<MdHierarchyComponent>>();
        Dictionary<int, List<HierarchicalStructure>> _searchAllowedHierarchicalStructureCache = new Dictionary<int, List<HierarchicalStructure>>();

        protected IConfiguration _configuration;

        public AuthorizationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<MdAllowedRole>> SearchAllowedRoleListWithoutHierarchyAsync(string userId, int applicationId)
        {
            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	R.[Id] AS RoleId,");
                sb.AppendLine("	R.[CodeName] AS RoleCodeName");
                sb.AppendLine("FROM [dbo].[Security] S");
                sb.AppendLine("INNER JOIN [dbo].[Role] R");
                sb.AppendLine("	ON S.[RoleId] = R.[Id]");
                sb.AppendLine("WHERE S.[IsActive] = 1");
                sb.AppendLine("AND S.[UserId] = @userId");
                sb.AppendLine("AND R.[ApplicationId] = @applicationId");

                return (await con.QueryAsync<MdAllowedRole>(sb.ToString(), new { userId, applicationId })).AsList();
            }
        }

        public async Task<List<MdAllowedModuleFeature>> SearchAllowedFeatureListWithoutHierarchyAsync(string userId, int applicationId)
        {
            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	M.[Id] AS ModuleId,");
                sb.AppendLine("	M.[CodeName] AS ModuleCodeName,");
                sb.AppendLine("	F.[Id] AS FeatureId,");
                sb.AppendLine("	F.[CodeName] AS FeatureCodeName");
                sb.AppendLine("FROM [dbo].[Security] S");
                sb.AppendLine("INNER JOIN [dbo].[Role] R");
                sb.AppendLine("	ON S.[RoleId] = R.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[RoleFeature] FR");
                sb.AppendLine("	ON R.[Id] = FR.[RoleId]");
                sb.AppendLine("INNER JOIN [dbo].[Feature] F");
                sb.AppendLine("	ON FR.[FeatureId] = F.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[Module] M");
                sb.AppendLine("	ON F.[ModuleId] = M.[Id]");
                sb.AppendLine("WHERE S.[IsActive] = 1");
                sb.AppendLine("AND S.[UserId] = @userId");
                sb.AppendLine("AND R.[ApplicationId] = @applicationId");

                return (await con.QueryAsync<MdAllowedModuleFeature>(sb.ToString(), new { userId, applicationId })).AsList();
            }
        }

        public async Task<List<MdAllowedRole>> SearchAllowedRoleListAsync(string userId, int applicationId)
        {
            var lAllowed = new List<VwAllowedRole>();
            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	R.[Id] AS RoleId,");
                sb.AppendLine("	R.[CodeName] AS RoleCodeName,");
                sb.AppendLine("	S.[HierarchicalStructureId] AS HierarchicalStructureId");
                sb.AppendLine("FROM [dbo].[Security] S");
                sb.AppendLine("INNER JOIN [dbo].[Role] R");
                sb.AppendLine("	ON S.[RoleId] = R.[Id]");
                sb.AppendLine("WHERE S.[IsActive] = 1");
                sb.AppendLine("AND S.[UserId] = @userId");
                sb.AppendLine("AND R.[ApplicationId] = @applicationId");

                lAllowed = (await con.QueryAsync<VwAllowedRole>(sb.ToString(), new { userId, applicationId })).AsList();
            }

            var l = new List<MdAllowedRole>();
            foreach (var iAllowed in lAllowed)
            {
                var m = new MdAllowedRole();
                m.RoleId = iAllowed.RoleId;
                m.RoleCodeName = iAllowed.RoleCodeName;

                if (iAllowed.HierarchicalStructureId == null)
                    m.HierarchyComponentList = new List<MdHierarchyComponent>();
                else
                    m.HierarchyComponentList = await SearchAllowedHierarchyComponentAsync((int)iAllowed.HierarchicalStructureId);

                var exist = l.Where(x => x.RoleId == m.RoleId).FirstOrDefault();
                if (exist == null)
                    l.Add(m);
                else
                    foreach (var iHierarchyComponent in m.HierarchyComponentList)
                        if (!exist.HierarchyComponentList.Any(x => x.HierarchyId == iHierarchyComponent.HierarchyId && x.ComponentTypeId == iHierarchyComponent.ComponentTypeId && x.ComponentId == iHierarchyComponent.ComponentId))
                            exist.HierarchyComponentList.Add(iHierarchyComponent);
            }

            return l;
        }

        public async Task<List<MdAllowedModuleFeature>> SearchAllowedFeatureListAsync(string userId, int applicationId)
        {
            var lAllowed = new List<VwAllowedModuleFeature>();
            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	M.[Id] AS ModuleId,");
                sb.AppendLine("	M.[CodeName] AS ModuleCodeName,");
                sb.AppendLine("	F.[Id] AS FeatureId,");
                sb.AppendLine("	F.[CodeName] AS FeatureCodeName,");
                sb.AppendLine("	S.[HierarchicalStructureId] AS HierarchicalStructureId");
                sb.AppendLine("FROM [dbo].[Security] S");
                sb.AppendLine("INNER JOIN [dbo].[Role] R");
                sb.AppendLine("	ON S.[RoleId] = R.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[RoleFeature] FR");
                sb.AppendLine("	ON R.[Id] = FR.[RoleId]");
                sb.AppendLine("INNER JOIN [dbo].[Feature] F");
                sb.AppendLine("	ON FR.[FeatureId] = F.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[Module] M");
                sb.AppendLine("	ON F.[ModuleId] = M.[Id]");
                sb.AppendLine("WHERE S.[IsActive] = 1");
                sb.AppendLine("AND S.[UserId] = @userId");
                sb.AppendLine("AND R.[ApplicationId] = @applicationId");

                lAllowed = (await con.QueryAsync<VwAllowedModuleFeature>(sb.ToString(), new { userId, applicationId })).AsList();
            }

            var l = new List<MdAllowedModuleFeature>();
            foreach (var iAllowed in lAllowed)
            {
                var m = new MdAllowedModuleFeature();
                m.ModuleId = iAllowed.ModuleId;
                m.ModuleCodeName = iAllowed.ModuleCodeName;
                m.FeatureId = iAllowed.FeatureId;
                m.FeatureCodeName = iAllowed.FeatureCodeName;

                if (iAllowed.HierarchicalStructureId == null)
                    m.HierarchyComponentList = new List<MdHierarchyComponent>();
                else
                    m.HierarchyComponentList = await SearchAllowedHierarchyComponentAsync((int)iAllowed.HierarchicalStructureId);

                var exist = l.Where(x => x.ModuleId == m.ModuleId && x.FeatureId == m.FeatureId).FirstOrDefault();
                if (exist == null)
                    l.Add(m);
                else
                    foreach (var iHierarchyComponent in m.HierarchyComponentList)
                        if (!exist.HierarchyComponentList.Any(x => x.HierarchyId == iHierarchyComponent.HierarchyId && x.ComponentTypeId == iHierarchyComponent.ComponentTypeId && x.ComponentId == iHierarchyComponent.ComponentId))
                            exist.HierarchyComponentList.Add(iHierarchyComponent);
            }

            return l;
        }

        public async Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync(int hierarchicalStructureId)
        {
            var value = _searchAllowedHierarchyComponentCache.Where(x => x.Key == hierarchicalStructureId).Select(x => x.Value).FirstOrDefault();
            if (value != null)
                return value;

            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("WITH [Recursive] AS");
                sb.AppendLine("(");
                sb.AppendLine("	SELECT");
                sb.AppendLine("		R.[Id]");
                sb.AppendLine("		,R.[HierarchyId]");
                sb.AppendLine("		,R.[ComponentTypeId]");
                sb.AppendLine("		,R.[ComponentId]");
                sb.AppendLine("	FROM [dbo].[HierarchicalStructure] AS R");
                sb.AppendLine("	WHERE R.[Id] = @hierarchicalStructureId");
                sb.AppendLine("	UNION ALL");
                sb.AppendLine("	SELECT");
                sb.AppendLine("		R.[Id]");
                sb.AppendLine("		,R.[HierarchyId]");
                sb.AppendLine("		,R.[ComponentTypeId]");
                sb.AppendLine("		,R.[ComponentId]");
                sb.AppendLine("	FROM [dbo].[HierarchicalStructure] AS R");
                sb.AppendLine("	INNER JOIN [Recursive] as RR");
                sb.AppendLine("		ON RR.[Id] = R.[ParentHierarchicalStructureId]");
                sb.AppendLine(")");

                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	[HierarchyId]");
                sb.AppendLine("	,[ComponentTypeId] AS ComponentTypeId");
                sb.AppendLine("	,[ComponentId] AS ComponentId");
                sb.AppendLine("FROM [Recursive]");

                var l = (await con.QueryAsync<MdHierarchyComponent>(sb.ToString(), new { hierarchicalStructureId })).ToList();
                _searchAllowedHierarchyComponentCache.Add(hierarchicalStructureId, l);

                return l;
            }
        }

        public async Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync(int hierarchicalStructureId)
        {
            var value = _searchAllowedHierarchicalStructureCache.Where(x => x.Key == hierarchicalStructureId).Select(x => x.Value).FirstOrDefault();
            if (value != null)
                return value;

            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("WITH [Recursive] AS");
                sb.AppendLine("(");
                sb.AppendLine("	SELECT");
                sb.AppendLine("		R.[Id]");
                sb.AppendLine("	    ,R.[ParentHierarchicalStructureId]");
                sb.AppendLine("		,R.[HierarchyId]");
                sb.AppendLine("		,R.[ComponentTypeId]");
                sb.AppendLine("		,R.[ComponentId]");
                sb.AppendLine("	FROM [dbo].[HierarchicalStructure] AS R");
                sb.AppendLine("	WHERE R.[Id] = @hierarchicalStructureId");
                sb.AppendLine("	UNION ALL");
                sb.AppendLine("	SELECT");
                sb.AppendLine("		R.[Id]");
                sb.AppendLine("	    ,R.[ParentHierarchicalStructureId]");
                sb.AppendLine("		,R.[HierarchyId]");
                sb.AppendLine("		,R.[ComponentTypeId]");
                sb.AppendLine("		,R.[ComponentId]");
                sb.AppendLine("	FROM [dbo].[HierarchicalStructure] AS R");
                sb.AppendLine("	INNER JOIN [Recursive] as RR");
                sb.AppendLine("		ON RR.[Id] = R.[ParentHierarchicalStructureId]");
                sb.AppendLine(")");

                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	[Id]");
                sb.AppendLine("	,[ParentHierarchicalStructureId]");
                sb.AppendLine("	,[HierarchyId]");
                sb.AppendLine("	,[ComponentTypeId]");
                sb.AppendLine("	,[ComponentId]");
                sb.AppendLine("FROM [Recursive]");

                var l = (await con.QueryAsync<HierarchicalStructure>(sb.ToString(), new { hierarchicalStructureId })).ToList();
                _searchAllowedHierarchicalStructureCache.Add(hierarchicalStructureId, l);

                return l;
            }
        }

        public async Task<List<int?>> SearchAllowedHierarchicalStructureIdAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion)
        {
            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	S.[HierarchicalStructureId]");
                sb.AppendLine("FROM [dbo].[Security] S");
                sb.AppendLine("INNER JOIN [dbo].[Role] R");
                sb.AppendLine("	ON S.[RoleId] = R.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[RoleFeature] FR");
                sb.AppendLine("	ON R.[Id] = FR.[RoleId]");
                sb.AppendLine("INNER JOIN [dbo].[Feature] F");
                sb.AppendLine("	ON FR.[FeatureId] = F.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[FeatureApi] FA");
                sb.AppendLine("	ON F.[Id] = FA.[FeatureId]");
                sb.AppendLine("INNER JOIN [dbo].[Api] A");
                sb.AppendLine("	ON FA.[ApiId] = A.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[Controller] C");
                sb.AppendLine("	ON A.[ControllerId] = C.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[Module] M");
                sb.AppendLine("	ON C.[ModuleId] = M.[Id]");
                sb.AppendLine("WHERE S.[IsActive] = 1");
                sb.AppendLine("AND S.[UserId] = @userId");
                sb.AppendLine("AND M.[ApplicationId] = @applicationId");
                sb.AppendLine("AND M.[CodeName] = @moduleCodeName");
                sb.AppendLine("AND C.[Name] = @controllerName");
                sb.AppendLine("AND A.[Name] = @apiName");
                sb.AppendLine("AND A.[HttpMethodId] = @apiHttpMethodId");
                sb.AppendLine("AND A.[Version] = @apiVersion");

                return (await con.QueryAsync<int?>(sb.ToString(), new { userId, applicationId, moduleCodeName, controllerName, apiName, apiHttpMethodId, apiVersion })).ToList();
            }
        }

        public async Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion)
        {
            var lHierarchicalStructureId = await SearchAllowedHierarchicalStructureIdAsync(userId, applicationId, moduleCodeName, controllerName, apiName, apiHttpMethodId, apiVersion);

            var l = new List<MdHierarchyComponent>();
            foreach (var iHierarchicalStructureId in lHierarchicalStructureId)
            {
                if (iHierarchicalStructureId != null)
                {
                    var lAllowed = await SearchAllowedHierarchyComponentAsync((int)iHierarchicalStructureId);
                    foreach (var iAllowed in lAllowed)
                        if (!l.Any(x => x.HierarchyId == iAllowed.HierarchyId && x.ComponentTypeId == iAllowed.ComponentTypeId && x.ComponentId == iAllowed.ComponentId))
                            l.Add(iAllowed);
                }
            }

            return l;
        }

        public async Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion)
        {
            var lHierarchicalStructureId = await SearchAllowedHierarchicalStructureIdAsync(userId, applicationId, moduleCodeName, controllerName, apiName, apiHttpMethodId, apiVersion);

            var l = new List<HierarchicalStructure>();
            foreach (var iHierarchicalStructureId in lHierarchicalStructureId)
            {
                if (iHierarchicalStructureId != null)
                {
                    var lAllowed = await SearchAllowedHierarchicalStructureAsync((int)iHierarchicalStructureId);
                    foreach (var iAllowed in lAllowed)
                        if (!l.Any(x => x.Id == iAllowed.Id))
                            l.Add(iAllowed);
                }
            }

            return l;
        }

        public async Task<List<int?>> SearchAllowedHierarchicalStructureIdAsync(string userId, int applicationId, string moduleCodeName, string featureCodeName)
        {
            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT");
                sb.AppendLine("	S.[HierarchicalStructureId]");
                sb.AppendLine("FROM [dbo].[Security] S");
                sb.AppendLine("INNER JOIN [dbo].[Role] R");
                sb.AppendLine("	ON S.[RoleId] = R.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[RoleFeature] FR");
                sb.AppendLine("	ON R.[Id] = FR.[RoleId]");
                sb.AppendLine("INNER JOIN [dbo].[Feature] F");
                sb.AppendLine("	ON FR.[FeatureId] = F.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[Module] M");
                sb.AppendLine("	ON F.[ModuleId] = M.[Id]");
                sb.AppendLine("WHERE S.[IsActive] = 1");
                sb.AppendLine("AND S.[UserId] = @userId");
                sb.AppendLine("AND M.[ApplicationId] = @applicationId");
                sb.AppendLine("AND M.[CodeName] = @moduleCodeName");
                sb.AppendLine("AND F.[CodeName] = @featureCodeName");

                return (await con.QueryAsync<int?>(sb.ToString(), new { userId, applicationId, moduleCodeName, featureCodeName })).ToList();
            }
        }

        public async Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync(string userId, int applicationId, string moduleCodeName, string featureCodeName)
        {
            var lHierarchicalStructureId = await SearchAllowedHierarchicalStructureIdAsync(userId, applicationId, moduleCodeName, featureCodeName);

            var l = new List<MdHierarchyComponent>();
            foreach (var iHierarchicalStructureId in lHierarchicalStructureId)
            {
                if (iHierarchicalStructureId != null)
                {
                    var lAllowed = await SearchAllowedHierarchyComponentAsync((int)iHierarchicalStructureId);
                    foreach (var iAllowed in lAllowed)
                        if (!l.Any(x => x.HierarchyId == iAllowed.HierarchyId && x.ComponentTypeId == iAllowed.ComponentTypeId && x.ComponentId == iAllowed.ComponentId))
                            l.Add(iAllowed);
                }
            }

            return l;
        }

        public async Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync(string userId, int applicationId, string moduleCodeName, string featureCodeName)
        {
            var lHierarchicalStructureId = await SearchAllowedHierarchicalStructureIdAsync(userId, applicationId, moduleCodeName, featureCodeName);

            var l = new List<HierarchicalStructure>();
            foreach (var iHierarchicalStructureId in lHierarchicalStructureId)
            {
                if (iHierarchicalStructureId != null)
                {
                    var lAllowed = await SearchAllowedHierarchicalStructureAsync((int)iHierarchicalStructureId);
                    foreach (var iAllowed in lAllowed)
                        if (!l.Any(x => x.Id == iAllowed.Id))
                            l.Add(iAllowed);
                }
            }

            return l;
        }

        public async Task<bool> IsAllowedApiAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion)
        {
            using (SqlConnection con = new SqlConnection(_configuration["NecnatServerSettings:AccessManagementConnectionString"]))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT");
                sb.AppendLine("	COUNT(1)");
                sb.AppendLine("FROM [dbo].[Security] S");
                sb.AppendLine("INNER JOIN [dbo].[Role] R");
                sb.AppendLine("	ON S.[RoleId] = R.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[RoleFeature] FR");
                sb.AppendLine("	ON R.[Id] = FR.[RoleId]");
                sb.AppendLine("INNER JOIN [dbo].[Feature] F");
                sb.AppendLine("	ON FR.[FeatureId] = F.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[FeatureApi] FA");
                sb.AppendLine("	ON F.[Id] = FA.[FeatureId]");
                sb.AppendLine("INNER JOIN [dbo].[Api] A");
                sb.AppendLine("	ON FA.[ApiId] = A.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[Controller] C");
                sb.AppendLine("	ON A.[ControllerId] = C.[Id]");
                sb.AppendLine("INNER JOIN [dbo].[Module] M");
                sb.AppendLine("	ON C.[ModuleId] = M.[Id]");
                sb.AppendLine("WHERE S.[IsActive] = 1");
                sb.AppendLine("AND S.[UserId] = @userId");
                sb.AppendLine("AND M.[ApplicationId] = @applicationId");
                sb.AppendLine("AND M.[CodeName] = @moduleCodeName");
                sb.AppendLine("AND C.[Name] = @controllerName");
                sb.AppendLine("AND A.[Name] = @apiName");
                sb.AppendLine("AND A.[HttpMethodId] = @apiHttpMethodId");
                sb.AppendLine("AND A.[Version] = @apiVersion");

                var count = await con.QueryAsync<int>(sb.ToString(), new { userId, applicationId, moduleCodeName, controllerName, apiName, apiHttpMethodId, apiVersion });
                return count.First() > 0;
            }
        }

        public async Task<bool> IsAllowedApiAsync(string userId, int applicationId, string moduleCodeName, string controllerName, string apiName, int apiHttpMethodId, string apiVersion, int componentTypeId, int componenteId)
        {
            var l = await SearchAllowedHierarchyComponentAsync(userId, applicationId, moduleCodeName, controllerName, apiName, apiHttpMethodId, apiVersion);
            return l.Any(x => x.ComponentTypeId == componentTypeId && x.ComponentId == componenteId);
        }

        public void ClearCache()
        {
            _searchAllowedHierarchyComponentCache = new Dictionary<int, List<MdHierarchyComponent>>();
        }
    }
}
