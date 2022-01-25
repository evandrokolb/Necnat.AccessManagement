using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.Server.Bases;
using Necnat.Shared.Constants;
using Necnat.Shared.Entities;
using Necnat.Shared.Filters;
using Necnat.Shared.Resources;
using System.Threading.Tasks;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlHierarchyManagement;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;
using Necnat.Shared.Models;
using System.Collections.Generic;
using Necnat.Shared.Exceptions;
using System.Linq;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlDevelopment
{
    [ApiController]
    [ApiVersion("1.0")]
    public class HierarchyController : NnControllerBase
    {
        HierarchyService _service;

        public HierarchyController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            HierarchyService service) : base(configuration, authorizationService, commonApiLocalizer)
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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/AList")]
        public async Task<IActionResult> AList([FromQuery] string culture)
        {
            var lAuth = await SearchAllowedHierarchyComponentIdAsync((int)NamHierarchyComponentTypeConstants.Hierarchy);

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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/AGetById")]
        public async Task<IActionResult> AGetById([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdAsync(id);

            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Hierarchy, e.Id);

            return StatusCode(200, e);
        }

        /// <summary>
        /// Insert the entity and add her to Hierarchy of Hierarchies.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/InsertAndAddToHierarchyHierarchies")]
        public async Task<IActionResult> InsertAndAddToHierarchyHierarchies([FromQuery] string culture, [FromBody] Hierarchy e)
        {
            var userId = await CheckAllowedApiAsync();

            return StatusCode(200, new { Key = await _service.InsertAndAddToHierarchyHierarchiesAsync(e, userId), Message = _commonApiLocalizer["ENTITY_SUCCESSFULLY_INCLUDED"].Value });
        }

        /// <summary>
        /// Update the record of the corresponding entity, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPut]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/AUpdate")]
        public async Task<IActionResult> AUpdate([FromQuery] string culture, [FromBody] Hierarchy e)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Hierarchy, e.Id);

            await _service.UpdateAsync(e, e.Id, userId);

            return StatusCode(200, _commonApiLocalizer["ENTITY_SUCCESSFULLY_UPDATED"].Value);
        }

        /// <summary>
        /// Delete the record of the entity with that id, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">A success message.</response>
        [HttpDelete]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/ADeleteAndRemoveFromHierarchicalStructure")]
        public async Task<IActionResult> ADeleteAndRemoveFromHierarchicalStructure([FromQuery] string culture, [FromQuery] int id)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Hierarchy, id);

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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/AFilter")]
        public async Task<IActionResult> AFilter([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdHierarchyFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleDevelopment
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentApplicationHierarchyInsert },
                //ModuleHierarchyManagement
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleHierarchyManagement, FeatureCodeName = NamFeatureConstants.FeatureHierarchyManagementHierarchyRead },                
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            var lAuth = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Hierarchy);

            return StatusCode(200, await _service.FilterByIdListAsync(md, lAuth));
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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/AFilterSupport")]
        public async Task<IActionResult> AFilterSupport([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdHierarchyFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            var lAuth = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Hierarchy);

            return StatusCode(200, await _service.FilterByIdListAsync(md, lAuth, true));
        }
    }
}