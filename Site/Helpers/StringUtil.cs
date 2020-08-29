using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace site.Helpers
{
    public static class StringUtil
    {

        public static string DefaultTo(this string input, string @default)
        {
            if (IsEmpty(input) && @default != null) return @default;

            return input;
        }

        public static bool IsEmpty(this string input) => string.IsNullOrEmpty(input);

        internal static string SafeGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        internal static bool TryGetSafeImageExtension(string fileName, out string extension)
        {
            extension = "";
            var split = fileName.ToLower().Split('.');
            if (split.Length != 2) return false;

            extension = split[1];
            if (new[] { "jpg", "gif", "png" }.Contains(extension)) { 
                return true;
            }

            extension = "";
            return false;
        }
    }
}
