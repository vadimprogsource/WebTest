namespace Test.Api.Infrastructure.Query
{
    public interface IQueryableContext
    {
        IQueryable<T> AsQueryable<T>();
    }
}

