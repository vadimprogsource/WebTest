using System;
using Test.Api;
using Test.Api.Infrastructure;

namespace Test.AppService.Infrastructure
{
    public abstract class DataAccessProvider<TInterface,TEntity> : IDataAccessProvider<TInterface> where TInterface : IIdentity where TEntity : class , TInterface
    {
        protected readonly IDataRepository<TEntity> Repository;
        protected readonly IUserContext             UserContext;

        public DataAccessProvider(IUserContext context , IDataRepository<TEntity> repository)
        {
            Repository = repository;
            UserContext = context;
        }

        public abstract Task<TEntity> OnCreateNewAsync(int ownerId ,TInterface source);
        public abstract Task<TEntity> OnUpdateAsync   (TInterface source, TEntity entity);
        

        public abstract Task<TInterface[]> ApplyFilterAsync(IFilterData filter);


        public virtual async Task<TInterface> ApplyUpdateAsync(TInterface entity)
        {
            TEntity data = await Repository.SelectAsync(entity.Id);
            data = await OnUpdateAsync(entity, data);
            return await Repository.UpdateAsync(data);
        }


        public virtual async Task<TInterface> InserNewAsync(TInterface entity, int ownerId = 0)
        {
            TEntity data = await OnCreateNewAsync(ownerId, entity);
            data = await Repository.InsertAsync(data);
            return await Repository.SelectAsync(data.Id);
        }

        public virtual Task<bool> ExecuteDeleteAsync(int id) => Repository.DeleteAsync(id);

        public virtual async Task<TInterface> GetByIdAsync(int id) => await Repository.SelectAsync(id);
        
    }
}

