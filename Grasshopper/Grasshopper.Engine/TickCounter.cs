using System.Diagnostics;

namespace Grasshopper.Engine
{
	public class TickCounter
	{
		const int MaxSamples = 100;
		int tickIndex = 0;
		long tickSum = 0;
		readonly long[] tickList = new long[MaxSamples];
		readonly Stopwatch clock;
		long frameCount;

		public TickCounter()
		{
			clock = Stopwatch.StartNew();
		}

		double CalcAverageTick(long newtick)
		{
			tickSum -= tickList[tickIndex];  /* subtract value falling off */
			tickSum += newtick;              /* add new value */
			tickList[tickIndex] = newtick;   /* save new value so it can be subtracted later */
			if(++tickIndex == MaxSamples)    /* inc buffer index */
				tickIndex = 0;

			if(frameCount < MaxSamples)
				return (double)tickSum / frameCount;
			return (double)tickSum / MaxSamples;
		}

		public void Tick()
		{
			frameCount++;
			var averageTick = CalcAverageTick(clock.ElapsedTicks) / Stopwatch.Frequency;
			FramesPerSecond = 1d / averageTick;
			AverageTickLength = averageTick * 1000d;
			clock.Reset();
		}

		public double FramesPerSecond { get; private set; }
		public double AverageTickLength { get; private set; }
	}
}