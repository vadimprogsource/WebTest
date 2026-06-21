using Test.Api.Infrastructure.Query;

namespace Test.Api.Infrastructure.Providers;

public interface IDataProvider<TEntity> where TEntity : IIdentity
{
    Task<IDataPage<TEntity>> GetByFilterAsync(IFilterData filter);
    Task<TEntity> GetDataAsync(Guid guid);
}

