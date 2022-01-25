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
    public class HierarchyHierarchyComponentTypeService : NnServiceBase<NecnatAccessManagementDbContext, HierarchyHierarchyComponentType, int>, INecnatAccessManagementService
    {
        public HierarchyHierarchyComponentTypeService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<HierarchyHierarchyComponentType> GetByIdAsync(int id)
        {
            return await _context.HierarchyHierarchyComponentType
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<HierarchyHierarchyComponentType>> SearchByHierarchyIdIncludeHierarchyComponentType(int hierarchyId)
        {
            return await _context.HierarchyHierarchyComponentType
                .AsNoTracking()
                .Include(x => x.HierarchyComponentType)
                .Where(x => x.HierarchyId == hierarchyId)
                .ToListAsync();
        }
    }
}