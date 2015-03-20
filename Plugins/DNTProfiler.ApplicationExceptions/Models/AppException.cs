using System;

namespace DNTProfiler.ApplicationExceptions.Models
{
    public class AppException
    {
        public DateTime AtDateTime { set; get; }
        public string Message { set; get; }
        public string Details { set; get; }

        public AppException()
        {
            AtDateTime = DateTime.Now;
        }
    }
}