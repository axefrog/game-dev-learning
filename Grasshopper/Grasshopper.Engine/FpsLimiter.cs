﻿using System;
using System.Diagnostics;

namespace Grasshopper.Engine
{
	public class FpsLimiter
	{
		private readonly long _minWaitTime;
		private readonly Stopwatch _timer = Stopwatch.StartNew();

		// This holds the next tick count at which CanRun returns true. We always increment this by the exact desired
		// single-frame duration to ensure we track the frame count limit as accurately as possible and to prevent us
		// slowly getting out of sync and thus not maintaining the desired frame rate.
		private long _next;

		// Some scenarios may cause temporary performance issues and may cause CanRun to not be called for a long
		// period of time, which means that when performance is restored, the game would appear to run in fast forward
		// until _next catches up to the current elapsed time. _threshold is maintained exactly one additional frame
		// worth of ticks ahead of _next and if has already been exceeded when CanRun is checked, we synchronize _next
		// with the current elapsed time to ensure frame rate stays consistent.
		private long _threshold;

		public FpsLimiter(double maxFps)
		{
			_minWaitTime = Convert.ToInt64(TimeSpan.TicksPerSecond / maxFps);
			_next = 0;
			_threshold = 0;
		}

		public bool CanRun
		{
			get
			{
				if(_timer.Elapsed.Ticks < _next)
					return false;
				if(_timer.Elapsed.Ticks > _threshold)
					_next = _timer.Elapsed.Ticks + _minWaitTime;
				else
					_next += _minWaitTime;
				_threshold = _next + _minWaitTime;
				return true;
			}
		}
	}
}
