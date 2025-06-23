using System;
using Test.Api.Domain;

namespace Tester
{
    public static class Date
    {
        public static DateTime ToUTC(this DateTime time) => time.ToUniversalTime();
        public static DateTime ToLT(this DateTime time) => time.ToLocalTime();

        public static DateTime toDateTime(this IForkLift @this) => @this.ModifiedAt.ToLocalTime();
    }
}

