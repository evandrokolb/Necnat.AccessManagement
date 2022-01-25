using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlDevelopment;
using Necnat.Server.Bases;
using Necnat.Shared.Constants;
using Necnat.Shared.Entities;
using Necnat.Shared.Exceptions;
using Necnat.Shared.Filters;
using Necnat.Shared.Models;
using Necnat.Shared.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlDevelopment
{
    [ApiController]
    [ApiVersion("1.0")]
    public class ApplicationController : NnControllerBase
    {
        ApplicationService _service;

        public ApplicationController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            ApplicationService service) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _service = service;
        }

        /// <summary>
        /// Obtains all allowed records from that entity.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <response code="200">Collection of the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/AList")]
        public async Task<IActionResult> AGetAll([FromQuery] string culture)
        {
            var lAuth = await SearchAllowedHierarchyComponentIdAsync((int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.SearchByIdListAsync(lAuth));
        }

        /// <summary>
        /// Obtains the record of the entity with that id, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/AGetById")]
        public async Task<IActionResult> AGetById([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdAsync(id);

            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.Id);

            return StatusCode(200, e);
        }

        /// <summary>
        /// Insert the entity and add her to Hierarchy of Applications.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/InsertAndAddToHierarchyApplications")]
        public async Task<IActionResult> InsertAndAddToHierarchyApplications([FromQuery] string culture, [FromBody] Application e)
        {
            var userId = await CheckAllowedApiAsync();

            return StatusCode(200, new { Key = await _service.InsertAndAddToHierarchyApplicationsAsync(e, userId), Message = _commonApiLocalizer["ENTITY_SUCCESSFULLY_INCLUDED"].Value });
        }

        /// <summary>
        /// Update the record of the corresponding entity, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPut]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/AUpdate")]
        public async Task<IActionResult> AUpdate([FromQuery] string culture, [FromBody] Application e)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.Id);

            await _service.UpdateAsync(e, e.Id, userId);
            return StatusCode(200, _commonApiLocalizer["ENTITY_SUCCESSFULLY_UPDATED"].Value);
        }

        /// <summary>
        /// Delete the record of the entity with that id and delete all corresponding records from hierarchies, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">A success message.</response>
        [HttpDelete]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/ADeleteAndRemoveFromHierarchicalStructure")]
        public async Task<IActionResult> ADeleteAndRemoveFromHierarchicalStructure([FromQuery] string culture, [FromQuery] int id)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, id);

            await _service.DeleteAndRemoveFromHierarchicalStructureAsync(id, userId);
            return StatusCode(200, _commonApiLocalizer["ENTITY_SUCCESSFULLY_DELETED"].Value);
        }

        /// <summary>
        /// Obtains all allowed records from that entity, corresponding to the filter.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="moduleCodeName">Module code name.</param>
        /// <param name="featureCodeName">Feature code name.</param>
        /// <param name="md">Filter parameters.</param>
        /// <response code="200">Collection of the entity.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/AFilter")]
        public async Task<IActionResult> AFilter([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdApplicationFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleDevelopment
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentApplicationRead }
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            var l = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.FilterByIdListAsync(md, l));
        }

        /// <summary>
        /// Obtains all minimal information of the allowed records from that entity, corresponding to the filter.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="moduleCodeName">Module code name.</param>
        /// <param name="featureCodeName">Feature code name.</param>
        /// <param name="md">Filter parameters.</param>
        /// <response code="200">Collection of the entity.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/AFilterSupport")]
        public async Task<IActionResult> AFilterSupport([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdApplicationFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleDevelopment
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentModuleRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentModuleInsert },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentModuleUpdate },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentControllerRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentControllerInsert },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentControllerUpdate },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentApiRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentApiInsert },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentApiUpdate },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentFeatureRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentFeatureInsert },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentFeatureUpdate },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentFeatureApiRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentFeatureApiInsert },
                //ModuleAccessConfiguration
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessConfiguration, FeatureCodeName = NamFeatureConstants.FeatureAccessConfigurationRoleRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessConfiguration, FeatureCodeName = NamFeatureConstants.FeatureAccessConfigurationRoleInsert },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessConfiguration, FeatureCodeName = NamFeatureConstants.FeatureAccessConfigurationRoleUpdate },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessConfiguration, FeatureCodeName = NamFeatureConstants.FeatureAccessConfigurationRoleFeatureRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessConfiguration, FeatureCodeName = NamFeatureConstants.FeatureAccessConfigurationRoleFeatureInsert },
            };
            if(!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            var l = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.FilterByIdListAsync(md, l, true));
        }
    }
}