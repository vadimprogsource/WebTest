using Test.Api.Domain;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataFactory : DataFactory<IForkFault, ForkFault>, IForkFaultFactory
{
    public Task<IForkFault> CreateInstanceAsync(Guid forkGuid)
    {
        IForkFault fault = new ForkFault()
        {
            ForkLiftGuid = forkGuid
        };

        return Task.FromResult(fault)!;
    }
}

