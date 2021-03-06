﻿using System;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDx5.Direct3D.Shaders;
using SharpDx5.Game.Geometry;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharpDx5.Direct3D
{
	public class Renderer : IDisposable
	{
		private readonly RenderingDevice _device;
		private SceneDeviceData _sceneData;
		private ViewTransformMatrices _viewTransformMatrices;
		private Buffer _constantBuffer;

		private readonly EffectBase _defaultEffect;

		public Renderer(RenderingDevice device)
		{
			_device = device;
			_defaultEffect = new DefaultEffect(_device.Device);
			_device.SetTopology(PrimitiveTopology.TriangleList);
		}

		public void SetScene(Scene scene)
		{
			Utilities.Dispose(ref _sceneData);
			_sceneData = new SceneDeviceData(scene, _device.Device);
			InitializeViewTransformMatrixBuffer();
		}

		private readonly Stopwatch _fpsSw = Stopwatch.StartNew();
		private readonly Stopwatch _sw = Stopwatch.StartNew();
		private int _fps;
		private const float _spf = 1000f/60f;
		private float _nextFrameTimeStamp;
		public int FramesPerSecond { get; private set; }

		public void Draw()
		{
			if (_sw.ElapsedMilliseconds < _nextFrameTimeStamp)
				return;
			_nextFrameTimeStamp += _spf;
			if (_sw.ElapsedMilliseconds > _nextFrameTimeStamp)
				_nextFrameTimeStamp = _sw.ElapsedMilliseconds;

			if (_device.AwaitingResize)
			{
				_device.Resize();
				ResetViewTransformMatrices();
			}

			_device.Clear(Color.CornflowerBlue);
			UpdateViewTransformMatrixBuffer();

			foreach (var modelData in _sceneData.ModelRenderData.Values)
			{
				_defaultEffect.PrepareInputAssembler(_device.Context, modelData);
				_defaultEffect.Draw(_device.Context, modelData, _sw.ElapsedMilliseconds);
			}

			_device.Present();

			if (_fpsSw.ElapsedMilliseconds >= 1000)
			{
				FramesPerSecond = _fps;
				_fps = 0;
				_fpsSw.Restart();
			}
			else
				_fps++;
		}

		private void ResetViewTransformMatrices()
		{
			_viewTransformMatrices = new ViewTransformMatrices
			{
				World = Matrix.Identity,
				View = Matrix.Identity,
				Projection = Matrix.PerspectiveFovLH((float)Math.PI / 2f, _device.Width / _device.Height, _sceneData.Scene.Camera.NearPlane, _sceneData.Scene.Camera.FarPlane),
			};
		}

		private void InitializeViewTransformMatrixBuffer()
		{
			Utilities.Dispose(ref _constantBuffer);
			ResetViewTransformMatrices();

			var sizeOfCoreMatrices = Utilities.SizeOf<ViewTransformMatrices>();
			_constantBuffer = new Buffer(_device.Device, sizeOfCoreMatrices, ResourceUsage.Default, BindFlags.ConstantBuffer,
				CpuAccessFlags.None, ResourceOptionFlags.None, sizeOfCoreMatrices);
			
			_device.Context.VertexShader.SetConstantBuffer(0, _constantBuffer);
		}

		private void UpdateViewTransformMatrixBuffer()
		{
			var camera = _sceneData.Scene.Camera;
			_viewTransformMatrices.View = Matrix.LookAtLH(camera.EyePosition, camera.TargetPosition, camera.UpVector);

			var matrices = new ViewTransformMatrices
			{
				World = _viewTransformMatrices.World,
				View = _viewTransformMatrices.View,
				Projection = _viewTransformMatrices.Projection,
			};

			_defaultEffect.UpdateConstants(matrices);
			//_device.Context.UpdateSubresource(ref matrices, _constantBuffer);
		}

		public void Dispose()
		{
			_sceneData.Dispose();
		}
	}
}
