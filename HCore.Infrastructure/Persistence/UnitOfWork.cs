using HCore.Domain.Common;
using HCore.Domain.Repositories;
using HCore.Infrastructure.Persistence.Repositories;

namespace HCore.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private bool _disposed = false;
        private readonly Dictionary<string, object> _repositories = new Dictionary<string, object>();

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : EntityBase<TKey>
        {
            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<TEntity, TKey>)_repositories[type];
            }

            var repositoryInstance = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories.Add(type, repositoryInstance);
            return repositoryInstance;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
