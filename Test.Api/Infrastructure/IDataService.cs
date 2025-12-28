namespace Test.Api.Infrastructure
{
    public interface IDataService<TEntity> where TEntity : IIdentity
    {
        Task<TEntity> InsertNewAsync(TEntity entity);
        Task<TEntity> ApplyUpdateAsync(TEntity entity);
        Task<bool> ExecuteDeleteAsync(Guid guid);
    }
}

