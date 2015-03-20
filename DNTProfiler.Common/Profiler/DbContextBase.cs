using System;

namespace DNTProfiler.Common.Profiler
{
    public class DbContextBase
    {
        public bool IsAsync { set; get; }
        public bool IsCanceled { set; get; }
        public Exception Exception { set; get; }
        public int? ObjectContextId { set; get; }
        public string ObjectContextName { set; get; }
        public int? ConnectionId { set; get; }
    }
}