using System;
using Microsoft.EntityFrameworkCore;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain
{
    public class ForkLiftRepository : DataRepository<ForkLift>
    {
        public ForkLiftRepository(ForkDbContext context) : base(context)
        {
        }

        protected override IQueryable<ForkLift> OnJoinWith(DbSet<ForkLift> dataSet) => dataSet.Include(x => x.ModifiedBy);

        public async override Task<bool> DeleteAsync(int id)
        {

            if(await Context.Set<ForkFault>().AnyAsync(x => x.ForkLiftId == id))
            {
                return false;
            }
            return await base.DeleteAsync(id);
        }

    }
}

