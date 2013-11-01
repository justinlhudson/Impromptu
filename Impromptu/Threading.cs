using System;
using System.Threading;
using System.Diagnostics;

namespace Impromptu
{
    public static class Threading
    {
        public static bool TryExecute(TimeSpan timeout, Action action, bool abort = true, bool surpressExceptions = true)
        {
            try
            {
                var thread = new Thread(() => action());
                thread.Start();
                var completed = thread.Join(timeout);
                if(!completed && abort)
                    thread.Abort();
                return completed;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
                if(!surpressExceptions)
                    throw;
            }
            return false;
        }
    }
}

