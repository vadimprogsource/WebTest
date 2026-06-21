namespace Test.Api.Infrastructure.Runtime
{
    public interface IDataMapper<TSource, TDesctination>
    {
        TDesctination New(TSource source);
        TDesctination Map(TSource source, TDesctination desctination);
    }
}

