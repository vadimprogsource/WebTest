using System.Linq.Expressions;

namespace Test.Api.Infrastructure
{
    public interface IDataRepository<TEntity> where TEntity : class, IIdentity
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> condition);
        IQueryContext<TEntity> Context { get; }

    }
}

