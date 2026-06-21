namespace Test.Api.Infrastructure.Runtime
{
    public interface IDataFactory<TEntity> where TEntity : IIdentity
    {
        Task<TEntity> CreateInstanceAsync();
    }
}

