using Test.Api.Domain.App;
using Test.AppService.Infrastructure;
using Test.Entity.Domain.App;

namespace Test.AppService.Domain.Fork;

public class ForkLiftDataMapper : EntityDataMapper<IForkLift, ForkLift>
{
    public ForkLiftDataMapper() : base()
    {
        Exclude(x => x.ModifiedBy);
        Exclude(x => x.ModifiedAt);
        Exclude(x => x.Faults);
    }
}

