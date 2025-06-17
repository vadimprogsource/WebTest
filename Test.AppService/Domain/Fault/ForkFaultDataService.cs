using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataService : EntityDataService<IForkFault, ForkFault> , IForkFaultService
{
    private readonly IDataRepository<ForkLift> fork_lift_repository;

    public ForkFaultDataService(IDataRepository<ForkFault> repository, IDataMapper<IForkFault, ForkFault> mapper, IDataRepository<ForkLift> fork_lift_repository) : base(repository, mapper)
    {
        this.fork_lift_repository = fork_lift_repository;
    }

    public async Task<IForkFault> AddFaultAsync(Guid forkGuid, IForkFault fault)
    {
        ForkFault entity = new ForkFault().Update(fault);
        entity.ForkLift = await fork_lift_repository.SelectAsync(forkGuid);
        entity.ForkLiftGuid = entity.ForkLift.Guid;
        return await InsertNewAsync(entity);

    }
}



