using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork;

public class ForkLiftDataProvider : EntityDataProvider<IForkLift, ForkLift>
{


    public ForkLiftDataProvider(IDataRepository<ForkLift> repository) : base(repository)
    {
    }

    protected override void ApplyFilter(IQueryContext<ForkLift> context, IFilterData filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchPattern))
        {
            context.Where(x => x.Number.Contains(filter.SearchPattern));
        }

        base.ApplyFilter(context, filter);

    }

}

