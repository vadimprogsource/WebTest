using System;
namespace Test.Api.Domain;

public interface IForkLiftService
{
    Task<IForkLift> SetActiveAsync(Guid guid, bool active);
}

