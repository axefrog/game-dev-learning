using System;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDx5.Direct3D.Shaders;
using SharpDx5.Game.Geometry;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharpDx5.Direct3D
{
	public class ModelDeviceData : IDisposable
	{
		public Model Model { get; set; }
		public VertexBufferBinding VerticesBufferBinding { get; set; }
		public VertexBufferBinding InstancesBufferBinding { get; set; }
		public Buffer IndexBuffer { get; set; }
		public ICompiledVertexShader VertexShader { get; set; }
		public ICompiledPixelShader PixelShader { get; set; }
			
		public void Dispose()
		{
			VerticesBufferBinding.Buffer.Dispose();
			InstancesBufferBinding.Buffer.Dispose();
			IndexBuffer.Dispose();
		}

		public ModelDeviceData(Model model, Device device)
		{
			Model = model;
			var vertices = Helpers.CreateBuffer(device, ResourceUsage.Default, BindFlags.VertexBuffer, model.Vertices);
			var instances = Helpers.CreateBuffer(device, ResourceUsage.Dynamic, BindFlags.VertexBuffer, Model.Instances.Select(m => Matrix.Transpose(m.WorldMatrix)).ToArray());

			VerticesBufferBinding = new VertexBufferBinding(vertices, Utilities.SizeOf<ColoredVertex>(), 0);
			InstancesBufferBinding = new VertexBufferBinding(instances, Utilities.SizeOf<Matrix>(), 0);
			IndexBuffer = Helpers.CreateBuffer(device, ResourceUsage.Default, BindFlags.IndexBuffer, model.Triangles);
		}

		public void Draw(DeviceContext context, long elapsedMilliseconds)
		{
			Model.Update(elapsedMilliseconds);
			Helpers.UpdateDynamicBuffer(context.Device, InstancesBufferBinding.Buffer, Model.Instances.Select(m => Matrix.Transpose(m.WorldMatrix)).ToArray());
			context.DrawIndexedInstanced(Model.Triangles.Length*3, Model.Instances.Count, 0, 0, 0);
		}
	}
}