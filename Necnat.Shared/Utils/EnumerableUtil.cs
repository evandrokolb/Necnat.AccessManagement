using System.Collections.Generic;
using System.Linq;

namespace Necnat.Shared.Utils
{
    public static class EnumerableUtil
    {
        public static bool ScrambledEquals<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in enumerable1)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]++;
                else
                    cnt.Add(s, 1);
            }
            foreach (T s in enumerable2)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]--;
                else
                    return false;
            }

            return cnt.Values.All(c => c == 0);
        }

        public static bool ScrambledEquals<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2, IEqualityComparer<T> comparer)
        {
            var cnt = new Dictionary<T, int>(comparer);
            foreach (T s in enumerable1)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]++;
                else
                    cnt.Add(s, 1);
            }
            foreach (T s in enumerable2)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]--;
                else
                    return false;
            }

            return cnt.Values.All(c => c == 0);
        }
    }
}
