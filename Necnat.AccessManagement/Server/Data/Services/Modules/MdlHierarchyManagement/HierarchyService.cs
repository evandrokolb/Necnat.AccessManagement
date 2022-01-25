using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlHierarchyManagement
{
    public class HierarchyService : NnServiceBase<NecnatAccessManagementDbContext, Hierarchy, int>, INecnatAccessManagementService
    {
        public HierarchyService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<Hierarchy> GetByIdAsync(int id)
        {
            return await _context.Hierarchy
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Hierarchy> GetByNameAsync(string name)
        {
            return await _context.Hierarchy
                .AsNoTracking()
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Hierarchy>> SearchByIdListAsync(ICollection<int> idList)
        {
            return await _context.Hierarchy
                .AsNoTracking()
                .Where(x => idList.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<int> InsertAndAddToHierarchyHierarchiesAsync(Hierarchy e, string actionUserId, IDbContextTransaction dbTransaction = null)
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

                var hierarchyId = (await GetByNameAsync(NamFeatureConstants.HierarchyHierarchiesName)).Id;

                var h = new HierarchicalStructure();
                h.HierarchyId = hierarchyId;
                h.ParentHierarchicalStructureId = _context.HierarchicalStructure.Where(x => x.HierarchyId == hierarchyId && x.ParentHierarchicalStructureId == null).First().Id;
                h.ComponentTypeId = (int)NamHierarchyComponentTypeConstants.Hierarchy;
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
                var e = await GetByIdAsync(id);

                var hierarchicalStructureService = new HierarchicalStructureService(_context);
                var lHierarchicalStructure = await hierarchicalStructureService.SearchByComponentAsync((int)NamHierarchyComponentTypeConstants.Hierarchy, e.Id);
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

        public async Task<MdFilterObject> FilterByIdListAsync(MdHierarchyFilter filter, ICollection<int> idList, bool isSupport = false)
        {
            var q = _context.Hierarchy
                .AsNoTracking().Where(x =>
                    (idList.Contains(x.Id))
                    && (filter.IsActiveFilter == null || x.IsActive == filter.IsActiveFilter)
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
                q = q.Select(x => new Hierarchy { Id = x.Id, Name = x.Name, IsActive = x.IsActive });

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

        public async Task<List<Hierarchy>> SearchHierarchyForTree(ICollection<int> l)
        {
            return await _context.Hierarchy
                .Include(x => x.HierarchyHierarchyComponentTypeList)
                .ThenInclude(x => x.HierarchyComponentType)
                .AsNoTracking().Where(x => l.Contains(x.Id))
                .Select(x => new Hierarchy { Id = x.Id, Name = x.Name, IsActive = x.IsActive, HierarchyHierarchyComponentTypeList = x.HierarchyHierarchyComponentTypeList }).ToListAsync();
        }
    }
}