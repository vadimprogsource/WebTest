using System;
namespace Test.Api.Domain;

public interface IForkLiftService
{
    Task<IForkLift> SetActiveAsync(int id, bool active);
}

