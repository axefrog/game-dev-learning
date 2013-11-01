using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDx4.Direct3D.Shaders
{
	public class DefaultVertexShader : ICompiledVertexShader
	{
		public DefaultVertexShader(Device device)
		{
			var shaderSource = Shaders.DefaultShaders; // I have stored the shader program in a resx file, to make loading it easier
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
				new InputElement("INSTANCE0", 0, Format.R32G32B32A32_Float, 0, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE1", 1, Format.R32G32B32A32_Float, 16, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE2", 2, Format.R32G32B32A32_Float, 32, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE3", 3, Format.R32G32B32A32_Float, 48, 1, InputClassification.PerInstanceData, 1),
			};
			InputLayout = new InputLayout(device, signature, elements);
		}

		public VertexShader Shader { get; private set; }
		public InputLayout InputLayout { get; private set; }

		public void Dispose()
		{
			Shader.Dispose();
		}
	}
}
