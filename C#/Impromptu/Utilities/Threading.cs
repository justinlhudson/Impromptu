using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Configuration;

namespace Impromptu.Utilities
{
	public static class Threading
	{
		public static ParallelOptions ParallelOptionsDefault(int max = 0)  //-1 means let system decide, 0 by # processors, > 0 manual set
		{
			if (max >= 0)
			{
				var maxDegreeOfParallelism = (int)Convert.ToInt16((Environment.ProcessorCount > 1 ? Environment.ProcessorCount : 1));

				var overrideMaxDegreeOfParallelism = ConfigurationManager.AppSettings["MaxDegreeOfParallelism"];
				if (!string.IsNullOrEmpty(overrideMaxDegreeOfParallelism))
					maxDegreeOfParallelism = (int)Convert.ToInt64(overrideMaxDegreeOfParallelism);

				if (max > 0)
					maxDegreeOfParallelism = max;

				return new ParallelOptions()
				{ MaxDegreeOfParallelism = maxDegreeOfParallelism };
			}
			return new ParallelOptions();
		}

		public static bool TryExecute(TimeSpan timeout, Action action, bool abort = true, bool surpressExceptions = false)
		{
			try
			{
				var thread = new Thread(() => action());
				thread.Start();
				var completed = thread.Join(timeout);
				if (!completed && abort)
					thread.Abort();
				return completed;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
				if (!surpressExceptions)
					throw;
			}
			return false;
		}
	}
}

