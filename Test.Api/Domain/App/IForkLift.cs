using Test.Api.Domain.Security;

namespace Test.Api.Domain.App;


public interface IForkLift : IEntity
{

    string Brand { get; }
    string Number { get; }
    decimal Capacity { get; }
    bool IsActive { get; }
    DateTime ModifiedAt { get; }
    IUser ModifiedBy { get; }

    IReadOnlyCollection<IForkFault> Faults { get; }


}

