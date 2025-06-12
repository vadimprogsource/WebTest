using System;
using Test.Api;
using Test.Api.Infrastructure;

namespace Test.AppService.Infrastructure
{
    public class DataFactory<TInterface, TEntity> : IDataFactory<TInterface> where TInterface : IIdentity where TEntity : class, TInterface
    { 
        async Task<TInterface> IDataFactory<TInterface>.CreateInstanceAsync() => await CreateInstanceAsync() is TInterface obj ? obj : throw new NotSupportedException();

        protected virtual Task<TEntity> CreateInstanceAsync() => Task.FromResult(CreateInstance());
        protected virtual TEntity CreateInstance() => Activator.CreateInstance<TEntity>();
    }
}

