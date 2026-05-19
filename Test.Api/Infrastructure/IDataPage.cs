namespace Test.Api.Infrastructure;

public interface IDataPage<TEntity> : IEnumerable<TEntity>
{
    int PageIndex { get; }
    int PageSize { get; }
    int Pages { get; }
    int Total { get; }


    IDataPage<TObject> Convert<TObject>(Func<TEntity, TObject> convertor);
}