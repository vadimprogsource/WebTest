using Test.Api.Infrastructure;

namespace TestWebApi.Provider;

public class EmptyDataCacheProvider : IDataCacheProvider
{

    private readonly struct DataCache<TEntity> : IDataCache<TEntity>
    {
        public Task<TEntity> AddAsync(TEntity obj) => Task.FromResult(obj);
        

        public Task<IDataPage<TEntity>> AddPageAsync(IDataPage<TEntity> page)=>Task.FromResult(page);
        

        public Task<bool> TryGetDataAsync(Guid guid, out TEntity obj)
        {
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            obj = default;
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            return Task.FromResult(false);
        }

        public Task<bool> TryGetPageAsync(IFilterData filter, out IDataPage<TEntity> page)
        {
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
            page = null;
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
            return Task.FromResult(false);
        }
    }


    public IDataCache<TEntity> GetCache<TEntity>() => new DataCache<TEntity>();
    
}
