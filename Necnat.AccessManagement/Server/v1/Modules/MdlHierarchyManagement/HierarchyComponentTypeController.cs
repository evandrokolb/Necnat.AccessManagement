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
using Necnat.Shared.Exceptions;
using Necnat.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlHierarchyManagement
{
    [ApiController]
    [ApiVersion("1.0")]
    public class HierarchyComponentTypeController : NnControllerBase
    {
        HierarchyComponentTypeService _service;

        public HierarchyComponentTypeController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            HierarchyComponentTypeService service) : base(configuration, authorizationService, commonApiLocalizer)
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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/List")]
        public async Task<IActionResult> List([FromQuery] string culture)
        {
            await CheckAllowedApiAsync();

            return StatusCode(200, await _service.ListAsync());
        }

        /// <summary>
        /// Obtains the record of the entity with that id, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/GetById")]
        public async Task<IActionResult> GetById([FromQuery] string culture, [FromQuery] int id)
        {
            await CheckAllowedApiAsync();

            return StatusCode(200, await _service.GetByIdAsync(id));
        }

        /// <summary>
        /// Insert the entity, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entity.</param>
        /// <response code="200">A success message.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/Insert")]
        public async Task<IActionResult> Insert([FromQuery] string culture, [FromBody] HierarchyComponentType e)
        {
            var userId = await CheckAllowedApiAsync();

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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/Update")]
        public async Task<IActionResult> Update([FromQuery] string culture, [FromBody] HierarchyComponentType e)
        {
            var userId = await CheckAllowedApiAsync();

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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/Delete")]
        public async Task<IActionResult> Delete([FromQuery] string culture, [FromQuery] int id)
        {
            var userId = await CheckAllowedApiAsync();

            await _service.DeleteAsync(id, userId);

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
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/Filter")]
        public async Task<IActionResult> Filter([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdHierarchyComponentTypeFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleHierarchyManagement
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleHierarchyManagement, FeatureCodeName = NamFeatureConstants.FeatureHierarchyManagementHierarchyComponentTypeRead },
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleHierarchyManagement, FeatureCodeName = NamFeatureConstants.FeatureHierarchyManagementHierarchyHierarchyComponentTypeInsert }
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            return StatusCode(200, await _service.FilterListAsync(md));
        }
    }
}