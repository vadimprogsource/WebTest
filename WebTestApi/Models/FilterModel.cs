using Test.Api.Infrastructure;

namespace TestWebApi.Models
{
    public record FilterModel : IFilterData
    {
        public int MaxCount { get; set; } = 50;
        public string SearchPattern { get; set; } = string.Empty;
        public int OwnerId { get; set; }
    }
}

