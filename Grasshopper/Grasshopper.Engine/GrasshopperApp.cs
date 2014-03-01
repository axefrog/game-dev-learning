using System;
using System.Windows.Forms;
using Grasshopper.Engine.Pipeline;
using Grasshopper.Engine.Rendering.UserInterface;
using SharpDX;

namespace Grasshopper.Engine
{
	public abstract class GrasshopperApp : Component
	{
		private readonly Form _form;
		private readonly DeviceManager _deviceManager;
		private readonly PipelineManager _pipeline;
		private readonly DebugInfoPanel _debug = new DebugInfoPanel();

		public event Action<GrasshopperApp> SizeChanged;

		public DeviceManager DeviceManager
		{
			get { return _deviceManager; }
		}

		public PipelineManager Pipeline
		{
			get { return _pipeline; }
		}

		public int Width
		{
			get { return (int)(_form.ClientRectangle.Width * DeviceManager.Dpi / 96.0); }
		}

		public int Height
		{
			get { return (int)(_form.ClientRectangle.Height * DeviceManager.Dpi / 96.0); }
		}

		public DebugInfoPanel Debug
		{
			get { return _debug; }
		}

		protected GrasshopperApp(Form form)
		{
			_form = form;
			_deviceManager = ToDispose(new DeviceManager());
			_pipeline = ToDispose(new PipelineManager(this, _form.Handle));

			_deviceManager.Initialized += () => Pipeline.CreateDeviceDependentResources();
			_form.SizeChanged += OnFormSizeChanged;
		}

		public void Initalize()
		{
			_deviceManager.Initialize();
			Debug.Initialize(this);
		}

		private void OnFormSizeChanged(object sender, EventArgs eventArgs)
		{
			Pipeline.CreateSizeDependentResources(false);
			
			var handler = SizeChanged;
			if(handler != null)
				handler(this);
		}
	}
}
