using System;
using System.Globalization;
using System.Text;

namespace Mirecad.Toolbox.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (char ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveWhiteSpaces(this string text)
            => System.Text.RegularExpressions.Regex.Replace(text, @"\s+", "");

        public static string RemoveSubstring(this string text, string substring)
            => text.Replace(substring, "");

        public static string GetSubstringBeforeFirst(this string text, char delimiter)
        {
            var charPosition = text.IndexOf(delimiter, StringComparison.Ordinal);
            return charPosition > 0 
                ? text.Substring(0, charPosition) 
                : text;
        }

        public static string Truncate(this string text, int maxLength)
        {
            if (text.Length > maxLength)
            {
                return text.Substring(0, maxLength);
            }
            return text;
        }
    }
}