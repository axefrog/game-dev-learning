using System;
using System.Collections.Generic;
using Grasshopper.Engine.Geometry;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Grasshopper.Engine.Rendering
{
	public class RenderManager : Component
	{
		private readonly GrasshopperApp _app;
		private Buffer _frameBuffer;

		public Camera Camera { get; set; }

		//public Matrix ViewProjectionMatrix { get; private set; }

		public Matrix WorldMatrix { get; set; }
		public Matrix ViewMatrix { get; set; }
		public Matrix ProjectionMatrix { get; set; }

		public RenderManager(GrasshopperApp app)
		{
			_app = app;
			_app.Pipeline.DeviceChanged += OnDeviceChanged;
			
			WorldMatrix = Matrix.Identity;
			ViewMatrix = Matrix.Identity;
		}

		private void OnDeviceChanged()
		{
			RemoveAndDispose(ref _frameBuffer);

			var device = _app.DeviceManager.Direct3D.Device;
			_frameBuffer = ToDispose(new Buffer(device, Utilities.SizeOf<ConstantBuffers.PerFrame>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0));
			
			var context = _app.DeviceManager.Direct3D.Context;
			context.VertexShader.SetConstantBuffer(0, _frameBuffer);
			context.PixelShader.SetConstantBuffer(0, _frameBuffer);
		}

		public void RenderFrame(params Renderer[] renderers)
		{
			RenderFrame((IEnumerable<Renderer>)renderers);
		}

		public void RenderFrame(IEnumerable<Renderer> renderers)
		{
			ViewMatrix = Matrix.LookAtLH(Camera.Position, Camera.Target, Camera.Up);
			ProjectionMatrix = Matrix.PerspectiveFovLH(Camera.FieldOfView, _app.Width / (float)_app.Height, Camera.NearPlane, Camera.FarPlane);

			//var camPosition = Matrix.Transpose(Matrix.Invert(view)).Column4;

			var perFrame = new ConstantBuffers.PerFrame
			{

				//CameraPosition = new Vector3(camPosition.X, camPosition.Y, camPosition.Z),
				World = Matrix.Transpose(WorldMatrix),
				View = Matrix.Transpose(ViewMatrix),
				Projection = Matrix.Transpose(ProjectionMatrix)
			};

			_app.DeviceManager.Direct3D.Context.UpdateSubresource(ref perFrame, _frameBuffer);
			
			foreach(var renderer in renderers)
				renderer.Render();
		}
	}
}
