using System.Collections.Generic;
using System.Linq;

namespace Mirecad.Toolbox.Extensions
{
    public static class EnumerableStringExtensions
    {
        public static bool ContainsIgnoreCare(this IEnumerable<string> collection, string target)
        {
            return collection.Any(x => x.ToLower() == target.ToLower());
        }
    }
}