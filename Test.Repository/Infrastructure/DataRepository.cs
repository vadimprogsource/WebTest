using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Test.Api;
using Test.Api.Infrastructure;

namespace Test.Repository.Infrastructure
{
    public class DataRepository<TEntity> : IDataRepository<TEntity> where TEntity : class ,  IIdentity
    {

        protected readonly DbContext Context;

        public DataRepository(DbContext context)
        {
            Context = context;
        }


        protected IQueryable<TEntity> GetQueryable() => Context.Set<TEntity>().AsQueryable();


        public virtual async  Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Attach(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int Id)
        {
            return await Context.Set<TEntity>().Where(x => x.Id == Id).ExecuteDeleteAsync() > 0;
        }

      
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            DbSet<TEntity> entitySet = Context.Set<TEntity>(); 
            Context.Attach(entity);
            await entitySet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        protected virtual IQueryable<TEntity> OnJoinWith(DbSet<TEntity> dataSet) => dataSet;

        public virtual async Task<TEntity[]> SelectAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> filter )
        {
            return await filter(OnJoinWith(Context.Set<TEntity>())).ToArrayAsync();
        }

        public virtual Task<TEntity> SelectAsync(int id)
        {
            return OnJoinWith(Context.Set<TEntity>()).SingleAsync(x => x.Id == id);
        }



        public Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> condition) => OnJoinWith(Context.Set<TEntity>()).FirstOrDefaultAsync(condition);


        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> condition) => (await Context.Set<TEntity>().Where(condition).ExecuteDeleteAsync())>0;
        
    }
}

