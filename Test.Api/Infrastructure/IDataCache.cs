using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure;

public interface IDataCache<TEntity>
{
    Task<bool> TryGetDataAsync(Guid guid, out TEntity obj);
    Task<bool> TryGetPageAsync(IFilterData filter,out IDataPage<TEntity> page); 
}
