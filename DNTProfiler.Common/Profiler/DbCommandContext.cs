using System.Data;

namespace DNTProfiler.Common.Profiler
{
    public class DbCommandContext : DbContextBase
    {
        public DataTable DataTable { set; get; }
        public long? ElapsedMilliseconds { set; get; }
        public object Result { set; get; }
    }
}