using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessManagement;
using Necnat.Server.Bases;
using Necnat.Shared.Constants;
using Necnat.Shared.Exceptions;
using Necnat.Shared.Filters;
using Necnat.Shared.Models;
using Necnat.Shared.Resources;
using System.Collections.Generic;
using System.Linq;
using IAuthorizationService = Necnat.Server.Interfaces.IAuthorizationService;

namespace Necnat.AccessManagement.Server.v1.Modules.MdlAccessManagement
{
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : NnControllerBase
    {
        private readonly UserService _service;

        public UserController(
            IConfiguration configuration,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            IAuthorizationService authorizationService,
            UserService service) : base(configuration, authorizationService, commonApiLocalizer)
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
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/GetAll")]
        public IActionResult GetAll([FromQuery] string culture)
        {
            return StatusCode(200, _service.GetAll());
        }

        /// <summary>
        /// Obtains the record of the entity with that id, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/GetById")]
        public IActionResult GetById([FromQuery] string culture, [FromQuery] string id)
        {
            return StatusCode(200, _service.GetById(id));
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
        [Route("Api/v{version:apiVersion}/AccessManagement/[controller]/Filter")]
        public IActionResult Filter([FromQuery] string culture, [FromQuery] string moduleCodeName, [FromQuery] string featureCodeName, [FromBody] MdUserFilter md)
        {
            var lFeature = new List<MdAuthorizedModuleFeature>
            {
                //ModuleDevelopment
                new MdAuthorizedModuleFeature { ModuleCodeName = NamFeatureConstants.ModuleAccessManagement, FeatureCodeName = NamFeatureConstants.FeatureAccessManagementUserRead }
            };
            if (!lFeature.Any(x => x.ModuleCodeName == moduleCodeName && x.FeatureCodeName == featureCodeName))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED_FEATURE"].Value);

            return StatusCode(200, _service.Filter(md));
        }
    }
}
