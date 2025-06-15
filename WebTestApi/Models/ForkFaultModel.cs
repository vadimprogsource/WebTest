using System;
using Test.Api;
using Test.Api.Domain;

namespace TestWebApi.Models
{
    public record ForkFaultModel : IForkFault
    {
    

        public Guid Guid { get; set; }
        public DateTime? ProblemDetectedAt { get; set; }
        public DateTime? ProblemResolvedAt { get; set; }
        public string? Reason { get; set; }

        public TimeSpan Downtime { get;  }

        string IForkFault.Reason => Reason ?? string.Empty;

        DateTime IForkFault.ProblemDetectedAt => ProblemDetectedAt.HasValue ? ProblemDetectedAt.Value.ToUniversalTime() : DateTime.UtcNow;

        bool IIdentity.IsValid => true;

        DateTime IEntity.CreatedAt => DateTime.UtcNow;

        public ForkFaultModel() { }

        public ForkFaultModel(IForkFault source)
        {
            Guid = source.Guid;
            ProblemDetectedAt = source.ProblemDetectedAt.ToLocalTime();
            ProblemResolvedAt = source.ProblemResolvedAt.HasValue?source.ProblemResolvedAt.Value.ToLocalTime(): null;
            Reason = source.Reason;
            Downtime = source.Downtime;
        }

    }
}

