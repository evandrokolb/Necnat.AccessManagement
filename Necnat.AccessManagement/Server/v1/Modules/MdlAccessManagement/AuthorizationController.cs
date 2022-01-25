using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.AccessManagement.Server.Data.DataSeeds;
using Necnat.Server.Bases;
using Necnat.Server.Data.DataSeeds;
using Necnat.Server.DbContexts;
using Necnat.Shared.Models;
using Necnat.Shared.Resources;
using Necnat.Shared.Settings;
using System.Threading.Tasks;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlAccessManagement
{
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthorizationController : NnControllerBase
    {
        IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        NecnatServerSettings _necnatServerSettings;
        NecnatAccessManagementDbContext _accessManagementContext;

        public AuthorizationController(
            IConfiguration configuration,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            IAuthorizationService authorizationService,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            NecnatServerSettings necnatServerSettings,
            NecnatAccessManagementDbContext accessManagementContext) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _necnatServerSettings = necnatServerSettings;
            _accessManagementContext = accessManagementContext;
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AGetRoleWithoutHierarchyByApplicationId")]
        public async Task<IActionResult> AGetRoleWithoutHierarchyByApplicationId([FromQuery] string culture, [FromQuery] int applicationId)
        {
            var userId = GetUserId();

            var m = new MdUserAuthorization();
            m.UserId = userId;
            m.ApplicationId = applicationId;
            m.AllowedRoleList = await _authorizationService.SearchAllowedRoleListWithoutHierarchyAsync(userId, applicationId);

            return StatusCode(200, m.ToReduced());
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AGetRoleWithHierarchyByApplicationId")]
        public async Task<IActionResult> AGetRoleWithHierarchyByApplicationId([FromQuery] string culture, [FromQuery] int applicationId)
        {
            var userId = GetUserId();

            var m = new MdUserAuthorization();
            m.UserId = userId;
            m.ApplicationId = applicationId;
            m.AllowedRoleList = await _authorizationService.SearchAllowedRoleListAsync(userId, applicationId);

            return StatusCode(200, m.ToReduced());
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AGetFeatureWithoutHierarchyByApplicationId")]
        public async Task<IActionResult> AGetFeatureWithoutHierarchyByApplicationId([FromQuery] string culture, [FromQuery] int applicationId)
        {
            var userId = GetUserId();

            var m = new MdUserAuthorization();
            m.UserId = userId;
            m.ApplicationId = applicationId;
            m.AllowedModuleFeatureList = await _authorizationService.SearchAllowedFeatureListWithoutHierarchyAsync(userId, applicationId);

            return StatusCode(200, m.ToReduced());
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AGetFeatureWithHierarchyByApplicationId")]
        public async Task<IActionResult> AGetFeatureWithHierarchyByApplicationId([FromQuery] string culture, [FromQuery] int applicationId)
        {
            var userId = GetUserId();

            var m = new MdUserAuthorization();
            m.UserId = userId;
            m.ApplicationId = applicationId;
            m.AllowedModuleFeatureList = await _authorizationService.SearchAllowedFeatureListAsync(userId, applicationId);

            return StatusCode(200, m.ToReduced());
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/DataSeed")]
        public async Task<IActionResult> DataSeed([FromQuery] string culture)
        {
            var userId = GetUserId();

            var dataSeed = new AccessManagementDataSeed(_accessManagementContext, _necnatServerSettings, _actionDescriptorCollectionProvider);
            await dataSeed.ApplicationDataSeedAsync();
            await dataSeed.ApiDataSeedAsync();
            await dataSeed.FeatureDataSeedAsync();
            await dataSeed.RoleDataSeedAsync();

            return StatusCode(200);
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/InitialUsersDataSeed")]
        public async Task<IActionResult> InitialUsersDataSeed([FromQuery] string culture)
        {
            var userId = GetUserId();

            var dataSeed = new AccessManagementDataSeed(_accessManagementContext, _necnatServerSettings, _actionDescriptorCollectionProvider);
            await dataSeed.ApplicationDataSeedAsync();
            await dataSeed.ApiDataSeedAsync();
            await dataSeed.FeatureDataSeedAsync();
            await dataSeed.RoleDataSeedAsync();
            await dataSeed.MasterUserDataSeedAsync("1");
            await dataSeed.MiddleMockHierarchyUserDataSeedAsync("11");

            return StatusCode(200);
        }
    }
}
