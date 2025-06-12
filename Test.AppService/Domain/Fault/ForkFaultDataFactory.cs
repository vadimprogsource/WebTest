using System;
using Test.Api.Domain;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataFactory : DataFactory<IForkFault, ForkFault>, IForkFaultFactory
{
    public Task<IForkFault> CreateInstanceAsync(int forkId)
    {
        ForkFault fault = new()
        {
            ForkLiftId = forkId
        };

        return Task.FromResult((IForkFault)fault);
    }
}

