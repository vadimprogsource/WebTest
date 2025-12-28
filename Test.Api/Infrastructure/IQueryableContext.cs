namespace Test.Api.Infrastructure
{
    public interface IQueryableContext
    {
        IQueryable<T> AsQueryable<T>();
    }
}

