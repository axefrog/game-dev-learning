using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SharpDx4.Direct3D.Shaders
{
	class DefaultPixelShader : ICompiledPixelShader
	{
		public DefaultPixelShader(Device device)
		{
			var shaderSource = Shaders.PShader; // I have stored the shader program in a resx file, to make loading it easier
			using (var bytecode = ShaderBytecode.Compile(shaderSource, "PShader", "ps_4_0"))
				Shader = new PixelShader(device, bytecode);
		}

		public PixelShader Shader { get; set; }
	
		public void Dispose()
		{
			Shader.Dispose();
		}
	}
}