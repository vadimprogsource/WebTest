namespace Test.Api.Infrastructure;

public interface IDataProvider<TEntity> where TEntity : IIdentity
{
    Task<IDataPage<TEntity>> GetByFilterAsync(IFilterData filter);
    Task<TEntity> GetDataAsync(Guid guid);
}

