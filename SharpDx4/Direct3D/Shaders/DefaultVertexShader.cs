using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDx4.Direct3D.Shaders
{
	public class DefaultVertexShader : ICompiledVertexShader
	{
		public VertexShader Shader { get; private set; }
		public InputLayout InputLayout { get; private set; }

		public DefaultVertexShader(Device device)
		{
			var shaderSource = Shaders.VShader; // I have stored the shader program in a resx file, to make loading it easier
			ShaderSignature signature;
			using (var bytecode = ShaderBytecode.Compile(shaderSource, "VShader", "vs_5_0"))
			{
				signature = ShaderSignature.GetInputSignature(bytecode);
				Shader = new VertexShader(device, bytecode);
			}
			var elements = new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
				new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0),
				new InputElement("INSTANCE", 0, Format.R32G32B32A32_Float, 0, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 1, Format.R32G32B32A32_Float, 16, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 2, Format.R32G32B32A32_Float, 32, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 3, Format.R32G32B32A32_Float, 48, 1, InputClassification.PerInstanceData, 1),
			};
			InputLayout = new InputLayout(device, signature, elements);
		}

		public void AssignToContext(DeviceContext context, ModelDeviceData modelData)
		{
			context.InputAssembler.InputLayout = InputLayout;
			context.InputAssembler.SetVertexBuffers(0, new[] { modelData.VerticesBufferBinding, modelData.InstancesBufferBinding });
			context.InputAssembler.SetIndexBuffer(modelData.IndexBuffer, Format.R32_UInt, 0);
			context.VertexShader.Set(Shader);
		}

		public void Dispose()
		{
			Shader.Dispose();
		}
	}
}
