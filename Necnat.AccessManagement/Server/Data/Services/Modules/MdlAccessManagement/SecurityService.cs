using Microsoft.EntityFrameworkCore;
using Necnat.Server.Bases;
using Necnat.Server.DbContexts;
using Necnat.Shared.Domains;
using Necnat.Shared.Entities;
using Necnat.Shared.Filters;
using Necnat.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Necnat.AccessManagement.Server.Interfaces;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlHierarchyManagement;

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessManagement
{
    public class SecurityService : NnServiceBase<NecnatAccessManagementDbContext, Security, int>, INecnatAccessManagementService
    {
        public SecurityService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<Security> GetByIdIncludeRoleAsync(int id)
        {
            return await _context.Security
                .Include(x => x.Role)
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstAsync();
        }

        public async Task<List<Security>> SearchByApplicationIdListAsync(ICollection<int> applicationIdList)
        {
            return await _context.Security
                .AsNoTracking()
                .Where(x => applicationIdList.Contains(x.Role.ApplicationId))
                .ToListAsync();
        }

        public async Task<MdFilterObject> FilterAllowedAsync(MdSecurityFilter filter, ICollection<int> l, bool isSupport = false)
        {
            var q = _context.Security
                .Include(x => x.HierarchicalStructure)
                .ThenInclude(x => x.Hierarchy)
                .AsNoTracking().Where(x =>
                    (l.Contains(x.Role.ApplicationId))
                    && (filter.RoleIdFilter == null || x.RoleId == filter.RoleIdFilter)
                    && (filter.IsActiveFilter == null || x.IsActive == filter.IsActiveFilter)
                    && ((filter.HierarchicalStructureIdFilter == null && filter.HierarchicalStructureIdFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) || x.HierarchicalStructureId == filter.HierarchicalStructureIdFilter)
                    && ((string.IsNullOrWhiteSpace(filter.UserIdFilter) && (filter.UserIdFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.UserIdFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.UserIdFilter) && x.UserId == null)
                        || ((filter.UserIdFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.UserIdFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.UserId == filter.UserIdFilter)
                        || ((filter.UserIdFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.UserIdFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.UserId.Contains(filter.UserIdFilter)))
                    && (filter.WithoutIdList == null || filter.WithoutIdList.Count < 1 || !filter.WithoutIdList.Contains(x.Id))
                );

            if (isSupport)
                q = q.Select(x => new Security { Id = x.Id, UserId = x.UserId, RoleId = x.RoleId, HierarchicalStructureId = x.HierarchicalStructureId, IsActive = x.IsActive });

            if ((!string.IsNullOrWhiteSpace(filter.OrderBy)) && filter.OrderBy.EndsWith("asc"))
                q = q.OrderBy(filter.OrderBy.Split(' ')[0]);
            else if ((!string.IsNullOrWhiteSpace(filter.OrderBy)) && filter.OrderBy.EndsWith("desc"))
                q = q.OrderByDescending(filter.OrderBy.Split(' ')[0]);
            else
                q = q.OrderBy(x => x.Id);

            var op = new MdFilterObject();

            if (filter.IsPaging)
            {
                op.Total = await q.CountAsync();
                q = q.Skip(filter.Skip).Take(filter.Take);
            }

            var lSecurity = await q.ToListAsync();

            var userService = new UserService();
            var hierarchicalStructureService = new HierarchicalStructureService(_context);

            var lData = new List<MdViewSecurity>();
            foreach (var iSecurity in lSecurity)
            {
                var e = new MdViewSecurity();
                e.Security = iSecurity;
                e.UserName = userService.GetById(iSecurity.UserId).Name;
                e.HierarchyName = iSecurity.HierarchicalStructure.Hierarchy.Name;
                var hc = await hierarchicalStructureService.GetHierarchyComponentByHierarchyComponentTypeIdAndHierarchyComponentIdAsync(iSecurity.HierarchicalStructure.ComponentTypeId, iSecurity.HierarchicalStructure.ComponentId);
                e.HierarchyComponentTypeName = hc.HierarchyComponentType.Name;
                e.HierarchyComponentName = hc.Name;

                if (isSupport)
                    e.Security = new Security { Id = iSecurity.Id, UserId = iSecurity.UserId, RoleId = iSecurity.RoleId, HierarchicalStructureId = iSecurity.HierarchicalStructureId, IsActive = iSecurity.IsActive };
                else
                {
                    iSecurity.HierarchicalStructure.Hierarchy = null;
                    e.Security = iSecurity;
                }

                lData.Add(e);
            }

            if (!filter.IsPaging)
                op.Total = lData.Count;

            op.Data = lData;
            return op;
        }
    }
}
