using Microsoft.EntityFrameworkCore;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain
{
    public class ForkLiftRepository(ForkDbContext context) : DataRepository<ForkLift>(context)
    {
        protected override IQueryable<ForkLift> OnJoinWith(DbSet<ForkLift> dataSet) => dataSet.Include(x => x.ModifiedBy);

        public override async Task<bool> DeleteAsync(Guid guid)
        {

            if (await Context.Set<ForkFault>().AnyAsync(x => x.ForkLiftGuid == guid))
            {
                return false;
            }
            return await base.DeleteAsync(guid);
        }

    }
}

