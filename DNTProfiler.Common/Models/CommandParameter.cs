namespace DNTProfiler.Common.Models
{
    public class CommandParameter
    {
        public string Name { set; get; }

        public string Value { set; get; }

        public string Type { set; get; }

        public string Direction { set; get; }

        public bool IsNullable { set; get; }

        public int Size { set; get; }

        public byte Precision { set; get; }

        public byte Scale { set; get; }

        public override string ToString()
        {
            return Name + ":" + Value;
        }
    }
}