namespace DNTProfiler.Common.Profiler
{
    public class DbTransactionContext : DbContextBase
    {
        public int? TransactionId { set; get; }
    }
}