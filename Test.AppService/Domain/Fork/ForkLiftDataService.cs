using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork
{
    public class ForkLiftDataService : DataService<IForkLift, ForkLift> , IForkLiftService
    {
        public ForkLiftDataService(IUserContext context, IDataRepository<ForkLift> repository) : base(context, repository)
        {
        }

        private async Task<ForkLift> UpdateModifiedAsync(ForkLift entity)
        {
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = (User)(await UserContext.GetUserAsync());
            entity.ModifiedById = entity.ModifiedBy.Id;
            return entity;
        }



        public override Task<ForkLift> OnCreateNewAsync(IForkLift source) => UpdateModifiedAsync(new ForkLift().Update(source));

        public override Task<ForkLift> OnUpdateAsync(IForkLift source, ForkLift entity) => UpdateModifiedAsync(entity.Update(source));


        public async Task<IForkLift> SetActiveAsync(int id, bool active)
        {
            ForkLift obj = await Repository.SelectAsync(id);

            if (obj.IsActive != active)
            {
                obj.IsActive = active;
                obj = await Repository.UpdateAsync(await UpdateModifiedAsync(obj));
            }

            return obj;
        }

    

    }
}

