using System;
namespace Test.Api.Infrastructure
{
    public interface IDataValidator<TEntity>
    {
        public bool Validate(TEntity entity);
    }
}

