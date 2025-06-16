using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataService : EntityDataService<IForkFault, ForkFault> , IForkFaultService
{
    private readonly IDataRepository<ForkLift> fork_lift_repository;

    public ForkFaultDataService(IUserContext context, IDataRepository<ForkFault> repository, IDataRepository<ForkLift> forkRepository) : base(context, repository)
    {
        fork_lift_repository = forkRepository;
    }

   

    public override  Task<ForkFault> OnCreateNewAsync(IForkFault source) => Task.FromResult(new ForkFault().Update(source));
 

    public override Task<ForkFault> OnUpdateAsync(IForkFault source, ForkFault entity) => Task.FromResult(entity.Update(source));



    public async Task<IForkFault> AddFaultAsync(Guid forkGuid, IForkFault fault)
    {
        ForkFault entity = new ForkFault().Update(fault);
        entity.ForkLift = await fork_lift_repository.SelectAsync(forkGuid);
        entity.ForkLiftGuid = entity.ForkLift.Guid;
        return await InsertNewAsync(entity);

    }
}



