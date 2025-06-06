using System;
namespace Test.Api.Infrastructure;

public interface IDataAccessProvider<TEntity> where TEntity : IIdentity
{

    Task<TEntity[]> ApplyFilterAsync(IFilterData filter);
    Task<TEntity>   GetByIdAsync(int Id); 
    Task<TEntity>   InserNewAsync(TEntity entity,int ownerId =  0);
    Task<TEntity>   ApplyUpdateAsync(TEntity entity);
    Task<bool>      ExecuteDeleteAsync(int Id);

}

