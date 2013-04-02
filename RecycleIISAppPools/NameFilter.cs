using System;
using System.Collections.Generic;
using System.Linq;

namespace RecycleIISAppPools
{
    public class NameFilter
    {
        public static IEnumerable<string> Filter(List<string> filters, List<string> names)
        {
            return names.Where(n => HasMatch(n, filters));
        }


        private static bool HasMatch(string appPoolName, IEnumerable<string> filters)
        {
            foreach (var n in filters)
            {
                var nWithoutStar = n.Replace("*", "");

                if (n.StartsWith("*") && n.EndsWith("*"))
                {
                    if (appPoolName.IndexOf(nWithoutStar, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                    //no hit to next name
                    continue;
                }

                if (n.StartsWith("*"))
                {
                    if (appPoolName.EndsWith(nWithoutStar, StringComparison.OrdinalIgnoreCase))
                        return true;
                    //no hit to next name
                    continue;
                }

                if (n.EndsWith("*"))
                {
                    if (appPoolName.StartsWith(nWithoutStar, StringComparison.OrdinalIgnoreCase))
                        return true;
                    //no hit to next name
                    continue;
                }

                if (appPoolName.Equals(nWithoutStar, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

    }
}
