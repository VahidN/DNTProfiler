using System;
using System.Collections.Generic;
using System.Linq;

namespace DNTProfiler.Common.Models
{
    public class CallingMethodStackTrace : StatisticsBase
    {
        public CallingMethodStackTrace()
        {
            CallingMethodInfoList = new List<CallingMethodInfo>();
        }

        public AppIdentity ApplicationIdentity { set; get; }

        public IList<CallingMethodInfo> CallingMethodInfoList { set; get; }

        public string StackTraceHash { set; get; }

        public override string ToString()
        {
            return CallingMethodInfoList.Select(x => x.StackTrace)
                                        .Aggregate((t1, t2) => t1 + Environment.NewLine + t2);
        }
    }
}