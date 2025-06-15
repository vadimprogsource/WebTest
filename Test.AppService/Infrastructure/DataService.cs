using System;
using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity;

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

    public abstract Task<TEntity> OnCreateNewAsync(TInterface source);
    public abstract void          OnInsertNewAsync(TEntity entity);
    public abstract Task<TEntity> OnUpdateAsync(TInterface source, TEntity entity);


    public virtual async Task<TInterface> ApplyUpdateAsync(TInterface entity)
    {
        TEntity data = await Repository.SelectAsync(entity.Guid);
        data = await OnUpdateAsync(entity, data);
        return await Repository.UpdateAsync(data);
    }


    public virtual async Task<TInterface> InserNewAsync(TInterface entity)
    {
        TEntity data = await OnCreateNewAsync(entity);
        OnInsertNewAsync(data);
        data = await Repository.InsertAsync(data);
        return await Repository.SelectAsync(data.Guid);
    }

    public virtual Task<bool> ExecuteDeleteAsync(Guid guid) => Repository.DeleteAsync(guid);
}

public abstract class EntityDataService<TInterface, TEntity> : DataService<TInterface, TEntity> where TInterface : IEntity where TEntity : EntityBase, TInterface
{
    protected EntityDataService(IUserContext context, IDataRepository<TEntity> repository) : base(context, repository)
    {
    }

    public override void OnInsertNewAsync(TEntity entity)
    {
        entity.Guid = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
    }

}

