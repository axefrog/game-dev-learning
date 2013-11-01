using System.Collections;
using System.Collections.Generic;
using System.IO;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDx4.Direct3D
{
	static class Helpers
	{
		public static Buffer CreateBuffer<T>(Device device, ResourceUsage resourceUsage, BindFlags bindFlags, params T[] items)
			where T : struct
		{
			var len = Utilities.SizeOf(items);

			Buffer buffer;
			DataStream stream;
			switch (resourceUsage)
			{
				case ResourceUsage.Dynamic:
					buffer = new Buffer(device, len, resourceUsage, bindFlags, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
					device.ImmediateContext.MapSubresource(buffer, MapMode.WriteDiscard, MapFlags.None, out stream);
					WriteToStream(stream, items);
					break;

				default:
					stream = new DataStream(len, true, true);
					WriteToStream(stream, items);
					buffer = new Buffer(device, stream, len, resourceUsage, bindFlags, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
					break;
			}
			return buffer;
		}

		public static void WriteToStream<T>(DataStream stream, IEnumerable<T> items)
			where T : struct
		{
			foreach (var item in items)
				stream.Write(item);
			stream.Position = 0;
		}

		public static void UpdateDynamicBuffer<T>(Device device, Buffer buffer, IEnumerable<T> items)
			where T : struct
		{
			DataStream stream;
			device.ImmediateContext.MapSubresource(buffer, 0, MapMode.WriteDiscard, MapFlags.None, out stream);
			WriteToStream(stream, items);
		}
	}
}
