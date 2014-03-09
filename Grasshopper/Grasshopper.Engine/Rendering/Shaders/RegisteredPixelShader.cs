using System;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace Grasshopper.Engine.Rendering.Shaders
{
	public class RegisteredPixelShader : IDisposable
	{
		private bool _isDisposed;

		public RegisteredPixelShader(ShaderBytecode bytecode)
		{
			Bytecode = bytecode;
		}

		public ShaderBytecode Bytecode { get; private set; }
		public PixelShader Shader { get; private set; }

		public void Create(Device device)
		{
			if(_isDisposed)
				return;
			if(Shader != null)
				Shader.Dispose();
			Shader = new PixelShader(device, Bytecode);
		}

		public void Dispose()
		{
			if(_isDisposed)
				return;
			_isDisposed = true;
			Bytecode.Dispose();
			if(Shader != null)
				Shader.Dispose();
		}
	}
}