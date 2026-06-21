namespace Test.Api.Infrastructure.Runtime
{
    public interface IDataValidator<TEntity>
    {
        bool Validate(TEntity entity);
        IEnumerable<IError> Errors { get; }
    }
}

