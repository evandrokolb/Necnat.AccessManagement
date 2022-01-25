using Microsoft.EntityFrameworkCore;
using Necnat.AccessManagement.Server.Interfaces;
using Necnat.Server.Bases;
using Necnat.Server.DbContexts;
using Necnat.Shared.Domains;
using Necnat.Shared.Entities;
using Necnat.Shared.Filters;
using Necnat.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlDevelopment
{
    public class ApiService : NnServiceBase<NecnatAccessManagementDbContext, Api, int>, INecnatAccessManagementService
    {
        public ApiService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<Api> GetByIdAsync(int id)
        {
            return await _context.Api
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Api> GetByIdIncludeControllerThenModuleAsync(int id)
        {
            return await _context.Api
                .AsNoTracking()
                .Include(x => x.Controller)
                .ThenInclude(x => x.Module)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Api>> SearchByControllerId(int controllerId)
        {
            return await _context.Api
                .AsNoTracking()
                .Where(x => x.ControllerId == controllerId)
                .ToListAsync();
        }

        public async Task<List<Api>> SearchByApplicationIdListAsync(ICollection<int> applicationIdList)
        {
            return await _context.Api
                .AsNoTracking()
                .Where(x => applicationIdList.Contains(x.Controller.Module.ApplicationId))
                .ToListAsync();
        }

        public async Task<MdFilterObject> FilterByApplicationIdListAsync(MdApiFilter filter, ICollection<int> applicationIdList, bool isSupport = false)
        {
            var q = _context.Api
                .AsNoTracking().Where(x =>
                    (applicationIdList.Contains(x.Controller.Module.ApplicationId))
                    && (filter.ApplicationIdFilter == null || x.Controller.Module.ApplicationId == filter.ApplicationIdFilter)
                    && (filter.ModuleIdFilter == null || x.Controller.ModuleId == filter.ModuleIdFilter)
                    && (filter.ControllerIdFilter == null || x.ControllerId == filter.ControllerIdFilter)
                    && (filter.HttpMethodIdFilter == null || x.HttpMethodId == filter.HttpMethodIdFilter)
                    && (filter.IsActiveFilter == null || x.IsActive == filter.IsActiveFilter)
                    && ((string.IsNullOrWhiteSpace(filter.NameFilter) && (filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.NameFilter) && x.Name == null)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Name == filter.NameFilter)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Name.Contains(filter.NameFilter)))
                    && ((string.IsNullOrWhiteSpace(filter.VersionFilter) && (filter.VersionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.VersionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.VersionFilter) && x.Version == null)
                        || ((filter.VersionFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.VersionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Version == filter.VersionFilter)
                        || ((filter.VersionFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.VersionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Version.Contains(filter.VersionFilter)))
                    && ((string.IsNullOrWhiteSpace(filter.DescriptionFilter) && (filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.DescriptionFilter) && x.Description == null)
                        || ((filter.DescriptionFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Description == filter.DescriptionFilter)
                        || ((filter.DescriptionFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Description.Contains(filter.DescriptionFilter)))

                    && (filter.WithoutIdList == null || filter.WithoutIdList.Count < 1 || !filter.WithoutIdList.Contains(x.Id))
                );

            if (isSupport)
                q = q.Select(x => new Api { Id = x.Id, ControllerId = x.ControllerId, Name = x.Name, IsActive = x.IsActive });

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