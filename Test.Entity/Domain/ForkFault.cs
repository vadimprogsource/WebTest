using Test.Api.Domain;

namespace Test.Entity.Domain
{
    public class ForkFault : EntityBase, IForkFault
    {

        public DateTime ProblemDetectedAt { get; set; }
        public DateTime? ProblemResolvedAt { get; set; }
        public string Reason { get; set; } = string.Empty;

        public TimeSpan Downtime => (ProblemResolvedAt ?? ProblemDetectedAt) - ProblemDetectedAt;

        public Guid ForkLiftGuid { get; set; }
        public ForkLift ForkLift { get; set; } = null!;
        public ForkFault() { ProblemDetectedAt = DateTime.UtcNow; }

        public ForkFault(IForkFault source) : base(source) => Update(source);

        public ForkFault Update(IForkFault source)
        {
            ProblemDetectedAt = source.ProblemDetectedAt;
            ProblemResolvedAt = source.ProblemResolvedAt;
            Reason = source.Reason;
            return this;
        }
    }
}

