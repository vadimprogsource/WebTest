using System;
using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test.AppService.Infrastructure;

public abstract class DataService<TInterface, TEntity> : IDataService<TInterface> where TInterface : IIdentity where TEntity : class, TInterface
{
    protected readonly IDataRepository<TEntity> Repository;
    protected readonly IDataMapper<TInterface, TEntity> Mapper;

    public DataService(IDataRepository<TEntity> repository, IDataMapper<TInterface, TEntity> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }



    public async Task<TInterface> InsertNewAsync(TInterface entity)
    {
        TEntity data =  Mapper.New(entity);
        return await InsertNewAsync(data);

    }


    protected virtual async Task<TEntity> InsertNewAsync(TEntity entity)
    {
        return await Repository.InsertAsync(entity);
    }


    public async  Task<TInterface> ApplyUpdateAsync(TInterface entity)
    {
        TEntity data = await Repository.SelectAsync(entity.Guid);
        Mapper.Map(entity,data);
        return await ApplyUpdateAsync(data);
    }
    protected virtual async Task<TEntity> ApplyUpdateAsync(TEntity entity)
    {
        return await Repository.UpdateAsync(entity);
    }



    public virtual Task<bool> ExecuteDeleteAsync(Guid guid) => Repository.DeleteAsync(guid);
}

public abstract class EntityDataService<TInterface, TEntity> : DataService<TInterface, TEntity> where TInterface : IEntity where TEntity : EntityBase, TInterface
{
    protected EntityDataService(IDataRepository<TEntity> repository, IDataMapper<TInterface, TEntity> mapper) : base(repository, mapper)
    {
    }

    protected override Task<TEntity> InsertNewAsync(TEntity entity)
    {
        entity.Guid = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        return base.InsertNewAsync(entity);
    }

}



