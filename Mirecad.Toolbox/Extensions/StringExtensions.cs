using System.Globalization;
using System.Text;

namespace Mirecad.Toolbox.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string input)
        {
            var normalized = input.Normalize(NormalizationForm.FormD);
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

        public static string RemoveWhiteSpaces(this string input)
            => System.Text.RegularExpressions.Regex.Replace(input, @"\s+", "");
    }
}