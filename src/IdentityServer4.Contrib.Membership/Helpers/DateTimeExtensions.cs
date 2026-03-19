namespace IdentityServer4.Contrib.Membership.Helpers
{
    using System;
    using Duende.IdentityModel;

    public static class DateTimeExtensions
    {
        public static DateTime ToUtc(this DateTime localDateTime)
        {
            return DateTime.SpecifyKind(localDateTime, DateTimeKind.Utc);
        }

        public static long ToUtcEpoch(this DateTime localDateTime)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(localDateTime.ToUtc());
            return dateTimeOffset.ToUnixTimeSeconds();
        }
    }
}
