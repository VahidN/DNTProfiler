namespace DNTProfiler.Common.Models
{
    public class CallingMethodInfo
    {
        public int CallingCol { set; get; }

        public string CallingFile { set; get; }

        public string CallingFileFullName { set; get; }

        public int CallingLine { set; get; }

        public string CallingMethod { set; get; }

        public string StackTrace { set; get; }

        public override string ToString()
        {
            return StackTrace;
        }
    }
}