using System;
namespace Test.Api.Infrastructure;

public interface IDataProvider<TEntity> where TEntity : IIdentity
{
    Task<TEntity[]> GetByFilterAsync(IFilterData filter);
    Task<TEntity>   GetDataAsync(Guid guid); 
}

