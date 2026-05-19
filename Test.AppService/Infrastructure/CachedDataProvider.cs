using System;
using System.Collections.Generic;
using System.Text;
using Test.Api;
using Test.Api.Infrastructure;

namespace Test.AppService.Infrastructure;

public class CachedDataProvider<TInterface,TEntity>(IDataRepository<TEntity> repository , IDataCache<TEntity> cache) : DataProvider<TInterface,TEntity>(repository) where TInterface : IIdentity
    where TEntity : class, TInterface
{
    protected IDataCache<TEntity> Cache = cache;

    public async override Task<TInterface> GetDataAsync(Guid guid)
    {
        if (await Cache.TryGetDataAsync(guid, out TEntity obj))
        {
            return obj;
        }

        return await base.GetDataAsync(guid);
    }

    public async override Task<IDataPage<TInterface>> GetByFilterAsync(IFilterData filter)
    {
        if (await Cache.TryGetPageAsync(filter, out IDataPage<TEntity> page))
        {
            return page.Convert<TInterface>(x => x);
        }

        return await base.GetByFilterAsync(filter);
    }
}
