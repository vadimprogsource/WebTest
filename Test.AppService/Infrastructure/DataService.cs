using System;
using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test.AppService.Infrastructure;

public abstract class DataService<TInterface, TEntity> : IDataService<TInterface> where TInterface : IIdentity where TEntity : class, TInterface
{
    protected readonly IDataRepository<TEntity> Repository;
    protected readonly IUserContext UserContext;

    public DataService(IUserContext context, IDataRepository<TEntity> repository)
    {
        Repository = repository;
        UserContext = context;
    }



    public async Task<TInterface> InsertNewAsync(TInterface entity)
    {
        TEntity data = await OnCreateNewAsync(entity);
        return await InsertNewAsync(data);

    }


    protected virtual async Task<TEntity> InsertNewAsync(TEntity entity)
    {
        return await Repository.InsertAsync(entity);
    }


    public async Task<TInterface> ApplyUpdateAsync(TInterface entity)
    {
        TEntity data = await Repository.SelectAsync(entity.Guid);
        data = await OnUpdateAsync(entity, data);
        return await ApplyUpdateAsync(data);
    }
    protected async Task<TEntity> ApplyUpdateAsync(TEntity entity)
    {
        return await Repository.UpdateAsync(entity);
    }

    public abstract Task<TEntity> OnCreateNewAsync(TInterface source);
    public abstract Task<TEntity> OnUpdateAsync(TInterface source, TEntity entity);


    public virtual Task<bool> ExecuteDeleteAsync(Guid guid) => Repository.DeleteAsync(guid);
}

public abstract class EntityDataService<TInterface, TEntity> : DataService<TInterface, TEntity> where TInterface : IEntity where TEntity : EntityBase, TInterface
{
    protected EntityDataService(IUserContext context, IDataRepository<TEntity> repository) : base(context, repository)
    {
    }

    protected override Task<TEntity> InsertNewAsync(TEntity entity)
    {
        entity.Guid = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        return base.InsertNewAsync(entity);
    }

}

