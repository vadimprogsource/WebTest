using Test.Api.Infrastructure.Query;

namespace TestWebApi.Models
{
    public record FilterModel : IFilterData
    {
        public int MaxCount { get; set; } = 50;
        public string SearchPattern { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}

