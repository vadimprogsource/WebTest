using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;

namespace Test.AppService.Infrastructure;

public class DataProvider<TInterface, TEntity>(IDataRepository<TEntity> repository) : IDataProvider<TInterface>
    where TInterface : IIdentity
    where TEntity : class, TInterface
{
    protected readonly IDataRepository<TEntity> Repository = repository;



    protected virtual Task<TEntity> GetEntityAsync(Guid guid)=> Repository.Context.FirstAsync(x => x.Guid == guid);


    protected virtual async Task<IDataPage<TEntity>> GetPageByFilterAsync(IFilterData filter)
    {

        IQueryContext<TEntity> context = Repository.Context;
        ApplyFilter(context, filter);
        return await DataPage<TEntity>.CreatePageAsync(context, filter.PageIndex, filter.PageSize);
    }

    public virtual async Task<TInterface> GetDataAsync(Guid guid) => await GetEntityAsync(guid);

    public virtual Task<IDataPage<TInterface>> GetByFilterAsync(IFilterData filter) => GetPageByFilterAsync(filter).ContinueWith(x=>x.Result.Convert<TInterface>(x=>x));
    protected virtual void ApplyFilter(IQueryContext<TEntity> context, IFilterData filter)
    {

    }

}

public class EntityDataProvider<TInterface, TEntity>(IDataRepository<TEntity> repository)
    : DataProvider<TInterface, TEntity>(repository)
    where TInterface : IEntity
    where TEntity : EntityBase, TInterface
{

    protected override void ApplyFilter(IQueryContext<TEntity> context, IFilterData filter)
    {
        context.OrderBy(x => x.CreatedAt);
        base.ApplyFilter(context, filter);
    }

  
}

