using System;
using System.Linq;

namespace Mirecad.Toolbox.Regex
{
    public static class RegexTools
    {
        public static string GetSingleRegexGroupMatch(string source, System.Text.RegularExpressions.Regex regex,
            string groupName)
        {
            var regexDefinesGroup = regex.GetGroupNames().Contains(groupName);
            if(!regexDefinesGroup)
            {
                throw new ArgumentException($"Regex does not contain group '{groupName}'");
            }
            var matches = regex.Matches(source);
            var matchesCount = matches.Count;
            return matchesCount switch
            {
                0 => throw new InvalidOperationException("No regex match found."),
                1 => matches.Single().Groups[groupName].Value,
                _ => throw new InvalidOperationException("More than one regex match found.")
            };
        }
    }
}