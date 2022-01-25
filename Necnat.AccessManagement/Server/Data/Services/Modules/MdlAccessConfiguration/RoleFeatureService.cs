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
    public class RoleFeatureService : NnServiceBase<NecnatAccessManagementDbContext, RoleFeature, int>, INecnatAccessManagementService
    {
        public RoleFeatureService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<RoleFeature> GetByIdAsync(int id)
        {
            return await _context.RoleFeature
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RoleFeature>> SearchByRoleIdIncludeFeature(int roleId)
        {
            return await _context.RoleFeature
                .AsNoTracking()
                .Include(x => x.Feature)
                .Where(x => x.RoleId == roleId)
                .ToListAsync();
        }
    }
}