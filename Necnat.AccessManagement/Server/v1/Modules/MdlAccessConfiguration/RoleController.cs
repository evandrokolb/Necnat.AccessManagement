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
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessConfiguration;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;
using System.Collections.Generic;
using Necnat.Shared.Models;
using System.Linq;
using Necnat.Shared.Exceptions;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlAccessConfiguration
{
    [ApiController]
    [ApiVersion("1.0")]
    public class RoleController : NnControllerBase
    {
        RoleService _service;

        public RoleController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            RoleService service) : base(configuration, authorizationService, commonApiLocalizer)
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
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/AList")]
        public async Task<IActionResult> AList([FromQuery] string culture)
        {
            var lAuth = await SearchAllowedHierarchyComponentIdAsync((int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.SearchByApplicationIdListAsync(lAuth));
        }

        /// <summary>
        /// Obtains the record of the entity with that id, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/AGetById")]
        public async Task<IActionResult> AGetById([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdAsync(id);

            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.ApplicationId);

            return StatusCode(200, e);
        }

        /// <summary>
        /// Insert the entity, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/AInsert")]
        public async Task<IActionResult> AInsert([FromQuery] string culture, [FromBody] Role e)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.ApplicationId);

            return StatusCode(200, new { Key = await _service.InsertAsync(e, userId), Message = _commonApiLocalizer["ENTITY_SUCCESSFULLY_INCLUDED"].Value });
        }

        /// <summary>
        /// Update the record of the corresponding entity, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPut]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/AUpdate")]
        public async Task<IActionResult> AUpdate([FromQuery] string culture, [FromBody] Role e)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.ApplicationId);

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
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/ADelete")]
        public async Task<IActionResult> ADelete([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdAsync(id);

            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.ApplicationId);

            await _service.DeleteAsync(e.Id, userId);

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
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/AFilter")]
        public async Task<IActionResult> AFilter([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdRoleFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleDevelopment
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessConfiguration, FeatureCodeName = NamFeatureConstants.FeatureAccessConfigurationRoleRead }
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            var lAuth = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.FilterByApplicationIdListAsync(md, lAuth));
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
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/AFilterSupport")]
        public async Task<IActionResult> AFilterSupport([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdRoleFilter md)
        {
            var lAuth = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.FilterByApplicationIdListAsync(md, lAuth, true));
        }
    }
}