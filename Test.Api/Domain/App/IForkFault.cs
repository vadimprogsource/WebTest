namespace Test.Api.Domain.App;

public interface IForkFault : IEntity
{
    DateTime ProblemDetectedAt { get; }
    DateTime? ProblemResolvedAt { get; }
    string Reason { get; }

    TimeSpan Downtime { get; }

}

