using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Oje.Infrastructure.Services
{
    public static class RegexCache
    {
        static Dictionary<int, Regex> regexCache = new();

        public static Regex getRegexInstance(string reg, RegexOptions? rp = null, TimeSpan? ti = null)
        {
            if (regexCache == null)
                regexCache = new();

            if (rp == null)
                rp = RegexOptions.Compiled | RegexOptions.IgnoreCase;

            var regHash = reg.GetHashCode32();

            if (regexCache.Keys.Any(t => t == regHash) && regexCache[regHash] != null)
                return regexCache[regHash];

            if (ti == null)
                regexCache[regHash] = new Regex(reg, rp.Value);
            else
                regexCache[regHash] = new Regex(reg, rp.Value, ti.Value);

            return regexCache[regHash];
        }

    }
}
