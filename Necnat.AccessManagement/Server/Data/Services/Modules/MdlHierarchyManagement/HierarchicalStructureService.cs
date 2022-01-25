using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Necnat.AccessManagement.Server.Interfaces;
using Necnat.Server.Bases;
using Necnat.Server.DbContexts;
using Necnat.Shared.Constants;
using Necnat.Shared.Entities;
using Necnat.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlHierarchyManagement
{
    public class HierarchicalStructureService : NnServiceBase<NecnatAccessManagementDbContext, HierarchicalStructure, int>, INecnatAccessManagementService
    {
        public HierarchicalStructureService(NecnatAccessManagementDbContext context) : base(context) { }

        public async Task<HierarchicalStructure> GetByIdAsync(int id)
        {
            return await _context.HierarchicalStructure
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<HierarchicalStructure>> SearchByHierarchyIdAsync(int hierarchyId)
        {
            return await _context.HierarchicalStructure
                .AsNoTracking()
                .Where(x => x.HierarchyId == hierarchyId)
                .ToListAsync();
        }

        public async Task<List<HierarchicalStructure>> SearchByParentRecursiveIdAsync(int? parentHierarchicalStructureId)
        {
            return await _context.HierarchicalStructure
                .Where(x => x.ParentHierarchicalStructureId == parentHierarchicalStructureId)
                .ToListAsync();
        }

        public async Task<List<HierarchicalStructure>> SearchByComponentAsync(int componentTypeId, int componentId)
        {
            return await _context.HierarchicalStructure
                .Where(x => x.ComponentTypeId == componentTypeId && x.ComponentId == componentId)
                .ToListAsync();
        }

        public async Task RecursiveDelete(int id, string actionUserId,  IDbContextTransaction dbTransaction = null)
        {
            bool closeDbTransaction = false;
            if (dbTransaction == null)
            {
                dbTransaction = _context.Database.BeginTransaction();
                closeDbTransaction = true;
            }

            try
            {
                var lHierarchicalStructure = await SearchByParentRecursiveIdAsync(id);
                foreach (var iHierarchicalStructure in lHierarchicalStructure)
                    await RecursiveDelete(iHierarchicalStructure.Id, actionUserId, dbTransaction);

                var e = await GetByIdAsync(id);
                if (e.ParentHierarchicalStructureId != null)
                    await DeleteAsync(e.Id, actionUserId);

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

        public async Task<List<MdViewHierarchyComponent>> SearchHierarchyComponentByHierarchyId(int hierarchyId)
        {
            var l = new List<MdViewHierarchyComponent>();

            var type = await _context.Hierarchy
                .Include(x => x.HierarchyHierarchyComponentTypeList)
                .AsNoTracking()
                .Where(x => x.Id == hierarchyId)
                .FirstAsync();

            foreach (var iType in type.HierarchyHierarchyComponentTypeList)
                l.AddRange(await SearchHierarchyComponentByHierarchyComponentTypeId(iType.HierarchyComponentTypeId));

            return l;
        }

        public async Task<List<MdViewHierarchyComponent>> SearchHierarchyComponentByHierarchyComponentTypeId(int hierarchyComponentTypeId)
        {
            var lType = await _context.HierarchyComponentType.ToListAsync();
            var l = new List<MdViewHierarchyComponent>();

            if (hierarchyComponentTypeId == (int)NamHierarchyComponentTypeConstants.Hierarchy)
            {
                var lE = await GetAllHierarchy();

                foreach (var iE in lE)
                    l.Add(new MdViewHierarchyComponent
                    {
                        Id = iE.Id,
                        HierarchyComponentTypeId = (int)NamHierarchyComponentTypeConstants.Hierarchy,
                        HierarchyComponentType = lType.Where(x => x.Id == (int)NamHierarchyComponentTypeConstants.Hierarchy).FirstOrDefault(),
                        Name = iE.Name,
                        IsActive = iE.IsActive
                    });

                return l;
            }

            if (hierarchyComponentTypeId == (int)NamHierarchyComponentTypeConstants.Application)
            {
                var lE = await GetAllApplication();

                foreach (var iE in lE)
                    l.Add(new MdViewHierarchyComponent
                    {
                        Id = iE.Id,
                        HierarchyComponentTypeId = (int)NamHierarchyComponentTypeConstants.Application,
                        HierarchyComponentType = lType.Where(x => x.Id == (int)NamHierarchyComponentTypeConstants.Application).FirstOrDefault(),
                        Name = iE.Name,
                        IsActive = iE.IsActive
                    });

                return l;
            }

            return null;
        }

        public async Task<MdViewHierarchyComponent> GetHierarchyComponentByHierarchyComponentTypeIdAndHierarchyComponentIdAsync(int hierarchyComponentTypeId, int hierarchyComponentId)
        {
            var lType = await _context.HierarchyComponentType.ToListAsync();

            if (hierarchyComponentTypeId == (int)NamHierarchyComponentTypeConstants.Hierarchy)
            {
                var e = (await GetAllHierarchy()).Where(x => x.Id == hierarchyComponentId).First();
                var m = new MdViewHierarchyComponent
                {
                    Id = e.Id,
                    HierarchyComponentTypeId = (int)NamHierarchyComponentTypeConstants.Hierarchy,
                    HierarchyComponentType = lType.Where(x => x.Id == (int)NamHierarchyComponentTypeConstants.Hierarchy).FirstOrDefault(),
                    Name = e.Name,
                    IsActive = e.IsActive
                };

                return m;
            }

            if (hierarchyComponentTypeId == (int)NamHierarchyComponentTypeConstants.Application)
            {
                var e = (await GetAllApplication()).Where(x => x.Id == hierarchyComponentId).First();
                var m = new MdViewHierarchyComponent
                {
                    Id = e.Id,
                    HierarchyComponentTypeId = (int)NamHierarchyComponentTypeConstants.Application,
                    HierarchyComponentType = lType.Where(x => x.Id == (int)NamHierarchyComponentTypeConstants.Application).FirstOrDefault(),
                    Name = e.Name,
                    IsActive = e.IsActive
                };

                return m;
            }

            return null;
        }

        //HierarchyComponent
        private List<Hierarchy> _hierarchyCache = null;
        private List<Application> _applicationCache = null;

        private async Task<List<Hierarchy>> GetAllHierarchy()
        {
            if (_hierarchyCache != null)
                return _hierarchyCache;

            _hierarchyCache = await _context.Hierarchy.ToListAsync();
            return _hierarchyCache;
        }

        private async Task<List<Application>> GetAllApplication()
        {
            if (_applicationCache != null)
                return _applicationCache;

            _applicationCache = await _context.Application.ToListAsync();
            return _applicationCache;
        }
    }
}