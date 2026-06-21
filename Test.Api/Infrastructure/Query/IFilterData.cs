namespace Test.Api.Infrastructure.Query;

public interface IFilterData
{
    int PageIndex { get; }
    int PageSize { get; }
    string SearchPattern { get; }

}

