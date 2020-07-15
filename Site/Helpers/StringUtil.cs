using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Helpers
{
    public static class StringUtil
    {

        public static string DefaultTo(this string input, string @default)
        {
            if (string.IsNullOrEmpty(input) && @default != null) return @default;

            return input;
        }
    }
}
