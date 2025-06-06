using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain
{
    public class ForkLiftDataValidator : IDataValidator<IForkLift>
    {
       
        public bool Validate(IForkLift entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Number) || string.IsNullOrWhiteSpace(entity.Brand) || entity.Capacity == 0)
            {
                return false;
            }

            return true;
        }
    }
}

