using System;
using System.Collections.Generic;
using System.Text;
using Test.Api;
using Test.Api.Infrastructure.Providers;
using Test.Api.Infrastructure.Repositories;
using Test.Api.Infrastructure.Runtime;

namespace Test.AppService.Infrastructure.Services;

public abstract class CachedDataService<TInterface, TEntity>(IDataRepository<TEntity> repository,IDataMapper<TInterface, TEntity> mapper, IDataCacheProvider provider) : DataService<TInterface,TEntity>(repository,mapper)
    where TInterface : IIdentity
    where TEntity : class, TInterface
{
    protected IDataCache<TEntity> Cache = provider.GetCache<TEntity>();

    protected async override Task<TEntity> InsertNewAsync(TEntity entity)
    {
        return await Cache.PutDataAsync( await base.InsertNewAsync(entity));
    }

    protected async override Task<TEntity> ApplyUpdateAsync(TEntity entity)
    {
        return await Cache.PutDataAsync(await base.ApplyUpdateAsync(entity));
    }

    public override async Task<bool> ExecuteDeleteAsync(Guid guid)
    {
        if (await base.ExecuteDeleteAsync(guid))
        {
            await Cache.DeleteAsync(guid);
            return true;
        }
   
        return false;
    }

}