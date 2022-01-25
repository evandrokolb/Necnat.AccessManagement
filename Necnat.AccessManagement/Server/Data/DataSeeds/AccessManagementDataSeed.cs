using Microsoft.AspNetCore.Mvc.Infrastructure;
using Necnat.Server.Data.DataSeeds;
using Necnat.Server.DbContexts;
using Necnat.Shared.Constants;
using Necnat.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.AccessManagement.Server.Data.DataSeeds
{
    public class AccessManagementDataSeed : DataSeed
    {
        public AccessManagementDataSeed(NecnatAccessManagementDbContext context,
            NecnatServerSettings necnatServerSettings,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) : base(context, necnatServerSettings, actionDescriptorCollectionProvider) { }

        public async Task ApplicationDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                //HierarchyComponentType
                await AddHierarchyComponentTypeAsync("Hierarchy");
                await AddHierarchyComponentTypeAsync("Application");

                //Hierarchy - HierarchyHierarchiesName
                await AddHierarchyAsync(NamFeatureConstants.HierarchyHierarchiesName);
                var hierarchyHierarchiesParentId = await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName]);
                await AddHierarchyHierarchyComponentTypeAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName], (int)NamHierarchyComponentTypeConstants.Hierarchy);

                //Hierarchy - HierarchyApplicationsName
                await AddHierarchyAsync(NamFeatureConstants.HierarchyApplicationsName);
                var hierarchyApplicationsParentId = await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName]);
                await AddHierarchyHierarchyComponentTypeAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Application);
                await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], hierarchyHierarchiesParentId);

                //Application
                await AddApplicationAsync("Necnat Access Management", "NAM");
                await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Application, IdentifierDict["Application:Necnat Access Management"], hierarchyApplicationsParentId);

                //Mock
                var h1 = await MockApplicationHierarchy(hierarchyHierarchiesParentId, 1);

                var a1 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", h1, 1);
                var a2 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", h1, 2);
                var a3 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", a1, 3);
                var a4 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", a1, 4);
                var a5 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", a2, 5);
                var a6 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", a2, 6);
                var a7 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", a3, 7);
                var a8 = await MockApplication(hierarchyApplicationsParentId, "Mock Hierarchy 1", a4, 8);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RoleDataSeedAsync()
        {
            await RoleDeveloperDataSeedAsync();
            await RoleAccessConfiguratorDataSeedAsync();
            await RoleHierarchyManagerDataSeedAsync();
            await RoleHierarchicalStructureManagerDataSeedAsync();
            await RoleAccessManagerApplicationDataSeedAsync();
            await RoleAccessManagerHierarchyDataSeedAsync();
        }

        private async Task RoleDeveloperDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await AddRoleAsync(IdentifierDict["Application:Necnat Access Management"], "Developer");

                //Application
                await AddFeatureApiAsync(NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationInsert, "Application", "InsertAndAddToHierarchyApplications");
                await AddFeatureApiAsync(NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationDelete, "Application", "ADeleteAndRemoveFromHierarchicalStructure");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationDelete);

                //ApplicationHierarchy
                await AddFeatureApiAsync(NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationHierarchyRead, "ApplicationHierarchy", "ASearchByApplicationIdIncludeHierarchy");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationHierarchyRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationHierarchyInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApplicationHierarchyDelete);

                //Module
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentModuleRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentModuleInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentModuleUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentModuleDelete);

                //Controller
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentControllerRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentControllerInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentControllerUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentControllerDelete);

                //Api
                await AddFeatureApiAsync(NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApiRead, "Api", "ASearchByControllerId");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApiRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApiInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApiUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentApiDelete);

                //Feature
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureDelete);

                //FeatureApi
                await AddFeatureApiAsync(NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureApiRead, "FeatureApi", "ASearchByFeatureIdIncludeApi");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureApiRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureApiInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleDevelopment, NamFeatureConstants.FeatureDevelopmentFeatureApiDelete);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RoleAccessConfiguratorDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await AddRoleAsync(IdentifierDict["Application:Necnat Access Management"], "Access Configurator");

                //Role
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleDelete);

                //RoleFeature
                await AddFeatureApiAsync(NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleFeatureRead, "RoleFeature", "ASearchByRoleIdIncludeFeature");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleFeatureRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleFeatureInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessConfiguration, NamFeatureConstants.FeatureAccessConfigurationRoleFeatureDelete);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RoleHierarchyManagerDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await AddRoleAsync(IdentifierDict["Application:Necnat Access Management"], "Hierarchy Manager");

                //Hierarchy            
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyInsert, "Hierarchy", "InsertAndAddToHierarchyHierarchies");
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyDelete, "Hierarchy", "ADeleteAndRemoveFromHierarchicalStructure");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyDelete);

                //HierarchyComponentType
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyComponentTypeRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyComponentTypeInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyComponentTypeUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyComponentTypeDelete);

                //HierarchyHierarchyComponentType
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyHierarchyComponentTypeRead, "HierarchyHierarchyComponentType", "ASearchByHierarchyIdIncludeHierarchyComponentType");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyHierarchyComponentTypeRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyHierarchyComponentTypeInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchyHierarchyComponentTypeDelete);

                //HierarchicalStructure
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead, "HierarchicalStructure", "ASearchTreeByHierarchyId");
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead, "HierarchicalStructure", "ASearchHierarchyComponentByHierarchyId");
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead, "HierarchicalStructure", "ASearchHierarchyOfHierarchicalStructure");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureDelete);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RoleHierarchicalStructureManagerDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await AddRoleAsync(IdentifierDict["Application:Necnat Access Management"], "Hierarchical Structure Manager");

                //HierarchicalStructure
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead, "HierarchicalStructure", "ASearchHierarchyForTree");
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead, "HierarchicalStructure", "ASearchTree");
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead, "HierarchicalStructure", "ASearchTreeById");
                await AddFeatureApiAsync(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead, "HierarchicalStructure", "ASearchHierarchyComponentByHierarchyId");

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchical Structure Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchical Structure Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchical Structure Manager"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureDelete);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RoleAccessManagerApplicationDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await AddRoleAsync(IdentifierDict["Application:Necnat Access Management"], "Access Manager Application");

                //Security
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Application"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementSecurityRead);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Application"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementSecurityInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Application"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementSecurityUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Application"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementSecurityDelete);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RoleAccessManagerHierarchyDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await AddRoleAsync(IdentifierDict["Application:Necnat Access Management"], "Access Manager Hierarchy");

                //Security
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureRead);

                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementSecurityInsert);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementSecurityUpdate);
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementSecurityDelete);

                //User
                await AddRoleFeatureAsync(IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Application:Necnat Access Management"], NamFeatureConstants.ModuleAccessManagement, NamFeatureConstants.FeatureAccessManagementUserRead);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task MasterUserDataSeedAsync(string userId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                //Security
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Developer"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName]);
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Configurator"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName]);
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchy Manager"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName]);
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchical Structure Manager"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName]);
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Application"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName]);
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName]);
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName]);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<int> MockApplicationHierarchy(int hierarchyHierarchiesParentId, int i)
        {
            await AddHierarchyAsync("Mock Hierarchy " + i.ToString());
            var parentId = await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:Mock Hierarchy " + i.ToString()], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:Mock Hierarchy " + i.ToString()]);

            await AddHierarchyHierarchyComponentTypeAsync(IdentifierDict["Hierarchy:Mock Hierarchy " + i.ToString()], (int)NamHierarchyComponentTypeConstants.Application);

            await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyHierarchiesName], (int)NamHierarchyComponentTypeConstants.Hierarchy, IdentifierDict["Hierarchy:Mock Hierarchy " + i.ToString()], hierarchyHierarchiesParentId);

            return parentId;
        }

        private async Task<int> MockApplication(int hierarchyApplicationsParentId, string hierarchyName, int parentHierarchyStructureId, int i)
        {
            await AddApplicationAsync("Mock Application " + i.ToString(), "T" + i);

            await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Application, IdentifierDict["Application:Necnat Access Management"], hierarchyApplicationsParentId);

            return await AddHierarchicalStructureAsync(IdentifierDict["Hierarchy:" + hierarchyName], (int)NamHierarchyComponentTypeConstants.Application, IdentifierDict["Application:Mock Application " + i.ToString()], parentHierarchyStructureId);

        }

        public async Task MiddleMockHierarchyUserDataSeedAsync(string userId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                //Security
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Hierarchical Structure Manager"], IdentifierDict["Hierarchy:Mock Hierarchy 1"], (int)NamHierarchyComponentTypeConstants.Application, IdentifierDict["Application:Mock Application 3"]);

                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Application"], IdentifierDict["Hierarchy:" + NamFeatureConstants.HierarchyApplicationsName], (int)NamHierarchyComponentTypeConstants.Application, IdentifierDict["Application:Necnat Access Management"]);
                await AddSecurityAsync(userId, IdentifierDict[IdentifierDict["Application:Necnat Access Management"] + "Role:Access Manager Hierarchy"], IdentifierDict["Hierarchy:Mock Hierarchy 1"], (int)NamHierarchyComponentTypeConstants.Application, IdentifierDict["Application:Mock Application 3"]);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
