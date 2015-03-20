using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DNTProfiler.Common.Threading
{
    public static class UniqueIdExtensions<T> where T : class
    {
        static readonly ConditionalWeakTable<T, string> _idTable =
                                    new ConditionalWeakTable<T, string>();

        // A static field is shared across all instances of the `same` type or T here.
        // This behavior is useful to produce unique auto increment Id's per each different object reference.
        private static int _uniqueId;

        public static string GetUniqueId(T obj)
        {
            return _idTable.GetValue(obj, o => Interlocked.Increment(ref _uniqueId).ToString(CultureInfo.InvariantCulture));
        }

        public static string GetUniqueId(T obj, string key)
        {
            return _idTable.GetValue(obj, o => key);
        }
    }
}