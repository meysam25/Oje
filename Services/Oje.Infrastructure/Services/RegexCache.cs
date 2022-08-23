using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Oje.Infrastructure.Services
{
    public static class RegexCache
    {
        static Dictionary<int, Regex> regexCache = new();

        public static Regex getRegexInstance(string reg)
        {
            if (regexCache == null)
                regexCache = new();

            var regHash = reg.GetHashCode32();

            if(regexCache.Keys.Any(t => t == regHash) && regexCache[regHash] != null)
                return regexCache[regHash];

            regexCache[regHash] = new Regex(reg, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regexCache[regHash];
        }

    }
}
