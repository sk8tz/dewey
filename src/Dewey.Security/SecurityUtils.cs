using System;
using System.Text;

namespace Dewey.Security
{
    public static class SecurityUtils
    {
        public static string GeneratePassword(int length = 10, bool includeSpecialCharacters = true)
        {
            string chars;

            if (includeSpecialCharacters) {
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-.%$#@!*()_+?";
            } else {
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            }

            var res = new StringBuilder();
            var rnd = new Random();

            while (0 < length--) {
                res.Append(chars[rnd.Next(chars.Length)]);
            }

            return res.ToString();
        }
    }
}
