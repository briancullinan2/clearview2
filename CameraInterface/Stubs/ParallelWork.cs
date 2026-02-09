using System;
using System.Threading;

namespace ParallelWork
{
    public static class Start
    {
        public static Worker Work(Action action, ThreadPriority priority)
        {
            action?.Invoke();
            return new Worker();
        }
    }

    public class Worker
    {
        public Worker OnException(Action<Exception> handler) { return this; }
        public void RunNow() { }
    }
}
