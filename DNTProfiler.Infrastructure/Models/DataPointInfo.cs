using OxyPlot;
using OxyPlot.Series;

namespace DNTProfiler.Infrastructure.Models
{
    public class DataPointInfo
    {
        public DataPointInfo() { }

        public DataPointInfo(DataPoint point, int index, LineSeries lineSeries)
        {
            Index = index;
            Point = point;
            LineSeries = lineSeries;
        }

        public int Index { set; get; }
        public LineSeries LineSeries { set; get; }
        public DataPoint Point { set; get; }
    }
}