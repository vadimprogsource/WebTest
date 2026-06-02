namespace Test.Api.Domain.App;

public interface IForkLiftService
{
    Task<IForkLift> SetActiveAsync(Guid guid, bool active);
}

