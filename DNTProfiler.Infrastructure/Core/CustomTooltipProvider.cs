using System;
using OxyPlot;

namespace DNTProfiler.Infrastructure.Core
{
    public class CustomTooltipProvider
    {
        public Func<TrackerHitResult, string> GetCustomTooltip { set; get; }
    }
}