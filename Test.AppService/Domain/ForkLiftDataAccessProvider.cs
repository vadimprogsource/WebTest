using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain
{
    public class ForkLiftDataAccessProvider : DataAccessProvider<IForkLift, ForkLift> , IDataFactory<IForkLift> , IForkDataAccessProvider
    {


        public ForkLiftDataAccessProvider(IUserContext context, IDataRepository<ForkLift> repository) : base(context, repository)
        {
        }

        public override async Task<IForkLift[]> ApplyFilterAsync(IFilterData filter)
        {
            return await Repository.SelectAsync(query =>
            {
                if (!string.IsNullOrWhiteSpace(filter.SearchPattern))
                {
                    query = query.Where(x => x.Number.Contains(filter.SearchPattern));
                }

               return query.OrderBy(x=>x.Id).Take(filter.MaxCount);
            });
        }

        private async Task<ForkLift> UpdateModifiedAsync(ForkLift entity)
        {
            entity.ModifiedAt   = DateTime.UtcNow;
            entity.ModifiedBy = (User)(await UserContext.GetUserAsync());
            entity.ModifiedById = entity.ModifiedBy.Id;
            return entity;
        }

    

        public override Task<ForkLift> OnCreateNewAsync(int ownerId, IForkLift source) => UpdateModifiedAsync(new ForkLift().Update(source));
       
        public override Task<ForkLift> OnUpdateAsync(IForkLift source, ForkLift entity) => UpdateModifiedAsync(entity.Update(source));

        public async Task<IForkLift> CreateInstanceAsync(int ownerId = 0)=> await UpdateModifiedAsync(new ForkLift());

        public async Task<IForkLift> UpdateIsActiveAsync(int id, bool active)
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

