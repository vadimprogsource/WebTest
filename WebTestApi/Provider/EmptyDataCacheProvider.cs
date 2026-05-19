using Test.Api.Infrastructure;

namespace TestWebApi.Provider;

public class EmptyDataCacheProvider : IDataCacheProvider
{
    public IDataCache<TEntity>? GetCache<TEntity>() => default;
    
}
