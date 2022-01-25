using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Necnat.AccessManagement.Server.Data.Services.Modules.MdlHierarchyManagement;
using Necnat.AccessManagement.Server.Interfaces;
using Necnat.Server.Bases;
using Necnat.Server.DbContexts;
using Necnat.Shared.Constants;
using Necnat.Shared.Domains;
using Necnat.Shared.Entities;
using Necnat.Shared.Filters;
using Necnat.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlDevelopment
{
    public class ApplicationService : NnServiceBase<NecnatAccessManagementDbContext, Application, int>, INecnatAccessManagementService
    {
        public ApplicationService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<Application> GetByIdAsync(int id)
        {
            return await _context.Application
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Application>> SearchByIdListAsync(ICollection<int> idList)
        {
            return await _context.Application
                .AsNoTracking()
                .Where(x => idList.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<int> InsertAndAddToHierarchyApplicationsAsync(Application e, string actionUserId, IDbContextTransaction dbTransaction = null)
        {
            bool closeDbTransaction = false;
            if (dbTransaction == null)
            {
                dbTransaction = _context.Database.BeginTransaction();
                closeDbTransaction = true;
            }

            try
            {
                var key = await InsertAsync(e, actionUserId);

                var hierarchyService = new HierarchyService(_context);
                var hierarchyId = (await hierarchyService.GetByNameAsync(NamFeatureConstants.HierarchyApplicationsName)).Id;

                var h = new HierarchicalStructure();
                h.HierarchyId = hierarchyId;
                h.ParentHierarchicalStructureId = _context.HierarchicalStructure.Where(x => x.HierarchyId == hierarchyId && x.ParentHierarchicalStructureId == null).First().Id;
                h.ComponentTypeId = (int)NamHierarchyComponentTypeConstants.Application;
                h.ComponentId = e.Id;

                var hierarchicalStructureService = new HierarchicalStructureService(_context);
                await hierarchicalStructureService.InsertAsync(h, actionUserId);

                if (closeDbTransaction)
                    await dbTransaction.CommitAsync();

                return key;
            }
            catch
            {
                if (closeDbTransaction)
                    await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAndRemoveFromHierarchicalStructureAsync(int id, string actionUserId, IDbContextTransaction dbTransaction = null)
        {
            bool closeDbTransaction = false;
            if (dbTransaction == null)
            {
                dbTransaction = _context.Database.BeginTransaction();
                closeDbTransaction = true;
            }

            try
            {
                var e = await _context.Application
                .Where(x => x.Id == id)
                .FirstAsync();

                var hierarchicalStructureService = new HierarchicalStructureService(_context);
                var lHierarchicalStructure = await hierarchicalStructureService.SearchByComponentAsync((int)NamHierarchyComponentTypeConstants.Application, e.Id);
                foreach (var iHierarchicalStructure in lHierarchicalStructure)
                    await hierarchicalStructureService.RecursiveDelete(iHierarchicalStructure.Id, actionUserId, dbTransaction);

                if (closeDbTransaction)
                    await dbTransaction.CommitAsync();
            }
            catch
            {
                if (closeDbTransaction)
                    await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<MdFilterObject> FilterByIdListAsync(MdApplicationFilter filter, ICollection<int> idList, bool isSupport = false)
        {
            var q = _context.Application
                .AsNoTracking().Where(x =>
                    (idList.Contains(x.Id))
                    && (filter.IsActiveFilter == null || x.IsActive == filter.IsActiveFilter)
                    && ((string.IsNullOrWhiteSpace(filter.AcronymFilter) && (filter.AcronymFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.AcronymFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.AcronymFilter) && x.Acronym == null)
                        || ((filter.AcronymFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.AcronymFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Acronym == filter.AcronymFilter)
                        || ((filter.AcronymFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.AcronymFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Acronym.Contains(filter.AcronymFilter)))
                    && ((string.IsNullOrWhiteSpace(filter.NameFilter) && (filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.NameFilter) && x.Name == null)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Name == filter.NameFilter)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Name.Contains(filter.NameFilter)))
                    && ((string.IsNullOrWhiteSpace(filter.DescriptionFilter) && (filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.DescriptionFilter) && x.Description == null)
                        || ((filter.DescriptionFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Description == filter.DescriptionFilter)
                        || ((filter.DescriptionFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.DescriptionFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Description.Contains(filter.DescriptionFilter)))
                    && (filter.WithoutIdList == null || filter.WithoutIdList.Count < 1 || !filter.WithoutIdList.Contains(x.Id))
                );

            if (isSupport)
                q = q.Select(x => new Application { Id = x.Id, Acronym = x.Acronym, Name = x.Name, IsActive = x.IsActive });

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
