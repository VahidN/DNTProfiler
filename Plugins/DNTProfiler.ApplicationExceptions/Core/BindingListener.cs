using System;
using System.Diagnostics;
using System.Threading;

namespace DNTProfiler.ApplicationExceptions.Core
{
    public sealed class BindingListener : DefaultTraceListener
    {
        private static readonly Lazy<BindingListener> _instance =
                    new Lazy<BindingListener>(() => new BindingListener(), LazyThreadSafetyMode.ExecutionAndPublication);

        private bool _enabled;
        private Action<string> _onWpfBindingException;

        private BindingListener()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(this);
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            TraceOutputOptions = TraceOptions.Callstack| TraceOptions.LogicalOperationStack;
        }

        public static BindingListener Instance
        {
            get { return _instance.Value; }
        }

        public void Start(Action<string> onWpfBindingException)
        {
            _onWpfBindingException = onWpfBindingException;
            _enabled = true;
        }

        public void Stop()
        {
            _enabled = false;
        }

        public override void WriteLine(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || !_enabled)
                return;

            if (_onWpfBindingException != null)
                _onWpfBindingException(message);
        }
    }
}