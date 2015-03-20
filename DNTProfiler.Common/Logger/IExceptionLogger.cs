using System;

namespace DNTProfiler.Common.Logger
{
    public interface IExceptionLogger
    {
        string GetDetailedException(Exception exception);
        void LogExceptionToFile(object exception, string fileName);
        string GetExceptionMessageStack(Exception e);
    }
}