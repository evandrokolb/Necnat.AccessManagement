using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessConfiguration;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessManagement;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlHierarchyManagement;
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

namespace Necnat.AccessManagement.Server.v1.Modules.MdlAccessManagement
{
    [ApiController]
    [ApiVersion("1.0")]
    public class SecurityController : NnControllerBase
    {
        SecurityService _service;
        RoleService _roleService;
        HierarchicalStructureService _hierarchicalStructureService;
        
        public SecurityController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            SecurityService service,
            RoleService roleService,
            HierarchicalStructureService hierarchicalStructureService) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _service = service;
            _roleService = roleService;
            _hierarchicalStructureService = hierarchicalStructureService;
        }

        /// <summary>
        /// Obtains all allowed records from that entity.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <response code="200">Collection of the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AList")]
        public async Task<IActionResult> AList([FromQuery] string culture)
        {
            var l = await SearchAllowedHierarchyComponentIdAsync((int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.SearchByApplicationIdListAsync(l));
        }

        /// <summary>
        /// Obtains the record of the entity with that id, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AGetById")]
        public async Task<IActionResult> AGetById([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdIncludeRoleAsync(id);
            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.Role.ApplicationId);

            e = EntityUtil.SetEntityNotSealedTypePropertiesToNull(e);
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
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AInsert")]
        public async Task<IActionResult> AInsert([FromQuery] string culture, [FromBody] Security e)
        {
            var aApp = await _roleService.GetByIdAsync(e.RoleId);
            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, aApp.ApplicationId);

            var aHie = await _hierarchicalStructureService.GetByIdAsync((int)e.HierarchicalStructureId);
            var userId = await CheckAllowedApiAsync(aHie.ComponentTypeId, aHie.ComponentId);

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
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AUpdate")]
        public async Task<IActionResult> AUpdate([FromQuery] string culture, [FromBody] Security e)
        {
            var a = await _roleService.GetByIdAsync(e.RoleId);
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, a.ApplicationId);

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
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/ADelete")]
        public async Task<IActionResult> ADelete([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdIncludeRoleAsync(id);

            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.Role.ApplicationId);

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
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/AFilter")]
        public async Task<IActionResult> AFilter([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdSecurityFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleDevelopment
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessManagement, FeatureCodeName = NamFeatureConstants.FeatureAccessManagementSecurityRead }
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            var l = await SearchAllowedHierarchyComponentIdAsync((int)NamHierarchyComponentTypeConstants.Application);

            return StatusCode(200, await _service.FilterAllowedAsync(md, l));
        }
    }
}
