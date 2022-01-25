using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Necnat.Server.DbContexts;
using Necnat.Shared.Entities;
using Necnat.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Necnat.Server.Data.DataSeeds
{
    public partial class DataSeed
    {
        protected NecnatAccessManagementDbContext _context;
        protected NecnatServerSettings _necnatServerSettings;
        protected IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public Dictionary<string, int> IdentifierDict;

        public DataSeed(NecnatAccessManagementDbContext context,
            NecnatServerSettings necnatServerSettings,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _context = context;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _necnatServerSettings = necnatServerSettings;

            IdentifierDict = new Dictionary<string, int>();
        }

        protected async Task<int> AddApplicationAsync(string name, string acronym = null, string description = null, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Application.Where(x =>
                x.Acronym == acronym
                && x.Name == name).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Application();
                e.Acronym = acronym;
                e.Name = name;
                e.Description = description;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey("Application:" + name))
                IdentifierDict.Add("Application:" + name, e.Id);

            return e.Id;
        }

        protected async Task<int> AddApplicationHierarchyAsync(int applicationId, int hierarchyId, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.ApplicationHierarchy.Where(x =>
                x.ApplicationId == applicationId
                && x.HierarchyId == hierarchyId).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new ApplicationHierarchy();
                e.ApplicationId = applicationId;
                e.HierarchyId = hierarchyId;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            return e.Id;
        }

        protected async Task<int> AddControllerAsync(int moduleId, string name, string description = null, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Controller.Where(x =>
                x.ModuleId == moduleId
                && x.Name == name).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Controller();
                e.ModuleId = moduleId;
                e.Name = name;
                e.Description = description;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey(moduleId + "Controller:" + name))
                IdentifierDict.Add(moduleId + "Controller:" + name, e.Id);

            return e.Id;
        }

        protected async Task<int> AddApiAsync(int controllerId, string name, int httpMethodId, string version, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Api.Where(x =>
                x.ControllerId == controllerId
                && x.Name == name
                && x.HttpMethodId == httpMethodId
                && x.Version == version).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Api();
                e.ControllerId = controllerId;
                e.Name = name;
                e.HttpMethodId = (int)httpMethodId;
                e.Version = version;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey(controllerId + "Api:" + name))
                IdentifierDict.Add(controllerId + "Api:" + name, e.Id);

            return e.Id;
        }

        protected async Task<int> AddFeatureAsync(int moduleId, string codeName, string name = null, string description = null, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Feature.Where(x =>
                x.ModuleId == moduleId
                && x.CodeName == codeName).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Feature();
                e.ModuleId = moduleId;
                e.CodeName = codeName;
                e.Name = name ?? codeName;
                e.Description = description;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey(moduleId + "Feature:" + codeName))
                IdentifierDict.Add(moduleId + "Feature:" + codeName, e.Id);

            return e.Id;
        }

        protected async Task<int> AddFeatureApiAsync(int featureId, int apiId, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.FeatureApi.Where(x =>
                x.FeatureId == featureId
                && x.ApiId == apiId).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new FeatureApi();
                e.FeatureId = featureId;
                e.ApiId = apiId;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            return e.Id;
        }

        protected async Task<int> AddFeatureApiAsync(string moduleCodeName, string featureCodeName, string controllerName, string apiName)
        {
            var module = _context.Module.Where(x => x.CodeName == moduleCodeName).FirstOrDefault();
            if (module == null)
                return 0;

            var api = _context.Api.Where(x => x.Name == apiName && x.Controller.Name == controllerName && x.Controller.ModuleId == module.Id).FirstOrDefault();
            if (api == null)
                return 0;

            var featureId = await AddFeatureAsync(module.Id, featureCodeName);
            return await AddFeatureApiAsync(featureId, api.Id);
        }

        protected async Task<int> AddHierarchicalStructureAsync(int hierarchyId, int componentTypeId, int componentId, int? parentHierarchicalStructureId = null, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.HierarchicalStructure.Where(x =>
                x.ParentHierarchicalStructureId == parentHierarchicalStructureId
                && x.HierarchyId == hierarchyId
                && x.ComponentTypeId == componentTypeId
                && x.ComponentId == componentId).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new HierarchicalStructure();
                e.ParentHierarchicalStructureId = parentHierarchicalStructureId;
                e.HierarchyId = hierarchyId;
                e.ComponentTypeId = componentTypeId;
                e.ComponentId = componentId;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            return e.Id;
        }

        protected async Task<int> AddHierarchyAsync(string name, string description = null, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Hierarchy.Where(x =>
                x.Name == name).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Hierarchy();
                e.Name = name;
                e.Description = description;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey("Hierarchy:" + name))
                IdentifierDict.Add("Hierarchy:" + name, e.Id);

            return e.Id;
        }

        protected async Task<int> AddHierarchyComponentTypeAsync(string name, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.HierarchyComponentType.Where(x =>
                x.Name == name).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new HierarchyComponentType();
                e.Name = name;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey("HierarchyComponentType:" + name))
                IdentifierDict.Add("HierarchyComponentType:" + name, e.Id);

            return e.Id;
        }

        protected async Task<int> AddHierarchyHierarchyComponentTypeAsync(int hierarchyId, int hierarchyComponentTypeId, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.HierarchyHierarchyComponentType.Where(x =>
                x.HierarchyId == hierarchyId
                && x.HierarchyComponentTypeId == hierarchyComponentTypeId).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new HierarchyHierarchyComponentType();
                e.HierarchyId = hierarchyId;
                e.HierarchyComponentTypeId = hierarchyComponentTypeId;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            return e.Id;
        }

        protected async Task<int> AddModuleAsync(int applicationId, string codeName, string name = null, string description = null, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Module.Where(x =>
                x.ApplicationId == applicationId
                && x.CodeName == codeName).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Module();
                e.ApplicationId = applicationId;
                e.CodeName = codeName;
                e.Name = name ?? codeName;
                e.Description = description;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey(applicationId + "Module:" + codeName))
                IdentifierDict.Add(applicationId + "Module:" + codeName, e.Id);

            return e.Id;
        }

        protected async Task<int> AddRoleAsync(int applicationId, string codeName, string name = null, string description = null, bool isActive = true, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Role.Where(x =>
                x.ApplicationId == applicationId
                && x.CodeName == codeName).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Role();
                e.ApplicationId = applicationId;
                e.CodeName = codeName;
                e.Name = name ?? codeName;
                e.Description = description;
                e.IsActive = isActive;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            if (!IdentifierDict.ContainsKey(applicationId + "Role:" + codeName))
                IdentifierDict.Add(applicationId + "Role:" + codeName, e.Id);

            return e.Id;
        }

        protected async Task<int> AddRoleFeatureAsync(int roleId, int featureId, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.RoleFeature.Where(x =>
                x.RoleId == roleId
                && x.FeatureId == featureId).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new RoleFeature();
                e.RoleId = roleId;
                e.FeatureId = featureId;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            return e.Id;
        }

        protected async Task<int> AddRoleFeatureAsync(int roleId, int applicationId, string moduleCodeName, string featureCodeName)
        {
            var moduleId = await AddModuleAsync(applicationId, moduleCodeName);
            var featureId = await AddFeatureAsync(moduleId, featureCodeName);
            var roleFeature = _context.RoleFeature.Where(x => x.FeatureId == featureId && x.RoleId == roleId).FirstOrDefault();
            if (roleFeature == null)
            {
                roleFeature = new RoleFeature();
                roleFeature.FeatureId = featureId;
                roleFeature.RoleId = roleId;
                await _context.AddAsync(roleFeature);

                await _context.SaveChangesAsync();
            }

            return roleFeature.Id;
        }

        protected async Task<int> AddSecurityAsync(string userId, int roleId, int hierarchicalStructureId, string actionUserId = "00000000-0000-0000-0000-000000000000")
        {
            var e = await _context.Security.Where(x =>
                x.UserId == userId
                && x.RoleId == roleId
                && x.HierarchicalStructureId == hierarchicalStructureId).FirstOrDefaultAsync();
            if (e == null)
            {
                e = new Security();
                e.UserId = userId;
                e.RoleId = roleId;
                e.HierarchicalStructureId = hierarchicalStructureId;
                e.IsActive = true;
                await _context.AddAsync(e);

                await _context.SaveChangesAsync();
            }

            return e.Id;
        }

        protected async Task<int> AddSecurityAsync(string userId, int roleId, int? hierarchyId, int? componentTypeId, int? componentId)
        {
            var e = _context.HierarchicalStructure.Where(x => x.HierarchyId == hierarchyId && x.ComponentTypeId == componentTypeId && x.ComponentId == componentId).FirstOrDefault();
            if (e == null)
                return 0;

            return await AddSecurityAsync(userId, roleId, e.Id);
        }
    }
}
