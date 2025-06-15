using System;
using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;

namespace Test.AppService.Infrastructure;

public  class DataProvider<TInterface, TEntity> : IDataProvider<TInterface> where TInterface : IIdentity where TEntity : class, TInterface
{
    protected readonly IDataRepository<TEntity> Repository;


    public DataProvider(IDataRepository<TEntity> repository)
    {
        Repository = repository;

    }


    public async Task<TInterface[]> GetByFilterAsync(IFilterData filter)
    {
          return await Repository.SelectAsync(query => ApplyFilter(query,filter));
    }

    public virtual async Task<TInterface> GetDataAsync(Guid guid) => await Repository.SelectAsync(guid);



    protected virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity>  query,  IFilterData filter)
    {
        return query.Take(filter.MaxCount);
        //return query.OrderBy(x => x.Id).Take(filter.MaxCount);
    }

    
}

public class EntityDataProvider<TInterface, TEntity> : DataProvider<TInterface, TEntity> where TInterface : IEntity where TEntity : EntityBase, TInterface
{
    public EntityDataProvider(IDataRepository<TEntity> repository) : base(repository)
    {
    }

    protected override IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, IFilterData filter)
    {
        return base.ApplyFilter(query.OrderBy(x=>x.CreatedAt), filter);
    }
}

