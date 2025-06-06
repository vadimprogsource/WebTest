using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain;

public class ForkFaultDataValidator : IDataValidator<IForkFault>
{

    public bool Validate(IForkFault entity)=>!(entity.ProblemDetectedAt == DateTime.MinValue || string.IsNullOrWhiteSpace(entity.Reason));
    
}

