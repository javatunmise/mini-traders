using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Helpers
{
    public static class AmountUtil
    {
        public static decimal ToMinor(this decimal amount) => Math.Round(amount, 2) * 100;

        internal static decimal ToMinor(string amount)
        {
            throw new NotImplementedException();
        }

        public static int ToPercentage(decimal old, decimal newValue)
        {
            if (old == 0) return 0;
            var ratio = (old - newValue) / old;
            return Convert.ToInt32(ratio * 100);
        }
    }
}
