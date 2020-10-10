using Microsoft.EntityFrameworkCore.Internal;
using Shared;
using Shared.Entities;
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
            var lastDot = fileName.LastIndexOf('.');
            extension = lastDot > 0 ? fileName.Substring(lastDot + 1).ToLower() : "";

            if(extension == "") return false;

            if (new[] { "jpg", "gif", "png", "jpeg" }.Contains(extension)) { 
                return true;
            }

            extension = "";
            return false;
        }

        public static string GenerateReferralCode(string username)
        {
            var _username = username.Split('@')[0];
            var code = _username.Substring(0, Math.Min(_username.Length, 5));
            return $"{code}{Guid.NewGuid()}".Substring(0,10).ToUpper();
        }

        internal static string GenerateWalletAccountId(int storeId, int userId)
        {
            //var numbers = "0123456789";
            //var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //var rN = new Random().Next(0, 9);
            //var rA = new Random().Next(0, 25);

            return $"WLT{storeId}{userId}{DateTime.Now:yyyyMMddHHmmssffff}";
        }

        internal static string GenerateTokenAccountId(int storeId, int userId)
        {
            //var numbers = "0123456789";
            //var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //var rN = new Random().Next(0, 9);
            //var rA = new Random().Next(0, 25);

            return $"TK{storeId}{userId}{DateTime.Now:yyyyMMddHHmmssffff}";
        }
    }
}
