using System;
using SharpDX.Direct3D11;

namespace SharpDx5.Direct3D.Shaders
{
	public interface ICompiledVertexShader : IDisposable
	{
		VertexShader Shader { get; }
		InputLayout InputLayout { get; }
		void AssignToContext(DeviceContext context, ModelDeviceData modelData);
	}

	public interface ICompiledPixelShader : IDisposable
	{
		PixelShader Shader { get; set; }
	}
}