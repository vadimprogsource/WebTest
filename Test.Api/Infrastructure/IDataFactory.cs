namespace Test.Api.Infrastructure
{
    public interface IDataFactory<TEntity> where TEntity : IIdentity
    {
        Task<TEntity> CreateInstanceAsync();
    }
}

