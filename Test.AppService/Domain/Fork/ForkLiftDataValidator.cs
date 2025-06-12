using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork;

public class ForkLiftDataValidator : DataValidator<IForkLift>
{

    protected override void OnValidate(IForkLift entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Number)) Throw(x => x.Number,"InputError","Invalid Number");
        if (string.IsNullOrWhiteSpace(entity.Brand)) Throw(x => x.Brand);

        if (entity.Capacity == 0)
        {
            Throw(x => x.Capacity,"InvalidInput");
            Throw(x => x.Capacity,"BadNumber");
        }
    }
}

