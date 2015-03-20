using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace DNTProfiler.Common.Mvvm
{
    public class AppMessenger
    {
        readonly static Messenger _messenger = new Messenger();
        private static int? _appDomainId;
        private static string _appDomainName;
        private static int? _processId;
        private static string _processName;

        public static int AppDomainId
        {
            get
            {
                if (_appDomainId == null)
                    _appDomainId = AppDomain.CurrentDomain.Id;
                return _appDomainId.Value;
            }
        }

        public static string AppDomainName
        {
            get { return _appDomainName ?? (_appDomainName = AppDomain.CurrentDomain.FriendlyName); }
        }

        public static string ExecutablePathDir
        {
            get { return System.IO.Path.GetDirectoryName(Application.ExecutablePath); }
        }

        public static string LogFile
        {
            get { return System.IO.Path.Combine(ExecutablePathDir, "ErrorsLog.Log"); }
        }

        public static Messenger Messenger
        {
            get { return _messenger; }
        }

        public static string Path
        {
            get { return Application.StartupPath; }
        }

        public static int ProcessId
        {
            get
            {
                if (_processId == null)
                    _processId = Process.GetCurrentProcess().Id;
                return _processId.Value;
            }
        }

        public static string ProcessName
        {
            get { return _processName ?? (_processName = Process.GetCurrentProcess().ProcessName); }
        }

        public static string AppVersion
        {
            get { return string.Format("DNT Profiler v{0}", Assembly.GetExecutingAssembly().GetName().Version); }
        }
    }
}