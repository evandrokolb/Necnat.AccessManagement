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
    public class HierarchicalStructureController : NnControllerBase
    {
        HierarchicalStructureService _service;
        HierarchyService _hierarchyService;

        public HierarchicalStructureController(
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            IStringLocalizer<CommonApiLocalizer> commonApiLocalizer,
            HierarchicalStructureService service,
            HierarchyService hierarchyService) : base(configuration, authorizationService, commonApiLocalizer)
        {
            _service = service;
            _hierarchyService = hierarchyService;
        }

        /// <summary>
        /// Obtains all minimal information of the allowed records from hierarchies of hierarchical structure, corresponding to the filter.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <response code="200">A list of records from that entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/ASearchHierarchyForTree")]
        public async Task<IActionResult> SearchHierarchyForTree([FromQuery] string culture)
        {
            var lSecurityHierarchy = await SearchAllowedHierarchyComponentAsync();

            var l = new List<int>();
            l.AddRange(lSecurityHierarchy.Where(x => x.ComponentTypeId == (int)NamHierarchyComponentTypeConstants.Hierarchy).Select(x => x.ComponentId).Distinct());
            l.AddRange(lSecurityHierarchy.Select(x => x.HierarchyId).Distinct());

            return StatusCode(200, await _hierarchyService.SearchHierarchyForTree(l.Distinct().ToList()));
        }

        /// <summary>
        /// Obtains all records of the entity with that hierarchyId in a custom model, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="e">Entry Model.</param>
        /// <response code="200">A list of records from that entity.</response>
        [HttpPost]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/ASearchTree")]
        public async Task<IActionResult> SearchTree([FromQuery] string culture, [FromBody] MdSearchTree e)
        {
            var lAuth = await SearchAllowedHierarchyComponentAsync();
            if (!(lAuth.Where(x => x.ComponentTypeId == (int)NamHierarchyComponentTypeConstants.Hierarchy).Select(x => x.ComponentId).Contains(e.HierarchyId) || lAuth.Select(x => x.HierarchyId).Contains(e.HierarchyId)))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            var allowedDict = new Dictionary<int, List<int>>();
            foreach (var iSearchTreeAllowed in e.SearchTreeAllowedList)
                allowedDict.Add(iSearchTreeAllowed.Id, await SearchHierarchicalStructureIdWithPermissionList(iSearchTreeAllowed.ModuleCodeName, iSearchTreeAllowed.FeatureCodeName));

            var lTree = new List<MdHierarchicalStructureTree>();
            var l = await _service.SearchByHierarchyIdAsync(e.HierarchyId);
            foreach (var iE in l)
            {
                var tree = new MdHierarchicalStructureTree();
                tree.HierarchicalStructure = iE;
                tree.HierarchyComponent = await _service.GetHierarchyComponentByHierarchyComponentTypeIdAndHierarchyComponentIdAsync(iE.ComponentTypeId, iE.ComponentId);

                foreach (var iSearchTreeAllowed in e.SearchTreeAllowedList)
                {
                    var m = new MdHierarchicalStructureTreeAllowed();
                    m.Id = iSearchTreeAllowed.Id;
                    m.IsAllowed = allowedDict[iSearchTreeAllowed.Id].Any(x => x == iE.Id);

                    tree.HierarchicalStructureTreeAllowedList.Add(m);
                }

                lTree.Add(tree);
            }

            return StatusCode(200, lTree);
        }

        /// <summary>
        /// Obtains the record of the entity with that id, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="id">Entity identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/ASearchTreeById")]
        public async Task<IActionResult> SearchTreeById([FromQuery] string culture, [FromQuery] int id)
        {
            var e = await _service.GetByIdAsync(id);

            var lAuth = await SearchAllowedHierarchyComponentAsync();
            if (!(lAuth.Where(x => x.ComponentTypeId == (int)NamHierarchyComponentTypeConstants.Hierarchy).Select(x => x.ComponentId).Contains(e.HierarchyId) || lAuth.Select(x => x.HierarchyId).Contains(e.HierarchyId)))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            var tree = new MdHierarchicalStructureTree();
            tree.HierarchicalStructure = e;
            tree.HierarchyComponent = await _service.GetHierarchyComponentByHierarchyComponentTypeIdAndHierarchyComponentIdAsync(e.ComponentTypeId, e.ComponentId);

            return StatusCode(200, tree);
        }

        /// <summary>
        /// Obtains all allowed Hierarchy Components of the entity with that hierarchyId, if allowed.
        /// </summary>
        /// <param name="culture">Response language.</param>
        /// <param name="hierarchyId">Hierarchy identifier.</param>
        /// <response code="200">One record from the entity.</response>
        [HttpGet]
        [Authorize]
        [Route("Api/v{version:apiVersion}/HierarchyManagement/[controller]/ASearchHierarchyComponentByHierarchyId")]
        public async Task<IActionResult> ASearchHierarchyComponentByHierarchyId([FromQuery] string culture, [FromQuery] int hierarchyId)
        {
            var lAuth = await SearchAllowedHierarchyComponentAsync();
            if (!(lAuth.Where(x => x.ComponentTypeId == (int)NamHierarchyComponentTypeConstants.Hierarchy).Select(x => x.ComponentId).Contains(hierarchyId) || lAuth.Select(x => x.HierarchyId).Contains(hierarchyId)))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            return StatusCode(200, await _service.SearchHierarchyComponentByHierarchyId(hierarchyId));
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
        public async Task<IActionResult> AInsert([FromQuery] string culture, [FromBody] HierarchicalStructure e)
        {
            List<int> lInsert = await SearchHierarchicalStructureIdWithPermissionList(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureInsert);
            if (!lInsert.Contains((int)e.ParentHierarchicalStructureId))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            return StatusCode(200, new { Key = await _service.InsertAsync(e, GetUserId()), Message = _commonApiLocalizer["ENTITY_SUCCESSFULLY_INCLUDED"].Value });
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
            List<int> lDelete = await SearchHierarchicalStructureIdWithPermissionList(NamFeatureConstants.ModuleHierarchyManagement, NamFeatureConstants.FeatureHierarchyManagementHierarchicalStructureDelete);
            if (!lDelete.Contains(e.Id))
                throw new BusinessException(_commonApiLocalizer["UNAUTHORIZED"].Value);

            await _service.RecursiveDelete(id, GetUserId());

            return StatusCode(200, _commonApiLocalizer["ENTITY_SUCCESSFULLY_DELETED"].Value);
        }

        private async Task<List<int>> SearchHierarchicalStructureIdWithPermissionList(string moduleCodeName, string featureCodeName)
        {
            List<int> l = new List<int>();

            //From Hierarchy
            var lFromHierarchy = await SearchAllowedHierarchyComponentIdAsync(moduleCodeName, featureCodeName, (int)NamHierarchyComponentTypeConstants.Hierarchy);
            foreach (var iFromHierarchy in lFromHierarchy)
            {
                var lHierarchicalStructure = await _service.SearchByHierarchyIdAsync(iFromHierarchy);
                foreach (var iE in lHierarchicalStructure)
                    if (!l.Contains(iE.Id))
                        l.Add(iE.Id);
            }

            //From Hierarchy Part
            var lFromHierarchyPart = await SearchAllowedHierarchicalStructureAsync(moduleCodeName, featureCodeName);
            foreach (var iFromHierarchyPart in lFromHierarchyPart)
                if (!l.Contains(iFromHierarchyPart.Id))
                    l.Add(iFromHierarchyPart.Id);

            return l;
        }

    }
}
