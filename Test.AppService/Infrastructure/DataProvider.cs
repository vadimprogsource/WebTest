using System;
using Test.Api;
using Test.Api.Infrastructure;

namespace Test.AppService.Infrastructure
{
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

        public virtual async Task<TInterface> GetByIdAsync(int id) => await Repository.SelectAsync(id);



        protected virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity>  query,  IFilterData filter)
        {
            return query.OrderBy(x => x.Id).Take(filter.MaxCount);
        }

        
    }
}

