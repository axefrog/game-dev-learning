using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;

namespace Grasshopper.Engine.Rendering
{
	static class BufferHelper
	{
		public static Buffer CreateBuffer<T>(Device device, ResourceUsage resourceUsage, BindFlags bindFlags, params T[] items)
			where T : struct
		{
			var len = items.Length * Marshal.SizeOf(typeof(T));

			Buffer buffer;
			DataStream stream;
			switch(resourceUsage)
			{
				case ResourceUsage.Dynamic:
					buffer = new Buffer(device, len, resourceUsage, bindFlags, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
					device.ImmediateContext.MapSubresource(buffer, MapMode.WriteDiscard, MapFlags.None, out stream);
					WriteToStream(stream, items);
					device.ImmediateContext.UnmapSubresource(buffer, 0);
					break;

				default:
					stream = new DataStream(len, true, true);
					WriteToStream(stream, items);
					buffer = new Buffer(device, stream, len, resourceUsage, bindFlags, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
					break;
			}
			//stream.Dispose();
			return buffer;
		}

		public static void WriteToStream<T>(DataStream stream, IEnumerable<T> items)
			where T : struct
		{
			foreach(var item in items)
				stream.Write(item);
			stream.Position = 0;
		}

		public static void UpdateDynamicBuffer<T>(Device device, Buffer buffer, IEnumerable<T> items)
			where T : struct
		{
			DataStream stream;
			device.ImmediateContext.MapSubresource(buffer, 0, MapMode.WriteDiscard, MapFlags.None, out stream);
			WriteToStream(stream, items);
			device.ImmediateContext.UnmapSubresource(buffer, 0);
			stream.Dispose();
		}
	}
}
