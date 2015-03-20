using System;
using System.Collections.Generic;

namespace DNTProfiler.Common.Toolkit
{
    public static class LinqExt
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}