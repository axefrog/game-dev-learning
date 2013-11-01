using SharpDX;
using SharpDX.Direct3D11;

namespace SharpDx4.Direct3D
{
	static class Helpers
	{
		public static Buffer CreateBuffer<T>(Device device, BindFlags bindFlags, params T[] items)
			where T : struct
		{
			var len = Utilities.SizeOf(items);
			var stream = new DataStream(len, true, true);
			foreach (var item in items)
				stream.Write(item);
			stream.Position = 0;
			var buffer = new Buffer(device, stream, len, ResourceUsage.Default,
				bindFlags, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
			return buffer;
		}
	}
}
