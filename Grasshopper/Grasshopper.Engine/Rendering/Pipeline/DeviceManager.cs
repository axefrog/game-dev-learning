using System;

namespace Grasshopper.Engine.Rendering.Pipeline
{
	public class DeviceManager : IDisposable
	{
		private Direct3DDeviceManager _direct3D;
		private Direct2DDeviceManager _direct2D;
		private float _dpi;

		public void Initialize(float dpi = 96f)
		{
			_dpi = dpi;
			_direct3D = new Direct3DDeviceManager();
			_direct2D = new Direct2DDeviceManager(_direct3D);

			_direct3D.Initialize();
			_direct2D.Initialize();

			var handler = Initialized;
			if(handler != null)
				handler();
		}

		public Direct3DDeviceManager Direct3D
		{
			get { return _direct3D; }
		}

		public Direct2DDeviceManager Direct2D
		{
			get { return _direct2D; }
		}

		public float Dpi
		{
			get { return _dpi; }
		}

		public event Action Initialized;
		
		public void Dispose()
		{
			_direct2D.Dispose();
			_direct3D.Dispose();
		}
	}
}