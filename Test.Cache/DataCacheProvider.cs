using Test.Api.Infrastructure.Providers;

namespace Test.Cache
{
    public class DataCacheProvider : IDataCacheProvider
    {
        public TimeSpan Expiration = TimeSpan.FromDays(1);
        public IDataCache<TEntity> GetCache<TEntity>()
        {
            DataCache<TEntity>? cache = DataCache<TEntity>.Default;

            if (cache == null)
            {
                cache = new DataCache<TEntity>(Expiration);
                DataCache<TEntity>.Default = cache;
            }
           
            return cache;
        }
    }
}
