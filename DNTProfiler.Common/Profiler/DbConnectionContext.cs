using System.Data;

namespace DNTProfiler.Common.Profiler
{
    public class DbConnectionContext : DbContextBase
    {
        public int? TransactionId { set; get; }
        public IsolationLevel IsolationLevel { set; get; }
        public string ConnectionString { set; get; }
    }
}