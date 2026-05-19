using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;

namespace Test.AppService.Infrastructure;

public class DataProvider<TInterface, TEntity>(IDataRepository<TEntity> repository) : IDataProvider<TInterface>
    where TInterface : IIdentity
    where TEntity : class, TInterface
{
    protected readonly IDataRepository<TEntity> Repository = repository;


    public virtual async Task<IDataPage<TInterface>> GetByFilterAsync(IFilterData filter)
    {

        IQueryContext<TEntity> context = Repository.Context;
        ApplyFilter(context, filter);
        return (await DataPage<TEntity>.CreatePageAsync(context, filter.PageIndex, filter.PageSize)).Convert<TInterface>(x => x);
    }

    public virtual async Task<TInterface> GetDataAsync(Guid guid)=> await Repository.Context.FirstAsync(x => x.Guid == guid);
    

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

