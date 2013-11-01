using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDx4.Direct3D.Shaders;
using SharpDx4.Game.Geometry;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharpDx4.Direct3D
{
	public class ModelRenderData : IDisposable
	{
		public Model Model { get; set; }
		public VertexBufferBinding VerticesBufferBinding { get; set; }
		public Buffer IndicesBuffer { get; set; }
		public ICompiledVertexShader VertexShader { get; set; }
		public ICompiledPixelShader PixelShader { get; set; }
			
		public void Dispose()
		{
			VerticesBufferBinding.Buffer.Dispose();
			IndicesBuffer.Dispose();

		}

		public ModelRenderData(Model model, Device device)
		{
			Model = model;
			var vertices = Helpers.CreateBuffer(device, BindFlags.VertexBuffer, model.Vertices);
			VerticesBufferBinding = new VertexBufferBinding(vertices, Utilities.SizeOf<ColoredVertex>(), 0);
			IndicesBuffer = Helpers.CreateBuffer(device, BindFlags.IndexBuffer, model.Triangles);
		}
	}
}