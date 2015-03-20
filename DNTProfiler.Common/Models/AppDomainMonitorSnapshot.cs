using System;

namespace DNTProfiler.Common.Models
{
    public class AppDomainMonitorSnapshot
    {
        public AppDomainMonitorSnapshot()
        {
            TotalProcessorTime = (long)AppDomain.CurrentDomain.MonitoringTotalProcessorTime.TotalMilliseconds;
            TotalAllocatedMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
        }

        public long TotalAllocatedMemorySize { get; set; }

        public long TotalProcessorTime { get; set; }
    }
}