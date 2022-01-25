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

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlHierarchyManagement
{
    public class HierarchyComponentTypeService : NnServiceBase<NecnatAccessManagementDbContext, HierarchyComponentType, int>, INecnatAccessManagementService
    {
        public HierarchyComponentTypeService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<List<HierarchyComponentType>> ListAsync()
        {
            return await _context.HierarchyComponentType
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<HierarchyComponentType> GetByIdAsync(int id)
        {
            return await _context.HierarchyComponentType
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<MdFilterObject> FilterListAsync(MdHierarchyComponentTypeFilter filter, bool isSupport = false)
        {
            var q = _context.HierarchyComponentType
                .AsNoTracking().Where(x =>
                    (filter.IsActiveFilter == null || x.IsActive == filter.IsActiveFilter)
                    && ((string.IsNullOrWhiteSpace(filter.NameFilter) && (filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.NameFilter) && x.Name == null)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Name == filter.NameFilter)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Name.Contains(filter.NameFilter)))

                    && (filter.WithoutIdList == null || filter.WithoutIdList.Count < 1 || !filter.WithoutIdList.Contains(x.Id))
                );

            if (isSupport)
                q = q.Select(x => new HierarchyComponentType { Id = x.Id, Name = x.Name, IsActive = x.IsActive });

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