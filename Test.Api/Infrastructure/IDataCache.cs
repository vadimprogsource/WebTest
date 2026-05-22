using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure;

public interface IDataCache<TEntity>
{
    TimeSpan Expiration { get; set; }
    Task<bool> TryGetDataAsync(Guid guid, out TEntity obj);
    Task<bool> TryGetPageAsync(IFilterData filter,out IDataPage<TEntity> page);

    Task<TEntity> AddAsync(TEntity obj);
    Task<IDataPage<TEntity>> AddPageAsync(IDataPage<TEntity> page);
    Task RemoveAsync(Guid guid);
    
}
