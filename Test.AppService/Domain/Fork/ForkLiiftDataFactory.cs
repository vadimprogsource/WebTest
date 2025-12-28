using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork
{
    public class ForkLiftDataFactory(IUserContext context) : DataFactory<IForkLift, ForkLift>
    {
        protected async override Task<ForkLift> CreateInstanceAsync()
        {
            ForkLift fork = new()
            {
                ModifiedBy = (User)await context.GetUserAsync(),
                ModifiedAt = DateTime.UtcNow
            };

            fork.ModifiedByGuid = fork.ModifiedBy.Guid;

            return fork;
        }
    }
}

