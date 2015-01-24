using System;
using System.Text;

namespace OpenTable.Features.Core.Helpers
{
    public static class RandomStringHelper
    {
        public static string CreateRandomString()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var builder = new StringBuilder();
            for (var i = 0; i < 14; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
