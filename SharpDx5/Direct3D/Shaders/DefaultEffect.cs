using System.Linq;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDx5.Game.Geometry;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDx5.Direct3D.Shaders
{
	public class DefaultEffect : EffectBase
	{
		private readonly Effect _effect;
		private readonly EffectTechnique _technique;
		private readonly EffectPass[] _passes;
		private readonly InputLayout _inputLayout;
		private readonly EffectMatrixVariable _worldMatrix;
		private readonly EffectMatrixVariable _viewMatrix;
		private readonly EffectMatrixVariable _projectionMatrix;
		private readonly EffectShaderResourceVariable _texture;

		public DefaultEffect(Device device)
		{
			var effectSource = Shaders.ColorTech;
			using (var compilationResult = ShaderBytecode.Compile(effectSource, "fx_5_0", ShaderFlags.Debug))
			{
				_effect = new Effect(device, compilationResult);
				_technique = _effect.GetTechniqueByIndex(0);
				_passes = Enumerable.Range(0, _technique.Description.PassCount)
					.Select(i => _technique.GetPassByIndex(i))
					.ToArray();
				
				_worldMatrix = _effect.GetVariableByName("World").AsMatrix();
				_viewMatrix = _effect.GetVariableByName("View").AsMatrix();
				_projectionMatrix = _effect.GetVariableByName("Projection").AsMatrix();
				_texture = _effect.GetVariableByName("gDiffuseMap").AsShaderResource();
			}

			_inputLayout = new InputLayout(device, _passes[0].Description.Signature, new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
				new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0),
				new InputElement("INSTANCE", 0, Format.R32G32B32A32_Float, 0, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 1, Format.R32G32B32A32_Float, 16, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 2, Format.R32G32B32A32_Float, 32, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 3, Format.R32G32B32A32_Float, 48, 1, InputClassification.PerInstanceData, 1)
			});
		}

		public override void PrepareInputAssembler(DeviceContext context, ModelDeviceData modelData)
		{
			context.InputAssembler.InputLayout = _inputLayout;
			context.InputAssembler.SetVertexBuffers(0, new[] { modelData.VerticesBufferBinding, modelData.InstancesBufferBinding });
			context.InputAssembler.SetIndexBuffer(modelData.IndexBuffer, Format.R32_UInt, 0);
		}

		public override void Draw(DeviceContext context, ModelDeviceData modelData, long elapsedMilliseconds)
		{
			_texture.SetResource(modelData.TextureView);
			Draw(context, _passes, modelData, elapsedMilliseconds);
		}

		public override void UpdateConstants(ViewTransformMatrices matrices)
		{
			_worldMatrix.SetMatrix(matrices.World);
			_viewMatrix.SetMatrix(matrices.View);
			_projectionMatrix.SetMatrix(matrices.Projection);
		}

		public void Dispose()
		{
			_effect.Dispose();
		}
	}
}
