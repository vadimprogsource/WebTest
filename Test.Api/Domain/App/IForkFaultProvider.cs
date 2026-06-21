using Test.Api.Infrastructure.Query;

namespace Test.Api.Domain.App;

public interface IForkFaultProvider
{
    Task<IForkFault[]> GetFaultsAsync(Guid forkGuid, IFilterData filter);
}

