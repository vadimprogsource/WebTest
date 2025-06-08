using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain;

public class ForkFaultDataValidator : DataValidator<IForkFault>
{

    protected override void OnValidate(IForkFault entity)
    {
        if (entity.ProblemDetectedAt == DateTime.MinValue) Throw(x => x.ProblemDetectedAt);
        if (entity.ProblemResolvedAt.HasValue && entity.ProblemResolvedAt.Value<entity.ProblemDetectedAt) Throw(x => x.ProblemResolvedAt);
        if (string.IsNullOrWhiteSpace(entity.Reason)) Throw(x => x.Reason);
    }
}

