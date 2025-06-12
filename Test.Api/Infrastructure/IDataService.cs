using System;
namespace Test.Api.Infrastructure
{
    public interface IDataService<TEntity> where TEntity : IIdentity
    {
        Task<TEntity> InserNewAsync     (TEntity entity);
        Task<TEntity> ApplyUpdateAsync  (TEntity entity);
        Task<bool>    ExecuteDeleteAsync(int Id);
    }
}

