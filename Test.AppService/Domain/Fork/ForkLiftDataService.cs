using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork
{
    public class ForkLiftDataService : EntityDataService<IForkLift, ForkLift> , IForkLiftService
    {
        public ForkLiftDataService(IUserContext context, IDataRepository<ForkLift> repository) : base(context, repository)
        {
        }

        private async Task<ForkLift> UpdateModifiedAsync(ForkLift entity)
        {
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = (User)(await UserContext.GetUserAsync());
            entity.ModifiedByGuid = entity.ModifiedBy.Guid;
            return entity;
        }



        public override Task<ForkLift> OnCreateNewAsync(IForkLift source) => UpdateModifiedAsync(new ForkLift().Update(source));

        public override Task<ForkLift> OnUpdateAsync(IForkLift source, ForkLift entity) => UpdateModifiedAsync(entity.Update(source));


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

