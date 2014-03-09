using SharpDX;
using SharpDX.Direct2D1;
using Factory = SharpDX.DirectWrite.Factory;

namespace Grasshopper.Engine.Rendering.Pipeline
{
	public class Direct2DDeviceManager : Component
	{
		private readonly Direct3DDeviceManager _d3DDeviceManager;
		private Factory1 _factory;
		private Device _device;
		private DeviceContext _context;

		public Factory DirectWriteFactory { get; private set; }

		public Factory1 Factory
		{
			get { return _factory; }
		}

		public Device Device
		{
			get { return _device; }
		}

		public DeviceContext Context
		{
			get { return _context; }
		}

		public Direct2DDeviceManager(Direct3DDeviceManager d3dDeviceManager)
		{
			_d3DDeviceManager = d3dDeviceManager;
		}

		public void Initialize()
		{
			RemoveAndDispose(ref _factory);
			RemoveAndDispose(ref _device);
			RemoveAndDispose(ref _context);

#if DEBUG
			const DebugLevel debugLevel = DebugLevel.Information;
#else
            const DebugLevel debugLevel = DebugLevel.None;
#endif
			_factory = ToDispose(new Factory1(FactoryType.SingleThreaded, debugLevel));
			using(var dxgiDevice = _d3DDeviceManager.Device.QueryInterface<SharpDX.DXGI.Device>())
				_device = ToDispose(new Device(_factory, dxgiDevice));
			_context = ToDispose(new DeviceContext(_device, DeviceContextOptions.None));

			DirectWriteFactory = ToDispose(new Factory(SharpDX.DirectWrite.FactoryType.Shared));
		}
	}
}