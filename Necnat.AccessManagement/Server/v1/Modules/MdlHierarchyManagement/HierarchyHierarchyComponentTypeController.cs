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

namespace Necnat.AccessManagement.Server.v1.Modules.MdlHierarchyManagement
{
    [ApiController]
    [ApiVersion("1.0")]
    public class HierarchyHierarchyComponentTypeController : NnControllerBase
    {
        HierarchyHierarchyComponentTypeService _service;

        public HierarchyHierarchyComponentTypeController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            HierarchyHierarchyComponentTypeService service) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/ASearchByHierarchyIdIncludeHierarchyComponentType")]
        public async Task<IActionResult> ASearchByHierarchyIdIncludeHierarchyComponentType([FromQuery] string culture, [FromQuery] int hierarchyId)
        {
            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Hierarchy, hierarchyId);

            return StatusCode(200, await _service.SearchByHierarchyIdIncludeHierarchyComponentType(hierarchyId));
        }        

        /// <summary>
        /// Insert the entity, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/AInsert")]
        public async Task<IActionResult> AInsert([FromQuery] string culture, [FromBody] HierarchyHierarchyComponentType e)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Hierarchy, e.HierarchyId);

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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/ADelete")]
        public async Task<IActionResult> ADelete([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdAsync(id);

            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Hierarchy, e.HierarchyId);

            await _service.DeleteAsync(e.Id, userId);

            return StatusCode(200, _commonApiLocalizer["ENTITY_SUCCESSFULLY_DELETED"].Value);
        }
    }
}