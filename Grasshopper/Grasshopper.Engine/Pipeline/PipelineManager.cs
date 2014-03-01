using System;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device1 = SharpDX.Direct3D11.Device1;
using Device2 = SharpDX.DXGI.Device2;
using Factory2 = SharpDX.DXGI.Factory2;
using Resource = SharpDX.Direct3D11.Resource;
using ResultCode = SharpDX.DXGI.ResultCode;

namespace Grasshopper.Engine.Pipeline
{
	public class PipelineManager : Component
	{
		private readonly GrasshopperApp _app;
		private readonly IntPtr _windowHandle;

		private SwapChain1 _swapChain;
		private RenderTargetView _renderTargetView;
		private DepthStencilView _depthStencilView;
		private Texture2D _backBuffer;
		private Texture2D _depthBuffer;
		private Bitmap1 _bitmapTarget;

		public event Action DeviceChanged;
		public event Action SizeChanged;

		public SwapChain1 SwapChain
		{
			get { return _swapChain; }
		}

		public RenderTargetView RenderTargetView
		{
			get { return _renderTargetView; }
		}

		public DepthStencilView DepthStencilView
		{
			get { return _depthStencilView; }
		}

		public Texture2D BackBuffer
		{
			get { return _backBuffer; }
		}

		public Texture2D DepthBuffer
		{
			get { return _depthBuffer; }
		}

		public Bitmap1 BitmapTarget
		{
			get { return _bitmapTarget; }
		}

		public ModeDescription[] DisplayModeList { get; private set; }
		public Rectangle RenderTargetBounds { get; private set; }
		public ViewportF Viewport { get; private set; }
		public bool VSync { get; set; }

		private Size2 RenderTargetSize
		{
			get { return new Size2(RenderTargetBounds.Width, RenderTargetBounds.Height); }
		}

		internal PipelineManager(GrasshopperApp app, IntPtr windowHandle)
		{
			_app = app;
			_windowHandle = windowHandle;

			DeviceChanged += () => CreateSizeDependentResources(true);
		}

		internal void CreateDeviceDependentResources()
		{
			if(_swapChain != null)
				RemoveAndDispose(ref _swapChain);

			DeviceChanged();
		}

		internal void CreateSizeDependentResources(bool wasTriggeredByDeviceChange)
		{
			var device = _app.DeviceManager.Direct3D.Device;
			var context = _app.DeviceManager.Direct3D.Context;
			var d2dContext = _app.DeviceManager.Direct2D.Context;

			RemoveAndDispose(ref _backBuffer);
			RemoveAndDispose(ref _renderTargetView);
			RemoveAndDispose(ref _depthStencilView);
			RemoveAndDispose(ref _depthBuffer);
			RemoveAndDispose(ref _bitmapTarget);
			d2dContext.Target = null;

			if(_swapChain != null)
			{
				var descr = _swapChain.Description;
				_swapChain.ResizeBuffers(_swapChain.Description1.BufferCount, _app.Width, _app.Height, descr.ModeDescription.Format, descr.Flags);
			}
			else
			{
				var desc = CreateSwapChainDescription();
				using(var dxgiDevice2 = device.QueryInterface<Device2>())
				using(var dxgiAdapter = dxgiDevice2.Adapter)
				using(var dxgiFactory2 = dxgiAdapter.GetParent<Factory2>())
				using(var output = dxgiAdapter.Outputs.First())
				{
					_swapChain = ToDispose(CreateSwapChain(dxgiFactory2, device, desc));
					DisplayModeList = output.GetDisplayModeList(desc.Format, DisplayModeEnumerationFlags.Scaling);
				}
			}

			_backBuffer = ToDispose(Resource.FromSwapChain<Texture2D>(_swapChain, 0));
			_renderTargetView = ToDispose(new RenderTargetView(device, BackBuffer));

			var backBufferDesc = BackBuffer.Description;
			RenderTargetBounds = new Rectangle(0, 0, backBufferDesc.Width, backBufferDesc.Height);
			Viewport = new ViewportF(RenderTargetBounds.X, RenderTargetBounds.Y, RenderTargetBounds.Width, RenderTargetBounds.Height, 0.0f, 1.0f);
			context.Rasterizer.SetViewport(Viewport);

			_depthBuffer = ToDispose(new Texture2D(device, new Texture2DDescription
			{
				Format = Format.D32_Float_S8X24_UInt,
				ArraySize = 1,
				MipLevels = 1,
				Width = RenderTargetSize.Width,
				Height = RenderTargetSize.Height,
				SampleDescription = SwapChain.Description.SampleDescription,
				BindFlags = BindFlags.DepthStencil,
			}));

			_depthStencilView = ToDispose(new DepthStencilView(device, DepthBuffer, new DepthStencilViewDescription
			{
				Dimension = SwapChain.Description.SampleDescription.Count > 1 || SwapChain.Description.SampleDescription.Quality > 0
					? DepthStencilViewDimension.Texture2DMultisampled
					: DepthStencilViewDimension.Texture2D
			}));

			context.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);

			var bitmapProperties = new BitmapProperties1(
				new PixelFormat(_swapChain.Description.ModeDescription.Format, AlphaMode.Premultiplied),
				_app.DeviceManager.Dpi, _app.DeviceManager.Dpi,
				BitmapOptions.Target | BitmapOptions.CannotDraw);
			using(var dxgiBackBuffer = _swapChain.GetBackBuffer<Surface>(0))
				_bitmapTarget = ToDispose(new Bitmap1(d2dContext, dxgiBackBuffer, bitmapProperties));

			d2dContext.Target = BitmapTarget;
			d2dContext.TextAntialiasMode = TextAntialiasMode.Grayscale;

			var handler = SizeChanged;
			if(handler != null)
				handler();
		}

		private SwapChainDescription1 CreateSwapChainDescription()
		{
			var desc = new SwapChainDescription1
			{
				Width = _app.Width,
				Height = _app.Height,
				Format = Format.B8G8R8A8_UNorm,
				Stereo = false,
				SampleDescription = new SampleDescription(1, 0),
				Usage = Usage.BackBuffer | Usage.RenderTargetOutput,
				BufferCount = 1,
				Scaling = Scaling.Stretch,
				SwapEffect = SwapEffect.Discard,
				Flags = SwapChainFlags.AllowModeSwitch
			};
			
			return desc;
		}

		private SwapChainFullScreenDescription CreateFullScreenSwapChainDescription()
		{
			return new SwapChainFullScreenDescription
			{
				RefreshRate = new Rational(60, 1),
				Scaling = DisplayModeScaling.Centered,
				Windowed = true
			};
		}

		private SwapChain1 CreateSwapChain(Factory2 factory, Device1 device, SwapChainDescription1 desc)
		{
			return new SwapChain1(factory, device, _windowHandle, ref desc, CreateFullScreenSwapChainDescription());
		}

		public void Present()
		{
			var parameters = new PresentParameters();
			try
			{
				_swapChain.Present((VSync ? 1 : 0), PresentFlags.None, parameters);
			}
			catch(SharpDXException ex)
			{
				if(ex.ResultCode == ResultCode.DeviceRemoved
					|| ex.ResultCode == ResultCode.DeviceReset)
					_app.DeviceManager.Initialize(_app.DeviceManager.Dpi);
				else
					throw;
			}
		}
	}
}
