using System;
using System.Text;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string Clear(this string text, params string[] stringsToRemove)
        {
            var stringBuilder = new StringBuilder(text);

            foreach (var str in stringsToRemove) stringBuilder = stringBuilder.Replace(str, "");

            return stringBuilder.ToString();
        }

        public static string ToBase64(this byte[] buffer) => Convert.ToBase64String(buffer);

        public static byte[] FromBase64(this string base64) => Convert.FromBase64String(base64);
    }
}
