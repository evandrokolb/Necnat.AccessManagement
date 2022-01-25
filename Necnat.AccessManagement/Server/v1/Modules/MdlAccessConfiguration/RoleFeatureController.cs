using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessConfiguration;
using Necnat.Server.Bases;
using Necnat.Shared.Constants;
using Necnat.Shared.Entities;
using Necnat.Shared.Resources;
using System.Threading.Tasks;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlAccessConfiguration
{
    [ApiController]
    [ApiVersion("1.0")]
    public class RoleFeatureController : NnControllerBase
    {
        RoleFeatureService _service;
        RoleService _roleService;

        public RoleFeatureController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            RoleFeatureService service,
            RoleService roleService) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _service = service;
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessConfiguration/[controller]/ASearchByRoleIdIncludeFeature")]
        public async Task<IActionResult> ASearchByRoleIdIncludeFeature([FromQuery] string culture, [FromQuery] int roleId)
        {
            var eAuth = await _roleService.GetByIdAsync(roleId);
            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, eAuth.ApplicationId);

            return StatusCode(200, await _service.SearchByRoleIdIncludeFeature(roleId));
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
        public async Task<IActionResult> AInsert([FromQuery] string culture, [FromBody] RoleFeature e)
        {
            var eAuth = await _roleService.GetByIdAsync(e.RoleId);
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, eAuth.ApplicationId);

            return StatusCode(200, new { Key = await _service.InsertAsync(e, userId), Message = _commonApiLocalizer["ENTITY_SUCCESSFULLY_INCLUDED"].Value });
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

            var eAuth = await _roleService.GetByIdAsync(e.RoleId);
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, eAuth.ApplicationId);

            await _service.DeleteAsync(e.Id, userId);

            return StatusCode(200, _commonApiLocalizer["ENTITY_SUCCESSFULLY_DELETED"].Value);
        }
    }
}