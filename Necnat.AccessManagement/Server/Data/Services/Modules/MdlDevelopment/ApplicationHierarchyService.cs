using Microsoft.EntityFrameworkCore;
using Necnat.AccessManagement.Server.Interfaces;
using Necnat.Server.Bases;
using Necnat.Server.DbContexts;
using Necnat.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlDevelopment
{
    public class ApplicationHierarchyService : NnServiceBase<NecnatAccessManagementDbContext, ApplicationHierarchy, int>, INecnatAccessManagementService
    {
        public ApplicationHierarchyService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<ApplicationHierarchy> GetByIdAsync(int id)
        {
            return await _context.ApplicationHierarchy
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ApplicationHierarchy>> SearchByApplicationIdIncludeHierarchy(int applicationId)
        {
            return await _context.ApplicationHierarchy
                .AsNoTracking()
                .Include(x => x.Hierarchy)
                .Where(x => x.ApplicationId == applicationId)
                .ToListAsync();
        }
    }
}