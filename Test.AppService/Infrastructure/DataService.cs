using System;
using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test.AppService.Infrastructure;

public abstract class DataService<TInterface, TEntity>(
    IDataRepository<TEntity> repository,
    IDataMapper<TInterface, TEntity> mapper)
    : IDataService<TInterface>
    where TInterface : IIdentity
    where TEntity : class, TInterface
{
    protected readonly IDataRepository<TEntity> Repository = repository;
    private readonly IDataMapper<TInterface, TEntity> _mapper = mapper;


    public async Task<TInterface> InsertNewAsync(TInterface entity)
    {
        TEntity data =  _mapper.New(entity);
        return await InsertNewAsync(data);

    }


    protected virtual async Task<TEntity> InsertNewAsync(TEntity entity)
    {
        return await Repository.InsertAsync(entity);
    }


    public async  Task<TInterface> ApplyUpdateAsync(TInterface entity)
    {
        TEntity data = await Repository.SelectAsync(entity.Guid);
        _mapper.Map(entity,data);
        return await ApplyUpdateAsync(data);
    }
    protected virtual async Task<TEntity> ApplyUpdateAsync(TEntity entity)
    {
        return await Repository.UpdateAsync(entity);
    }



    public virtual Task<bool> ExecuteDeleteAsync(Guid guid) => Repository.DeleteAsync(guid);
}

public abstract class EntityDataService<TInterface, TEntity>(
    IDataRepository<TEntity> repository,
    IDataMapper<TInterface, TEntity> mapper)
    : DataService<TInterface, TEntity>(repository, mapper)
    where TInterface : IEntity
    where TEntity : EntityBase, TInterface
{
    protected override Task<TEntity> InsertNewAsync(TEntity entity)
    {
        entity.Guid = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        return base.InsertNewAsync(entity);
    }

}



