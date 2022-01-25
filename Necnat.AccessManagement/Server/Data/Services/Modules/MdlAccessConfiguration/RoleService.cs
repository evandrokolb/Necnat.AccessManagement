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

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessConfiguration
{
    public class RoleService : NnServiceBase<NecnatAccessManagementDbContext, Role, int>, INecnatAccessManagementService
    {
        public RoleService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Role
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Role>> SearchByApplicationIdListAsync(ICollection<int> xyzIdList)
        {
            return await _context.Role
                .AsNoTracking()
                .Where(x => xyzIdList.Contains(x.ApplicationId))
                .ToListAsync();
        }

        public async Task<MdFilterObject> FilterByApplicationIdListAsync(MdRoleFilter filter, ICollection<int> xyzIdList, bool isSupport = false)
        {
            var q = _context.Role
                .AsNoTracking().Where(x =>
                    (xyzIdList.Contains(x.ApplicationId))
                    && (filter.ApplicationIdFilter == null || x.ApplicationId == filter.ApplicationIdFilter)
                    && (filter.IsActiveFilter == null || x.IsActive == filter.IsActiveFilter)
                    && ((string.IsNullOrWhiteSpace(filter.NameFilter) && (filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.NameFilter) && x.Name == null)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Name == filter.NameFilter)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Name.Contains(filter.NameFilter)))
                    && ((string.IsNullOrWhiteSpace(filter.CodeNameFilter) && (filter.CodeNameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.CodeNameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.CodeNameFilter) && x.CodeName == null)
                        || ((filter.CodeNameFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.CodeNameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.CodeName == filter.CodeNameFilter)
                        || ((filter.CodeNameFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.CodeNameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.CodeName.Contains(filter.CodeNameFilter)))
                    && ((string.IsNullOrWhiteSpace(filter.DescriptionFilter) && (filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.DescriptionFilter) && x.Description == null)
                        || ((filter.DescriptionFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Description == filter.DescriptionFilter)
                        || ((filter.DescriptionFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Description.Contains(filter.DescriptionFilter)))

                    && (filter.WithoutIdList == null || filter.WithoutIdList.Count < 1 || !filter.WithoutIdList.Contains(x.Id))
                );

            if (isSupport)
                q = q.Select(x => new Role { Id = x.Id, ApplicationId = x.ApplicationId, Name = x.Name, IsActive = x.IsActive });

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

            var lData = await q.ToListAsync();

            if (!filter.IsPaging)
                op.Total = lData.Count;

            op.Data = lData;
            return op;
        }
    }
}