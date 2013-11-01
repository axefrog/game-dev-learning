using System;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDx4.Direct3D.Shaders;
using SharpDx4.Game.Geometry;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharpDx4.Direct3D
{
	public class ModelDeviceData : IDisposable
	{
		public Model Model { get; set; }
		public VertexBufferBinding VerticesBufferBinding { get; set; }
		public VertexBufferBinding InstancesBufferBinding { get; set; }
		public Buffer IndicesBuffer { get; set; }
		public ICompiledVertexShader VertexShader { get; set; }
		public ICompiledPixelShader PixelShader { get; set; }
			
		public void Dispose()
		{
			VerticesBufferBinding.Buffer.Dispose();
			InstancesBufferBinding.Buffer.Dispose();
			IndicesBuffer.Dispose();
		}

		public ModelDeviceData(Model model, Device device)
		{
			Model = model;
			var vertices = Helpers.CreateBuffer(device, BindFlags.VertexBuffer, model.Vertices);
			var instances = Helpers.CreateBuffer(device, BindFlags.VertexBuffer, Model.Instances.Select(m =>
			{
				var world = m.WorldMatrix;
				world.Transpose();
				return world;
			}).ToArray());

			VerticesBufferBinding = new VertexBufferBinding(vertices, Utilities.SizeOf<ColoredVertex>(), 0);
			InstancesBufferBinding = new VertexBufferBinding(instances, Utilities.SizeOf<Matrix>(), 0);
			IndicesBuffer = Helpers.CreateBuffer(device, BindFlags.IndexBuffer, model.Triangles);
		}
	}
}