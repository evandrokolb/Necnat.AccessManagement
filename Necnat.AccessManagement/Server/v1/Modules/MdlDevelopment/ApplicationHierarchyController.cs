using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlDevelopment;
using Necnat.Server.Bases;
using Necnat.Shared.Constants;
using Necnat.Shared.Entities;
using Necnat.Shared.Resources;
using System.Threading.Tasks;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlDevelopment
{
    [ApiController]
    [ApiVersion("1.0")]
    public class ApplicationHierarchyController : NnControllerBase
    {
        ApplicationHierarchyService _service;

        public ApplicationHierarchyController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            ApplicationHierarchyService service) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/Development/[controller]/ASearchByApplicationIdIncludeHierarchy")]
        public async Task<IActionResult> ASearchByApplicationIdIncludeHierarchy([FromQuery] string culture, [FromQuery] int applicationId)
        {
            await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, applicationId);

            return StatusCode(200, await _service.SearchByApplicationIdIncludeHierarchy(applicationId));
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
        public async Task<IActionResult> AInsert([FromQuery] string culture, [FromBody] ApplicationHierarchy e)
        {
            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.ApplicationId);

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
        [Route("Api/v{version:apiVersion}/Development/[controller]/ADelete")]
        public async Task<IActionResult> ADelete([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdAsync(id);

            var userId = await CheckAllowedApiAsync((int)NamHierarchyComponentTypeConstants.Application, e.ApplicationId);

            await _service.DeleteAsync(e.Id, userId);

            return StatusCode(200, _commonApiLocalizer["ENTITY_SUCCESSFULLY_DELETED"].Value);
        }
    }
}