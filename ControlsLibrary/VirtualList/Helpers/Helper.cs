using System.Collections.Generic;
using System.Linq;

namespace VirtlistLib.Helpers
{
    public static class Helper
    {
        public static int ArrayNumGetHashCode(IReadOnlyList<int> list)
        {
            return list.Aggregate(list.Count, (current, item) => unchecked(current * 314159 + item));
        }
    }
}