using System;
using System.Collections.Generic;
using System.Text;
using Test.Api;
using Test.Api.Infrastructure.Providers.Cache;
using Test.Api.Infrastructure.Query;

namespace Test.Cache;

public class DataCache<TEntity> : IDataCache<TEntity>
{
    private readonly Dictionary<Guid, DataCacheEntry<TEntity>> cache = new();


    public static DataCache<TEntity>? Default = null;

    private  TimeSpan expiration;

    public TimeSpan Expiration {get => expiration; set => expiration = value; }

    private Guid keyOf(TEntity obj)
    {
        if (obj is IEntity entity)
        {
            return entity.Guid;
        }

        throw new NotSupportedException();
    }


    public DataCache(TimeSpan expiration)
    {
        this.expiration = expiration;
    }

    public Task<TEntity> PutDataAsync(TEntity obj)
    {
        cache[keyOf(obj)] = new DataCacheEntry<TEntity>(obj, expiration);
        return Task.FromResult(obj);
    }

    public Task<IDataPage<TEntity>> PutPageAsync(IDataPage<TEntity> page)
    {
        foreach (TEntity obj in page)
        {
            cache[keyOf(obj)] = new DataCacheEntry<TEntity>(obj, expiration);
        }

        return Task.FromResult(page);
    }

    public Task DeleteAsync(Guid guid)
    {
        cache.Remove(guid);
        return Task.CompletedTask;
    }

    public Task<bool> TryGetDataAsync(Guid guid, out TEntity obj)
    {
        if (cache.TryGetValue(guid, out DataCacheEntry<TEntity> entry))
        {
            if (!entry.HasExpired)
            {
                obj = entry.Ptr;
                return Task.FromResult(true);
            }

            cache.Remove(keyOf(entry.Ptr));
        }

#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
        obj = default;
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
        return Task.FromResult(false); ;
    }

    public Task<bool> TryGetPageAsync(IFilterData filter, out IDataPage<TEntity> page)
    {
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
        page = default;
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
        return Task.FromResult(false);
    }
}
