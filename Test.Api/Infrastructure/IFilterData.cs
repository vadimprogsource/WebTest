using System;
namespace Test.Api.Infrastructure;

public interface IFilterData
{
    int MaxCount { get; }
    string SearchPattern { get; }
    int OwnerId { get; }
}

