using System;
using System.Linq;
using Grasshopper.Engine.Geometry;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Grasshopper.Engine.Rendering
{
	public class ModelRenderer : Renderer
	{
		private readonly Model _model;
		private bool _dirty;

		private Buffer _vertexBuffer;
		private Buffer _indexBuffer;
		private Buffer _instanceBuffer;
		
		private VertexBufferBinding _vertexBufferBinding;
		private VertexBufferBinding _instanceBufferBinding;

		public ModelRenderer(Model model)
		{
			_model = model;
			
			model.InstancesUpdated += OnModelInstancesUpdated;
		}

		private void OnModelInstancesUpdated()
		{
			_dirty = true;
		}

		protected override void CreateDeviceDependentResources()
		{
			RemoveAndDispose(ref _vertexBuffer);
			RemoveAndDispose(ref _indexBuffer);
			RemoveAndDispose(ref _instanceBuffer);

			var device = App.DeviceManager.Direct3D.Device;

			_vertexBuffer = ToDispose(BufferHelper.CreateBuffer(device, ResourceUsage.Default, BindFlags.VertexBuffer, _model.Vertices));
			_indexBuffer = ToDispose(BufferHelper.CreateBuffer(device, ResourceUsage.Default, BindFlags.IndexBuffer, _model.Triangles));
			_instanceBuffer = ToDispose(BufferHelper.CreateBuffer(device, ResourceUsage.Dynamic, BindFlags.VertexBuffer, CreateInstanceBufferData()));

			_vertexBufferBinding = new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<Vertex>(), 0);
			_instanceBufferBinding = new VertexBufferBinding(_instanceBuffer, Utilities.SizeOf<ModelInstance>(), 0);
		}

		private ModelInstance[] CreateInstanceBufferData()
		{
			var renderManager = App.RenderManager;
			var data = _model.Instances.Select(m =>
			{
				var inst = new ModelInstance();
				inst.World = m.WorldMatrix * renderManager.WorldMatrix;
				inst.WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(m.WorldMatrix));
				inst.WorldViewProjection = m.WorldMatrix * renderManager.ViewProjectionMatrix;
				inst.Transpose();
				return inst;
			}).ToArray();
			return data;
		}

		protected override void OnRender()
		{
			var device = App.DeviceManager.Direct3D.Device;
			var context = App.DeviceManager.Direct3D.Context;

			var vertexShader = App.Shaders.GetVertexShader(_model.VertexShader);
			var pixelShader = App.Shaders.GetPixelShader(_model.PixelShader);

			context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			context.InputAssembler.InputLayout = vertexShader.InputLayout;
			context.InputAssembler.SetVertexBuffers(0, _vertexBufferBinding, _instanceBufferBinding);
			context.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R32_UInt, 0);

			context.VertexShader.Set(vertexShader.Shader);
			context.PixelShader.Set(pixelShader.Shader);

			var instances = CreateInstanceBufferData();
			BufferHelper.UpdateDynamicBuffer(device, _instanceBuffer, instances);

			context.DrawIndexedInstanced(_model.Triangles.Length * 3, _model.Instances.Count, 0, 0, 0);
		}
	}
}
