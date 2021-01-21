using OnlineStore.Data.Contracts;
using OnlineStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>, IDisposable
        where TEntity : EntityBase<TKey>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private bool _disposed;


        protected RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        #region IRepository members
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task Delete(TKey id)
        {
            var entity = await GetAsync(id);

            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }
        public IQueryable<TEntity> GetAll(int skip, int take)
        {
            return _dbSet.OrderBy(q => q.Id).Skip(skip).Take(take);
        }

        public IQueryable<TEntity> GetAll(int skip, int take, Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll(skip, take).Where(predicate);
        }

        // async ve await kullanmadık çünkü bir referans gibi davrandırdık
        //nerde await kullanılırsa orda execute islemi yapılsın diye burda yazmadık
        public Task<TEntity> GetAsync(TKey id)
        {
            return _dbSet.FindAsync(id);
        }

        public Task SaveChangeAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        #endregion

        #region IDisposable members
        ~RepositoryBase()
        {
            //de constructor method garbage collaction tarafından dispose edilmiş sınıfın sonraki işlemlerinde çagrılan bir method
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
