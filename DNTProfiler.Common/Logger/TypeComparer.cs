using System;
using System.Collections.Generic;

namespace DNTProfiler.Common.Logger
{
    public class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            if (x != null && y != null)
                return String.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
            if (x != null)
                return String.Compare(x.FullName, null, StringComparison.Ordinal);
            if (y != null)
                return String.Compare(y.FullName, null, StringComparison.Ordinal);
            return 0;
        }
    }
}