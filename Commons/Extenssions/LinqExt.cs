using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Extenssions
{
    public static class LinqExt
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

    }
}
