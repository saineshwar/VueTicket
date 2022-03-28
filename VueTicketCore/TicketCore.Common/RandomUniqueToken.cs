using System;

namespace TicketCore.Common
{
    public static class RandomUniqueToken
    {
        public static string Value()
        {
            var guid1 = Guid.NewGuid().ToString("N").Substring(0, 20).ToUpper();
            var unique = $"{guid1}";
            return unique;
        }
    }
}
