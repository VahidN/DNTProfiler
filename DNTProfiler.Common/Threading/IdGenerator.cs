using System.Threading;

namespace DNTProfiler.Common.Threading
{
    public static class IdGenerator
    {
        private static int _id;

        public static int GetId()
        {
            return Interlocked.Increment(ref _id);
        }

        public static void Reset()
        {
            _id = 0;
        }
    }
}