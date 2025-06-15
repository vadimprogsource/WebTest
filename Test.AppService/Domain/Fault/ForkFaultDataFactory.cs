using System;
using Test.Api.Domain;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataFactory : DataFactory<IForkFault, ForkFault>, IForkFaultFactory
{
    public Task<IForkFault> CreateInstanceAsync(Guid forkGuid)
    {
        ForkFault fault = new()
        {
            ForkLiftGuid = forkGuid
        };

        return Task.FromResult((IForkFault)fault);
    }
}

