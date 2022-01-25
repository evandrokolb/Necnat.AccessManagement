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
    public class FeatureApiService : NnServiceBase<NecnatAccessManagementDbContext, FeatureApi, int>, INecnatAccessManagementService
    {
        public FeatureApiService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<FeatureApi> GetByIdAsync(int id)
        {
            return await _context.FeatureApi
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<FeatureApi>> SearchByFeatureIdIncludeApi(int featureId)
        {
            return await _context.FeatureApi
                .AsNoTracking()
                .Include(x => x.Api)
                .Where(x => x.FeatureId == featureId)
                .ToListAsync();
        }        
    }
}