using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace Grasshopper.Engine.Rendering.Pipeline
{
	public class Direct3DDeviceManager : Component
	{
		private Device1 _device;
		private DeviceContext1 _context;

		public FeatureLevel[] FeatureLevels { get; set; }

		public Device1 Device
		{
			get { return _device; }
		}

		public DeviceContext1 Context
		{
			get { return _context; }
		}

		public Direct3DDeviceManager()
		{
			FeatureLevels = new[]
			{
				FeatureLevel.Level_11_1,
				FeatureLevel.Level_11_0,
			};
		}

		public void Initialize()
		{
			RemoveAndDispose(ref _device);
			RemoveAndDispose(ref _context);

			var creationFlags = DeviceCreationFlags.BgraSupport;
#if DEBUG
			creationFlags |= DeviceCreationFlags.Debug;
#endif
			using(var device = new Device(DriverType.Hardware, creationFlags, FeatureLevels))
				_device = ToDispose(device.QueryInterface<Device1>());
			_context = ToDispose(_device.ImmediateContext.QueryInterface<DeviceContext1>());
		}
	}
}