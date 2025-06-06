using System;
namespace Test.Api.Domain;

public interface IForkFault : IIdentity
{
    DateTime ProblemDetectedAt { get;  }
    DateTime? ProblemResolvedAt { get; }
    string Reason { get;  }

    TimeSpan Downtime { get; }

}

