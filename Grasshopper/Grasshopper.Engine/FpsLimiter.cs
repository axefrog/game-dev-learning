using System;
using System.Diagnostics;

namespace Grasshopper.Engine
{
	public class FpsLimiter
	{
		private readonly long _minWaitTime;
		private readonly Stopwatch _timer = Stopwatch.StartNew();
		private long _next;

		public FpsLimiter(int maxFps)
		{
			_minWaitTime = TimeSpan.TicksPerSecond / maxFps;
			_next = 0;
		}

		public bool CanRun
		{
			get
			{
				if(_timer.Elapsed.Ticks < _next)
					return false;
				_next += _minWaitTime;
				return true;
			}
		}
	}
}
