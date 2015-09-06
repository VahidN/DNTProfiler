namespace DNTProfiler.Common.Models
{
    public class ColumnInfo
    {
        public string ColumnName { set; get; }

        public string DataType { set; get; }

        public int Ordinal { set; get; }

        public bool IsKey { set; get; }
    }
}