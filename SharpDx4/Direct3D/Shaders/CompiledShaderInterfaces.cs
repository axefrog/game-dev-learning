using System;
using SharpDX.Direct3D11;

namespace SharpDx4.Direct3D.Shaders
{
	public interface ICompiledVertexShader : IDisposable
	{
		VertexShader Shader { get; }
		InputLayout InputLayout { get; }
	}

	public interface ICompiledPixelShader : IDisposable
	{
		PixelShader Shader { get; set; }
	}
}