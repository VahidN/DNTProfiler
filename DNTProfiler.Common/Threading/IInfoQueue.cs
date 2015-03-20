using System;

namespace DNTProfiler.Common.Threading
{
    public interface IInfoQueue<T> where T : class
    {
        Action<T> OnNext { set; get; }
        void Stop();
        void Enqueue(T value);
        bool IsEmpty { get; }
    }
}