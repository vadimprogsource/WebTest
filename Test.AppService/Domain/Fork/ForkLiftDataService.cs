using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork
{
    public class ForkLiftDataService : EntityDataService<IForkLift, ForkLift>, IForkLiftService
    {

        private readonly IUserContext userContext;

        public ForkLiftDataService(IDataRepository<ForkLift> repository, IDataMapper<IForkLift, ForkLift> mapper, IUserContext context) : base(repository, mapper)
        {
            userContext = context;
        }

        private async Task<ForkLift> UpdateModifiedAsync(ForkLift entity)
        {
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = (User)(await userContext.GetUserAsync());
            entity.ModifiedByGuid = entity.ModifiedBy.Guid;
            return entity;
        }


        protected override async Task<ForkLift> InsertNewAsync(ForkLift entity)
        {
            await UpdateModifiedAsync(entity);
            return await base.InsertNewAsync(entity);
        }

        protected override async Task<ForkLift> ApplyUpdateAsync(ForkLift entity)
        {
            await UpdateModifiedAsync(entity);
            return await base.ApplyUpdateAsync(entity);
        }


        public async Task<IForkLift> SetActiveAsync(Guid guid, bool active)
        {
            ForkLift obj = await Repository.SelectAsync(guid);

            if (obj.IsActive != active)
            {
                obj.IsActive = active;
                obj = await Repository.UpdateAsync(await UpdateModifiedAsync(obj));
            }

            return obj;
        }


    }
}

