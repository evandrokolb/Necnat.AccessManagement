using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.Server.Interfaces;
using Necnat.Shared.Domains;
using Necnat.Shared.Entities;
using Necnat.Shared.Exceptions;
using Necnat.Shared.Models;
using Necnat.Shared.Resources;
using Necnat.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Necnat.Server.Bases
{
    public abstract class NnControllerBase : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IAuthorizationService _authorizationService;
        protected readonly IStringLocalizer<CommonApiLocalizer> _commonApiLocalizer;

        protected int _applicationId;

        public NnControllerBase(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer)
        {
            _configuration = configuration;
            _authorizationService = authorizationService;
            _commonApiLocalizer = commonApiLocalizer;

            _applicationId = int.Parse(configuration["NecnatServerSettings:ApplicationId"]);
        }

        protected virtual string GetUserId()
        {
            var userIdClaim = User.FindFirst("sub");
            if (userIdClaim == null)
                throw new BusinessException(_commonApiLocalizer["COMMON3_UNABLE_IDENTIFY_USER"].Value);

            return userIdClaim.Value;
        }

        protected virtual async Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync()
        {
            var userId = GetUserId();
            var m = ConvertToMdCheckAllowedApi(Request);

            return await _authorizationService.SearchAllowedHierarchyComponentAsync(userId, _applicationId, m.ModuleCodeName, m.ControllerName, m.ApiName, m.HttpMethodId, m.ApiVersion);
        }

        protected virtual async Task<List<MdHierarchyComponent>> SearchAllowedHierarchyComponentAsync(string moduleCodeName, string featureCodeName)
        {
            var userId = GetUserId();

            return await _authorizationService.SearchAllowedHierarchyComponentAsync(userId, _applicationId, moduleCodeName, featureCodeName);
        }

        protected virtual async Task<List<int>> SearchAllowedHierarchyComponentIdAsync(int componentTypeId)
        {
            var userId = GetUserId();
            var m = ConvertToMdCheckAllowedApi(Request);

            var l = await _authorizationService.SearchAllowedHierarchyComponentAsync(userId, _applicationId, m.ModuleCodeName, m.ControllerName, m.ApiName, m.HttpMethodId, m.ApiVersion);
            return l.Where(x => x.ComponentTypeId == componentTypeId).Select(x => x.ComponentId).ToList();
        }

        protected virtual async Task<List<int>> SearchAllowedHierarchyComponentIdAsync(string moduleCodeName, string featureCodeName, int componentTypeId)
        {
            var userId = GetUserId();

            var l = await _authorizationService.SearchAllowedHierarchyComponentAsync(userId, _applicationId, moduleCodeName, featureCodeName);
            return l.Where(x => x.ComponentTypeId == componentTypeId).Select(x => x.ComponentId).ToList();
        }

        protected virtual async Task<List<int?>> SearchAllowedHierarchicalStructureIdAsync()
        {
            var userId = GetUserId();
            var m = ConvertToMdCheckAllowedApi(Request);

            return await _authorizationService.SearchAllowedHierarchicalStructureIdAsync(userId, _applicationId, m.ModuleCodeName, m.ControllerName, m.ApiName, m.HttpMethodId, m.ApiVersion);
        }

        protected virtual async Task<List<int?>> SearchAllowedHierarchicalStructureIdAsync(string moduleCodeName, string featureCodeName)
        {
            var userId = GetUserId();

            return await _authorizationService.SearchAllowedHierarchicalStructureIdAsync(userId, _applicationId, moduleCodeName, featureCodeName);
        }

        protected virtual async Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync()
        {
            var userId = GetUserId();
            var m = ConvertToMdCheckAllowedApi(Request);

            return await _authorizationService.SearchAllowedHierarchicalStructureAsync(userId, _applicationId, m.ModuleCodeName, m.ControllerName, m.ApiName, m.HttpMethodId, m.ApiVersion);
        }

        protected virtual async Task<List<HierarchicalStructure>> SearchAllowedHierarchicalStructureAsync(string moduleCodeName, string featureCodeName)
        {
            var userId = GetUserId();

            return await _authorizationService.SearchAllowedHierarchicalStructureAsync(userId, _applicationId, moduleCodeName, featureCodeName);
        }

        protected virtual async Task<string> CheckAllowedApiAsync()
        {
            var userId = GetUserId();
            var m = ConvertToMdCheckAllowedApi(Request);

            if (!await _authorizationService.IsAllowedApiAsync(userId, _applicationId, m.ModuleCodeName, m.ControllerName, m.ApiName, m.HttpMethodId, m.ApiVersion))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            return userId;
        }

        protected virtual async Task<string> CheckAllowedApiAsync(int componenteTypeId, int componenteId)
        {
            var userId = GetUserId();
            var m = ConvertToMdCheckAllowedApi(Request);

            if (!await _authorizationService.IsAllowedApiAsync(userId, _applicationId, m.ModuleCodeName, m.ControllerName, m.ApiName, m.HttpMethodId, m.ApiVersion, componenteTypeId, componenteId))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            return userId;
        }

        protected virtual async Task<string> CheckAllowedFeatureAsync(string moduleCodeName, string featureCodeName, int componenteTypeId, int componenteId)
        {
            var userId = GetUserId();

            var l = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, componenteTypeId);

            if (!l.Contains(componenteId))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            return userId;
        }

        protected virtual MdCheckAllowedApi ConvertToMdCheckAllowedApi(HttpRequest request)
        {
            var necnatServerSettings = _configuration.GetSection(nameof(NecnatServerSettings)).Get<NecnatServerSettings>();
            var split = request.Path.Value.Substring(necnatServerSettings.ApiRouteSettings.ApiPerfix.Length + 1).Split('/');

            var sm = new MdCheckAllowedApi();
            sm.ModuleCodeName = necnatServerSettings.ApiRouteSettings.ModuleOrder > -1 ? split[necnatServerSettings.ApiRouteSettings.ModuleOrder] : necnatServerSettings.ApiRouteSettings.NoModuleDefault;
            sm.ControllerName = split[necnatServerSettings.ApiRouteSettings.ControllerOrder];
            sm.ApiName = split[necnatServerSettings.ApiRouteSettings.ApiNameOrder];
            sm.HttpMethodId = (int)HttpMethodDomain.GetByName(request.Method).Id;
            sm.ApiVersion = necnatServerSettings.ApiRouteSettings.ApiVersionOrder > -1 ? split[necnatServerSettings.ApiRouteSettings.ApiVersionOrder] : necnatServerSettings.ApiRouteSettings.NoApiVersionDefault;

            return sm;
        }
    }
}
