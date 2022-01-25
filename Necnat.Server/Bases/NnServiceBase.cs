using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Necnat.Shared.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Necnat.Server.Bases
{
    public abstract class NnServiceBase<TDbContext, TEntity, TKey> where TDbContext : DbContext where TEntity : class
    {
        protected readonly TDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public NnServiceBase(TDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<TKey> InsertAsync(TEntity e, string actionUserId)
        {
            e = EntityUtil.SetEntityNotSealedTypePropertiesToNull(e);

            await _context.AddAsync(e);
            await _context.SaveChangesAsync();

            return EntityUtil.GetEntityKeyValue<TEntity, TKey>(e);
        }

        public virtual async Task InsertRangeAsync(List<TEntity> lE, string actionUserId)
        {
            for (var i = 0; i < lE.Count; i++)
                lE[i] = EntityUtil.SetEntityNotSealedTypePropertiesToNull(lE[i]);

            await _dbSet.AddRangeAsync(lE);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity e, TKey key, string actionUserId)
        {
            e = EntityUtil.SetEntityNotSealedTypePropertiesToNull(e);

            var eDb = await _dbSet.FindAsync(key);
            _context.Entry(eDb).CurrentValues.SetValues(e);
            _dbSet.Update(eDb);

            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TKey key, string actionUserId)
        {
            var e = await _dbSet.FindAsync(key);
            _dbSet.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}