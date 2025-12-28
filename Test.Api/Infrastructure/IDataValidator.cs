namespace Test.Api.Infrastructure
{
    public interface IDataValidator<TEntity>
    {
        bool Validate(TEntity entity);
        IEnumerable<IError> Errors { get; }
    }
}

