using HCore.Domain.Common;

namespace HCore.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : EntityBase<TKey>;
        Task<int> SaveChangesAsync();
    }

}
