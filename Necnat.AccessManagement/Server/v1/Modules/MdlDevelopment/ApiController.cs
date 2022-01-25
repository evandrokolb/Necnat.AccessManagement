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
using Necnat.Shared.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlDevelopment
{
    [ApiController]
    [ApiVersion("1.0")]
    public class ApiController : NnControllerBase
    {
        ApiService _service;
        ControllerService _controllerService;

        public ApiController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            ApiService service,
            ControllerService controllerService) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _service = service;
            _controllerService = controllerService;
        }

        /// <summary>
        /// Obtains all allowed records from that entity.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <response code="200">Collection of the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/AList")]
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
        [Route("Api/v{version:apiVersion}/Development/[controller]/AGetById")]
        public async Task<IActionResult> AGetById([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdIncludeControllerThenModuleAsync(id);

            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.Controller.Module.ApplicationId);

            e = EntityUtil.SetEntityNotSealedTypePropertiesToNull(e);
            return StatusCode(200, e);
        }

        /// <summary>
        /// Obtains the record of the entity with that ControllerId, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="controllerId">Controller identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/ASearchByControllerId")]
        public async Task<IActionResult> ASearchByControllerId([FromQuery] string culture, [FromQuery] int controllerId)
        {
            var eAuth = await _controllerService.GetByIdIncludeModuleAsync(controllerId);
            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, eAuth.Module.ApplicationId);

            return StatusCode(200, await _service.SearchByControllerId(controllerId));
        }

        /// <summary>
        /// Insert the entity, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/AInsert")]
        public async Task<IActionResult> AInsert([FromQuery] string culture, [FromBody] Api e)
        {
            var eAuth = await _controllerService.GetByIdIncludeModuleAsync(e.ControllerId);
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, eAuth.Module.ApplicationId);

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
        [Route("Api/v{version:apiVersion}/Development/[controller]/AUpdate")]
        public async Task<IActionResult> AUpdate([FromQuery] string culture, [FromBody] Api e)
        {
            var eAuth = await _controllerService.GetByIdIncludeModuleAsync(e.ControllerId);
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, eAuth.Module.ApplicationId);

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
        [Route("Api/v{version:apiVersion}/Development/[controller]/ADelete")]
        public async Task<IActionResult> ADelete([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdIncludeControllerThenModuleAsync(id);

            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.Controller.Module.ApplicationId);

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
        [Route("Api/v{version:apiVersion}/Development/[controller]/AFilter")]
        public async Task<IActionResult> AFilter([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdApiFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleDevelopment
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentApiRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleDevelopment, FeatureCodeName = NamFeatureConstants.FeatureDevelopmentFeatureApiInsert },
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            var lAuth = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.FilterByApplicationIdListAsync(md, lAuth));
        }
    }
}