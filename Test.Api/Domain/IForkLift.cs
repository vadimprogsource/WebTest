using System;
namespace Test.Api.Domain;


public interface IForkLift : IIdentity
{
 
    string Brand { get; }
    string Number { get;  }
    decimal Capacity { get;  }
    bool IsActive { get;  }
    DateTime ModifiedAt { get; }
    IUser ModifiedBy { get;  }

    IReadOnlyCollection<IForkFault> Faults { get; }

 
}

