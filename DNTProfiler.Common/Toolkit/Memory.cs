using System;
using System.Diagnostics;

namespace DNTProfiler.Common.Toolkit
{
    /// <summary>
    /// محاسبه مجدد میزان مصرف حافظه برنامه
    /// </summary>
    public static class Memory
    {
        /// <summary>
        /// محاسبه مجدد میزان مصرف حافظه برنامه
        /// </summary>
        public static void ReEvaluateWorkingSet()
        {
            try
            {
                Process loProcess = Process.GetCurrentProcess();
                //it doesn't matter what you set maxWorkingSet to  
                //setting it to any value apparently causes the working set to be re-evaluated and excess discarded  
                loProcess.MaxWorkingSet = (IntPtr)((int)loProcess.MaxWorkingSet + 1);
            }
            catch
            {
                //The above code requires Admin privileges.   
                //So it's important to trap exceptions in case you're running without admin rights.   
            }
        }
    }
}