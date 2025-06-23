using System;
namespace TestWebApi.Services
{
    public static class DateTimeExtension
    {

        public static DateTime ToLocal(this DateTime value  ) => value.ToLocalTime();
        public static DateTime? ToLocal(this DateTime? value) => value.HasValue ? value.Value.ToLocalTime() : null;
    }
}

