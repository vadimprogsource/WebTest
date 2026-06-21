using Test.Api.Domain.App;
using Test.AppService.Infrastructure.Runtime;
using Test.Entity.Domain.App;

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

