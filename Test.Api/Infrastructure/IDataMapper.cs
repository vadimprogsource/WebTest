namespace Test.Api.Infrastructure
{
    public interface IDataMapper<TSource, TDesctination>
    {
        TDesctination New(TSource source);
        TDesctination Map(TSource source, TDesctination desctination);
    }
}

