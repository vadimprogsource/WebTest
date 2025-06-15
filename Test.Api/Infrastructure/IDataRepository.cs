using System;
using System.Linq.Expressions;

namespace Test.Api.Infrastructure
{
    public interface IDataRepository<TEntity> where TEntity : class , IIdentity
    {
        Task<TEntity> SelectAsync(Guid guid);
        Task<TEntity?> SelectAsync(Expression<Func<TEntity,bool>> condition);
        Task<TEntity[]> SelectAsync(Func<IQueryable<TEntity>,IQueryable<TEntity>> filter );
        Task<TEntity>   InsertAsync(TEntity entity);
        Task<TEntity>   UpdateAsync(TEntity entity);
        Task<bool>      DeleteAsync(Guid guid);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> condition);

    }
}

