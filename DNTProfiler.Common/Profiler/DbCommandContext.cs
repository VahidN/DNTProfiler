using System.Collections.Generic;
using System.Data;

namespace DNTProfiler.Common.Profiler
{
    public class DbCommandContext : DbContextBase
    {
        public DataTable DataTable { set; get; }

        public long? ElapsedMilliseconds { set; get; }

        public object Result { set; get; }

        public ISet<string> Keys { set; get; }

        public DbCommandContext()
        {
            Keys = new HashSet<string>();
        }
    }
}