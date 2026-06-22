using System;
using System.Collections.Generic;
using System.Text;
using Test.Api.Infrastructure.Query;

namespace Test.Api.Infrastructure.Providers.Cache;

public interface IDataCache<TEntity>
{
    TimeSpan Expiration { get; set; }
    Task<bool> TryGetDataAsync(Guid guid, out TEntity obj);
    Task<bool> TryGetPageAsync(IFilterData filter,out IDataPage<TEntity> page);

    Task<TEntity> PutDataAsync(TEntity obj);
    Task<IDataPage<TEntity>> PutPageAsync(IDataPage<TEntity> page);
    Task DeleteAsync(Guid guid);
    
}
