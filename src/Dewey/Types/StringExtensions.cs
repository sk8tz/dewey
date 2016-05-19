using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dewey.Types
{
    public static class StringExtensions
    {
        public static string Chomp(this string value, int len)
        {
            if (value == null) {
                return value;
            }
            
            if (value.Length <= len) {
                return value;
            }

            return value.Substring(0, value.Length - len);
        }
        public static Guid? ToNullableGuid(this string guid) => (guid.IsEmpty()) ? null : (Guid?)(new Guid(guid));

        public static Guid ToGuid(this string guid)
        {
            if (guid.IsEmpty()) {
                throw new ArgumentNullException(nameof(guid));
            }

            return new Guid(guid);
        }

        public static string Digits(this string value) => new string(value?.Where(c => c >= '0' && c <= '9').ToArray());

        public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

        public static bool IsNotEmpty(this string value) => !value.IsEmpty();

        public static T Default<T>(this T value, T defaultValue = default(T)) => (value.IsBlank()) ? defaultValue : value;

        public static string Between(this string value, char from, char to)
        {
            var result = string.Empty;
            bool read = false;
            bool stopped = false;

            foreach (var c in value.ToArray()) {
                if ((from != to && !read && c == to) || (read && c == to)) {
                    stopped = true;
                    break;
                }

                if (read) {
                    result += c;
                }

                if (c == from) {
                    read = true;
                }
            }

            // from and to were found
            if (read && stopped) {
                return result;
            }

            return string.Empty;
        }

        public static string ToPascal(this string value)
        {
            // Probably a constant value
            if (value.Contains("_") || value.IsAllUpper()) {
                value = value.ToLower();
                string[] parts = value.Split('_');
                var result = string.Empty;

                foreach (var part in parts) {
                    result += part.ToPascal();
                }

                return result;
            }

            return value.Capitalize();
        }

        public static bool IsAllUpper(this string value)
        {
            for (int i = 0; i < value.Length; i++) {
                if (char.IsLetter(value[i]) && !char.IsUpper(value[i]))
                    return false;
            }

            return true;
        }

        public static string Capitalize(this string value) => char.ToUpper(value[0]) + value.Substring(1);

        public static string AsDisplayName(this string value)
        {
            var result = string.Empty;

            foreach (string s in value.SplitOnUpper()) {
                result += s + " ";
            }

            return result.Trim();
        }

        public static string[] SplitOnUpper(this string value)
        {
            var result = new List<string>();
            var s = string.Empty;

            foreach (var c in value.ToList()) {
                if (char.IsLetter(c) && char.IsUpper(c)) {
                    if (s.IsNotEmpty()) {
                        result.Add(s);
                    }

                    s = string.Empty;
                }

                s += c;
            }

            result.Add(s);

            return result.ToArray();
        }

        public static string Minify(this string value)
        {
            var lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            var result = new StringBuilder();

            foreach (var line in lines) {
                var trimLine = line.Trim();

                result.Append(trimLine);
            }

            return result.ToString();
        }

        public static string ToMinifiedString(this StringBuilder value)
        {
            var valueText = value.ToString();

            var lines = valueText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            var result = new StringBuilder();

            foreach (var line in lines) {
                var trimLine = line.Trim();

                result.Append(trimLine);
            }

            return result.ToString();
        }
    }
}
