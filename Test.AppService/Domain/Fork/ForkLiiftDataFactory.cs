using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork
{
    public class ForkLiftDataFactory : DataFactory<IForkLift,ForkLift>
    {
        private readonly IUserContext userContext;

        public ForkLiftDataFactory(IUserContext context)
        {
            userContext = context;
        }

        protected async override Task<ForkLift> CreateInstanceAsync()
        {
            ForkLift fork = new()
            {
                ModifiedBy = (User)await userContext.GetUserAsync(),
                ModifiedAt = DateTime.UtcNow
            };

            fork.ModifiedByGuid = fork.ModifiedBy.Guid;

            return fork;
        }
    }
}

