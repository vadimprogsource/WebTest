using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataService(
    IDataRepository<ForkFault> repository,
    IDataMapper<IForkFault, ForkFault> mapper,
    IDataRepository<ForkLift> forkLiftRepository)
    : EntityDataService<IForkFault, ForkFault>(repository, mapper), IForkFaultService
{
    public async Task<IForkFault> AddFaultAsync(Guid forkGuid, IForkFault fault)
    {
        ForkFault entity = new ForkFault().Update(fault);
        entity.ForkLift = await forkLiftRepository.SelectAsync(forkGuid);
        entity.ForkLiftGuid = entity.ForkLift.Guid;
        return await InsertNewAsync(entity);

    }
}



