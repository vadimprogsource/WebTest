namespace Test.Api.Infrastructure;

public interface IFilterData
{
    int PageIndex { get; }
    int PageSize { get; }
    string SearchPattern { get; }

}

