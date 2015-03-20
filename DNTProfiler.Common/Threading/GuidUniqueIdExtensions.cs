using System;
using System.Collections.Generic;
using System.Threading;

namespace DNTProfiler.Common.Threading
{
    public static class GuidUniqueIdExtensions
    {
        static readonly Dictionary<Guid, int> _idTable =
            new Dictionary<Guid, int>();

        private static int _uniqueId;
        private static readonly object _lockObject = new object();

        public static int GetUniqueId(this Guid? guidKey)
        {
            lock (_lockObject)
            {
                if (guidKey == null)
                    return 0;

                int value;
                if (_idTable.TryGetValue(guidKey.Value, out value))
                {
                    return value;
                }

                value = Interlocked.Increment(ref _uniqueId);
                _idTable.Add(guidKey.Value, value);
                return value;
            }
        }
    }
}