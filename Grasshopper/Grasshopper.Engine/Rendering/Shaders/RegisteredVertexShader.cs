using System;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace Grasshopper.Engine.Rendering.Shaders
{
	public class RegisteredVertexShader : IDisposable
	{
		private readonly ShaderSignature _signature;
		private readonly InputElement[] _inputElements;
		private bool _isDisposed;

		public RegisteredVertexShader(ShaderBytecode bytecode)
		{
			Bytecode = bytecode;
			_signature = ShaderSignature.GetInputSignature(Bytecode);
			_inputElements = new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
				new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
				new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0),
				new InputElement("TEXTUREUV", 0, Format.R32G32_Float, 40, 0),
				new InputElement("INSTANCE", 0, Format.R32G32B32A32_Float, 0, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 1, Format.R32G32B32A32_Float, 16, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 2, Format.R32G32B32A32_Float, 32, 1, InputClassification.PerInstanceData, 1),
				new InputElement("INSTANCE", 3, Format.R32G32B32A32_Float, 48, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WORLD", 0, Format.R32G32B32A32_Float, 64, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WORLD", 1, Format.R32G32B32A32_Float, 80, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WORLD", 2, Format.R32G32B32A32_Float, 96, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WORLD", 3, Format.R32G32B32A32_Float, 112, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WINVTRANS", 0, Format.R32G32B32A32_Float, 128, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WINVTRANS", 1, Format.R32G32B32A32_Float, 144, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WINVTRANS", 2, Format.R32G32B32A32_Float, 160, 1, InputClassification.PerInstanceData, 1),
				new InputElement("WINVTRANS", 3, Format.R32G32B32A32_Float, 176, 1, InputClassification.PerInstanceData, 1),
			};
		}

		public ShaderBytecode Bytecode { get; private set; }
		public VertexShader Shader { get; private set; }
		public InputLayout InputLayout { get; private set; }

		public void Create(Device device)
		{
			if(_isDisposed)
				return;
			if(Shader != null)
				Shader.Dispose();
			if(InputLayout != null)
				InputLayout.Dispose();
			Shader = new VertexShader(device, Bytecode);
			InputLayout = new InputLayout(device, Bytecode, _inputElements);
		}

		public void Dispose()
		{
			if(_isDisposed)
				return;
			_isDisposed = true;
			_signature.Dispose();
			Bytecode.Dispose();
			if(Shader != null)
				Shader.Dispose();
			if(InputLayout != null)
				InputLayout.Dispose();
		}
	}
}