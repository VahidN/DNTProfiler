using System;
using System.Windows;
using System.Windows.Threading;

namespace DNTProfiler.Common.Threading
{
    public static class DispatcherHelper
    {
        public static void DispatchAction(Action action,
            DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
        {
            var dispatcher = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

            if (action == null || dispatcher == null)
                return;

            dispatcher.Invoke(dispatcherPriority, action);
        }

        public static Dispatcher GetDispatcher()
        {
            return Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        }

        public static int GetManagedThreadId()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(dispatcherFrame =>
                {
                    ((DispatcherFrame)dispatcherFrame).Continue = false;
                    return null;
                }), frame);
            Dispatcher.PushFrame(frame);
        }
    }
}