using System;
using System.Threading;

namespace DNTProfiler.Common.Toolkit
{
    public class TimedRunnerResult<TR>
    {
        public TR Result { set; get; }
        public bool IsTimedOut { set; get; }
    }

    public static class TimedRunner
    {
        public static TimedRunnerResult<TR> RunWithTimeout<TR>(Func<TR> proc, int timeoutSeconds = 3)
        {
            return RunWithTimeout(proc, TimeSpan.FromSeconds(timeoutSeconds));
        }

        public static TimedRunnerResult<TR> RunWithTimeout<TR>(Func<TR> proc, TimeSpan timeout)
        {
            using (var waitHandle = new AutoResetEvent(false))
            {
                var ret = default(TR);
                var thread = new Thread(() =>
                {
                    ret = proc();
                    waitHandle.Set();
                }) { IsBackground = true };
                thread.Start();

                var timedOut = !waitHandle.WaitOne(timeout, false);
                waitHandle.Close();

                if (timedOut)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch { }
                    return new TimedRunnerResult<TR>
                    {
                        Result = default(TR),
                        IsTimedOut = true
                    };
                }

                return new TimedRunnerResult<TR>
                {
                    Result = ret,
                    IsTimedOut = false
                };
            }
        }
    }
}