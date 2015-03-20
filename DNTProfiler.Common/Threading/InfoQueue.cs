using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DNTProfiler.Common.Threading
{
    public class InfoQueue<T> : IInfoQueue<T> where T : class
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly ManualResetEvent _syncEvent = new ManualResetEvent(false);
        private bool _isRunning = true;
        private Thread _runningThread;

        public InfoQueue()
        {
            startThread();
        }

        public bool IsEmpty
        {
            get { return _queue.IsEmpty; }
        }

        public Action<T> OnNext { set; get; }

        public void Enqueue(T value)
        {
            _queue.Enqueue(value);
            _syncEvent.Set();
        }

        public void Stop()
        {
            _isRunning = false;
            _syncEvent.Set();
        }

        private void runMethod()
        {
            while (_isRunning && _syncEvent.WaitOne())
            {
                T result;
                if (_queue.TryDequeue(out result))
                {
                    OnNext(result);
                }
                else
                {
                    _syncEvent.Reset();
                }
            }
        }

        private void startThread()
        {
            _runningThread = new Thread(runMethod)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            _runningThread.Start();
        }
    }
}