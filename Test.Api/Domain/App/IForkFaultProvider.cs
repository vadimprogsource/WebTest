using Test.Api.Infrastructure;

namespace Test.Api.Domain.App;

public interface IForkFaultProvider
{
    Task<IForkFault[]> GetFaultsAsync(Guid forkGuid, IFilterData filter);
}

