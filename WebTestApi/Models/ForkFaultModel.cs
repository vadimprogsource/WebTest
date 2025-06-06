using System;
using Test.Api;
using Test.Api.Domain;

namespace TestWebApi.Models
{
    public record ForkFaultModel : IForkFault
    {
    

        public int Id { get; set; }
        public DateTime ProblemDetectedAt { get; set; }
        public DateTime? ProblemResolvedAt { get; set; }
        public string? Reason { get; set; }

        public TimeSpan Downtime { get;  }

        DateTime IForkFault.ProblemDetectedAt => ProblemDetectedAt.ToUniversalTime();
        DateTime? IForkFault.ProblemResolvedAt => ProblemResolvedAt.HasValue?ProblemResolvedAt.Value.ToUniversalTime(): ProblemDetectedAt.AddHours(1);

        string IForkFault.Reason => Reason ?? string.Empty;

        public ForkFaultModel() { }

        public ForkFaultModel(IForkFault source)
        {
            Id = source.Id;
            ProblemDetectedAt = source.ProblemDetectedAt.ToLocalTime();
            ProblemResolvedAt = source.ProblemResolvedAt.HasValue?source.ProblemResolvedAt.Value.ToLocalTime(): ProblemDetectedAt.AddHours(1);
            Reason = source.Reason;
            Downtime = source.Downtime;
        }

    }
}

