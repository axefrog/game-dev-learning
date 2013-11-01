﻿using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;

using Device11 = SharpDX.Direct3D11.Device;

namespace SharpDx4.Direct3D
{
	public class RendererDevice : IDisposable
	{
		private SwapChain _swapChain;
		private Device11 _device;
		private RenderTargetView _backBufferView;
		private DepthStencilView _zBufferView;
		private DeviceContext _deviceContext;
		
		private RasterizerState _rasterState;

		public Device11 Device
		{
			get { return _device; }
		}

		public SwapChain SwapChain
		{
			get { return _swapChain; }
		}

		public DeviceContext DeviceContext
		{
			get { return _deviceContext; }
		}

		public RenderTargetView BackBufferView
		{
			get { return _backBufferView; }
		}

		public DepthStencilView ZBufferView
		{
			get { return _zBufferView; }
		}

		public RenderForm Form { get; private set; }
		public bool AwaitingResize { get; private set; }

		public RendererDevice(RenderForm form, bool debug = false)
		{
			Form = form;

			var scdesc = new SwapChainDescription
			{
				BufferCount = 1,
				IsWindowed = true,
				SwapEffect = SwapEffect.Discard,
				OutputHandle = form.Handle,
				Usage = Usage.RenderTargetOutput,
				ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
				SampleDescription = new SampleDescription(4, 4)
			};

			var levels = new[] { FeatureLevel.Level_11_0, FeatureLevel.Level_10_1, FeatureLevel.Level_10_0 };
			var dcflag = debug ? DeviceCreationFlags.Debug : DeviceCreationFlags.None;

			Device11.CreateWithSwapChain(DriverType.Hardware, dcflag, levels, scdesc, out _device, out _swapChain);

			_deviceContext = Device.ImmediateContext;
			using(var factory = SwapChain.GetParent<Factory>())
				factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAltEnter);

			form.UserResized += (sender, args) => AwaitingResize = true;
		}

		public void Resize()
		{
			Utilities.Dispose(ref _backBufferView);
			Utilities.Dispose(ref _zBufferView);

			SwapChain.ResizeBuffers(1, Form.ClientSize.Width, Form.ClientSize.Height, Format.B8G8R8A8_UNorm, SwapChainFlags.AllowModeSwitch);

			var backBufferTexture = _swapChain.GetBackBuffer<Texture2D>(0);
			_backBufferView = new RenderTargetView(_device, backBufferTexture);

			var zBufferTextureDescription = new Texture2DDescription
			{
				Format = Format.D16_UNorm,
				ArraySize = 1,
				MipLevels = 1,
				Width = Form.ClientSize.Width,
				Height = Form.ClientSize.Height,
				SampleDescription = new SampleDescription(1, 0),
				Usage = ResourceUsage.Default,
				BindFlags = BindFlags.DepthStencil,
				CpuAccessFlags = CpuAccessFlags.None,
				OptionFlags = ResourceOptionFlags.None
			};
			using(var zBufferTexture = new Texture2D(_device, zBufferTextureDescription))
				_zBufferView = new DepthStencilView(_device, zBufferTexture);

			SetDefaultTargets();
			AwaitingResize = false;
		}

		public void SetDefaultTargets()
		{
			DeviceContext.Rasterizer.SetViewport(0, 0, Form.ClientSize.Width, Form.ClientSize.Height);
			DeviceContext.OutputMerger.SetTargets(_zBufferView, _backBufferView);
		}

		public void Clear(Color4 color)
		{
			_deviceContext.ClearRenderTargetView(_backBufferView, color);
			_deviceContext.ClearDepthStencilView(_zBufferView, DepthStencilClearFlags.Depth, 1f, 0);
		}

		public void Present()
		{
			_swapChain.Present(0, PresentFlags.None);
		}

		public void SetDefaultRasterState()
		{
			Utilities.Dispose(ref _rasterState);
			var rdesc = RasterizerStateDescription.Default();
			_rasterState = new RasterizerState(_device, rdesc);
			DeviceContext.Rasterizer.State = _rasterState;
		}

		public void SetWireframeRasterState()
		{
			Utilities.Dispose(ref _rasterState);
			var rdesc = RasterizerStateDescription.Default();
			rdesc.FillMode = FillMode.Wireframe;
			_rasterState = new RasterizerState(_device, rdesc);
			DeviceContext.Rasterizer.State = _rasterState;
		}

		public void Dispose()
		{
			_backBufferView.Dispose();
			_zBufferView.Dispose();
			_swapChain.Dispose();
			_deviceContext.Dispose();
			_device.Dispose();
		}
	}
}
