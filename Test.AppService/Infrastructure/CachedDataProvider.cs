using System;
using System.Collections.Generic;
using System.Text;
using Test.Api;
using Test.Api.Infrastructure;

namespace Test.AppService.Infrastructure;

public abstract class CachedDataProvider<TInterface,TEntity>(IDataRepository<TEntity> repository , IDataCacheProvider provider) : DataProvider<TInterface,TEntity>(repository) where TInterface : IIdentity
    where TEntity : class, TInterface
{
    protected IDataCache<TEntity> Cache = provider.GetCache<TEntity>();

    protected async override Task<TEntity> GetEntityAsync(Guid guid)
    {
        if (await Cache.TryGetDataAsync(guid, out TEntity obj))
        {
            return obj;
        }

        return await Cache.AddAsync(await base.GetEntityAsync(guid));
    }

    protected async override Task<IDataPage<TEntity>> GetPageByFilterAsync(IFilterData filter)
    {
        if (await Cache.TryGetPageAsync(filter, out IDataPage<TEntity> page))
        {
            return page;
        }

        return await base.GetPageByFilterAsync(filter);
    }
}
