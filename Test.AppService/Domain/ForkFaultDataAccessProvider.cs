using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain
{
    public class ForkFaultDataAccessProvider : DataAccessProvider<IForkFault,ForkFault> , IDataFactory<IForkFault>
    {

        private readonly IDataRepository<ForkLift> fork_lift_repository;

        public ForkFaultDataAccessProvider(IUserContext context, IDataRepository<ForkFault> repository, IDataRepository<ForkLift> forkLift) : base(context, repository)
        {
            fork_lift_repository = forkLift;
        }

        public override async Task<IForkFault[]> ApplyFilterAsync(IFilterData filter)
        {
            return await Repository.SelectAsync(query => query.Where(x => x.ForkLiftId == filter.OwnerId).OrderBy(x=>x.Id).Take(filter.MaxCount));
        }

        public override async Task<IForkFault> InserNewAsync(IForkFault entity, int ownerId = 0)
        {
            ForkFault fault = new(entity)
            {
                ForkLiftId = ownerId
            };

            fault = await Repository.InsertAsync(fault);
            return await GetByIdAsync(fault.Id);
        }

     

        public override async Task<ForkFault> OnCreateNewAsync(int ownerId, IForkFault source)
        {
            ForkFault entity = new ForkFault().Update(source);
            entity.ForkLift = await fork_lift_repository.SelectAsync( ownerId);
            entity.ForkLiftId = entity.ForkLift.Id;
            return entity;
        }

        public override Task<ForkFault> OnUpdateAsync(IForkFault source, ForkFault entity) =>Task.FromResult( entity.Update(source));

        public Task<IForkFault> CreateInstanceAsync(int ownerId = 0)
        {
            ForkFault entity = new()
            {
                ProblemDetectedAt = DateTime.UtcNow,
                ForkLiftId = ownerId
            };
            return Task.FromResult((IForkFault)entity);
        }
    }
}

