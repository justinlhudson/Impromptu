using System;
using System.Threading;

namespace Impromptu
{
    public static class Threading
    {
        public static bool TryExecute(TimeSpan timeout, Action action)
        {
            var thread = new Thread(() => action());
            thread.Start();
            var completed = thread.Join(timeout);
            if(!completed)
                thread.Abort();
            return completed;
        }
    }
}

